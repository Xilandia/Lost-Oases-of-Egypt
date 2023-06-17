using System;
using System.Collections.Generic;
using _Scripts.Enemy.Unit;
using _Scripts.GameFlow.Sound;
using UnityEngine;
using _Scripts.Utility.Interface;
using _Scripts.Utility.Entity;
using _Scripts.Interaction.Interactable;
using _Scripts.Interaction.Management;
using _Scripts.Player.Management;
using _Scripts.Player.Unit;
using _Scripts.Utility.Static;
using UnityEngine.AI;

namespace _Scripts.Player.Structure
{
    public class PlayerTower : MonoBehaviour, IDamageable
    {
        [SerializeField] private BoxCollider structureCollider;
        [SerializeField] private SphereCollider rangeCollider;
        [SerializeField] private NavMeshObstacle navObstacle;

        public string towerName;
        public float towerCostOre, towerCostWood, towerHealth, towerArmor, towerAttack, towerTimeBetweenAttacks, towerAttackRange, towerBuildTime;
        public float towerCurrentHealth;
        public float towerAttackCooldown;
        public AudioClip towerAttackSound;
        public AudioClip towerBuildSound;
        public AudioClip towerDamagedSound;
        public AudioClip towerDeathSound;

        public GameObject towerPrefab;
        public Transform towerTransform;
        public PlacableObject towerPlacable;

        public bool isPrototype, isPlaced, isComplete;
        public InteractableTower interactable;

        private bool constructionStarted = false;
        private float currProgress, prevSoundEffect, initYScale;
        private Vector3 originalScale;
        public List<PlayerWorker> workersInvolvedInConstruction = new List<PlayerWorker>();
        
        private Collider[] rangeColliders;
        public Transform aggroTarget;
        public IDamageable aggroDamageable;
        public bool hasAggro = false;
        private float distanceToTarget;

        void Start()
        {
            rangeCollider.radius = towerAttackRange;
        }
        
        void Update()
        {

            if (hasAggro)
            {
                CheckAggroTarget();
                ConsiderAttack();
            }
        }

        public void UpdatePrototypePosition()
        {
            if (isPrototype)
            {
                transform.position = BuildingHandler.SnapToGrid(BuildingHandler.GetMouseWorldPosition());
            }
        }

        public void StartConstruction()
        {
            constructionStarted = true;
            isPlaced = true;
            isPrototype = false;
            currProgress = 0f;
            originalScale = transform.localScale;
            transform.localScale = new Vector3(originalScale.x, originalScale.y / 100, originalScale.z);
            interactable.OnInteractExit();
            navObstacle.enabled = true;
            structureCollider.enabled = true;
            
            foreach (PlayerWorker worker in InputHandler.instance.selectedWorkers)
            {
                workersInvolvedInConstruction.Add(worker);
                worker.isAttemptingToBuild = true;
                worker.isAttemptingToGather = false;
                worker.isBuildingTower = true;
                worker.structureTarget = gameObject.transform;
                worker.MoveWorker(worker.structureTarget.position);
                worker.constructionTower = this;
                worker.interactable.OnInteractExit();
            }
        }
        
        public void TickConstruction()
        {
            if (constructionStarted)
            {
                currProgress += Time.deltaTime;
                transform.localScale = new Vector3(originalScale.x, initYScale * (currProgress / towerBuildTime),
                    originalScale.z);

                if (currProgress >= prevSoundEffect + 1f)
                {
                    SoundHandler.instance.PlaySoundEffect(towerBuildSound);
                    prevSoundEffect = currProgress;
                }
                
                if (currProgress >= towerBuildTime)
                {
                    CompleteConstruction();
                }
            }
        }
        
        private void CompleteConstruction()
        {
            isPlaced = false;
            isComplete = true;
            transform.localScale = originalScale;
            rangeCollider.enabled = true;
            PlayerManager.instance.towers.Add(this);
            
            foreach (PlayerWorker worker in workersInvolvedInConstruction)
            {
                worker.isAttemptingToBuild = false;
                worker.isConstructing = false;
                worker.isAttemptingToGather = false;
                worker.isBuildingTower = false;
                worker.structureTarget = null;
                worker.constructionBarracks = null;
                
                worker.CheckForResourceTargets();
            }
            
            workersInvolvedInConstruction.Clear();
        }

        private void CheckAggroTarget()
        {
            if (aggroTarget == null)
            {
                hasAggro = false;
                CheckForEnemyTargets();
            }
            
            distanceToTarget = Vector3.Distance(transform.position, aggroTarget.position);

            if (distanceToTarget > towerAttackRange + 1) // range offset
            {
                aggroTarget = null;
                hasAggro = false;
            }
        }
        
        private void CheckForEnemyTargets()
        {
            rangeColliders =
                Physics.OverlapSphere(transform.position, towerAttackRange, EntityHandler.instance.enemyUnitLayer);

            foreach (Collider col in rangeColliders)
            {
                hasAggro = true;
                aggroTarget = col.transform;
                aggroDamageable = aggroTarget.gameObject.GetComponent<EnemyUnit>();

                break;
            }
        }
        
        private void ConsiderAttack()
        {
            if (distanceToTarget <= towerAttackRange + 1)
            {
                if (towerAttackCooldown <= 0)
                {
                    towerAttackCooldown = towerTimeBetweenAttacks;
                    if (aggroDamageable != null)
                    {
                        aggroDamageable.TakeDamage(towerAttack);
                        SoundHandler.instance.PlaySoundEffect(towerAttackSound);
                    }
                    else
                    {
                        Debug.Log("Has target, but no damageable component");
                    }
                }
                else
                {
                    towerAttackCooldown -= Time.deltaTime;
                }
            }
            else
            {
                towerAttackCooldown = (towerAttackCooldown + towerTimeBetweenAttacks) / 2;
            }
        }

        public void TakeDamage(float damage)
        {
            float totalDamage = damage - towerArmor;
            towerCurrentHealth -= Math.Max(totalDamage, 1);
            SoundHandler.instance.PlaySoundEffect(towerDamagedSound);

            if (towerCurrentHealth <= 0)
            {
                SoundHandler.instance.PlaySoundEffect(towerDeathSound);
                Destroy(gameObject);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (Utilities.IsInLayerMask(other.gameObject.layer, EntityHandler.instance.enemyUnitLayer))
            {
                if (!hasAggro)
                {
                    hasAggro = true;
                    aggroTarget = other.transform;
                    aggroDamageable = aggroTarget.gameObject.GetComponent<EnemyUnit>();
                }
            }
        }
    }
}