using System;
using System.Collections.Generic;
using _Scripts.GameFlow.Sound;
using UnityEngine;
using _Scripts.Utility.Interface;
using _Scripts.Utility.Entity;
using _Scripts.Interaction.Interactable;
using _Scripts.Interaction.Management;
using _Scripts.Player.Management;
using _Scripts.Player.Unit;
using UnityEngine.AI;

namespace _Scripts.Player.Structure
{
    public class PlayerBarracks : MonoBehaviour, IDamageable
    {
        [SerializeField] private BoxCollider structureCollider;
        [SerializeField] private NavMeshObstacle navObstacle;

        public string barracksName;
        public float barracksCostOre, barracksCostWood, barracksHealth, barracksArmor, barracksBuildTime;
        public float barracksCurrentHealth;
        public AudioClip barracksBuildSound;
        public AudioClip barracksTrainSound;
        public AudioClip barracksDamagedSound;
        public AudioClip barracksDeathSound;

        public GameObject barracksPrefab;
        public Transform barracksTransform;
        public PlacableObject barracksPlacable;

        public bool isPrototype, isPlaced, isComplete;
        public EntityUnit[] buildableUnits;
        public EntityUI[] buildableUnitInfo;
        public InteractableBarracks interactable;

        private bool constructionStarted = false;
        private float currProgress, prevSoundEffect, initYScale;
        public List<PlayerWorker> workersInvolvedInConstruction = new List<PlayerWorker>();

        [SerializeField] private Transform spawnPoint;
        private readonly Queue<EntityUnit> unitQueue = new Queue<EntityUnit>();
        private float currentUnitTrainTime;
        private float elapsedTrainingTime;
        private bool isTraining;

        private Vector3 originalScale;

        void Update()
        {
            if (isTraining)
            {
                elapsedTrainingTime += Time.deltaTime;
                if (elapsedTrainingTime >= currentUnitTrainTime)
                {
                    elapsedTrainingTime = 0;
                    FinishTrainingUnit();
                }
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
            PlayerManager.instance.barracks.Add(this);

            foreach (PlayerWorker worker in InputHandler.instance.selectedWorkers)
            {
                workersInvolvedInConstruction.Add(worker);
                worker.isAttemptingToBuild = true;
                worker.isAttemptingToGather = false;
                worker.isBuildingTower = false;
                worker.structureTarget = gameObject.transform;
                worker.MoveWorker(worker.structureTarget.position);
                worker.constructionBarracks = this;
                worker.interactable.OnInteractExit();
            }
        }

        public void TickConstruction()
        {
            if (constructionStarted)
            {
                currProgress += Time.deltaTime;
                transform.localScale = new Vector3(originalScale.x, initYScale * (currProgress / barracksBuildTime),
                    originalScale.z);
                
                if (currProgress >= prevSoundEffect + 1f)
                {
                    prevSoundEffect = currProgress;
                    SoundHandler.instance.PlaySoundEffect(barracksBuildSound);
                }

                if (currProgress >= barracksBuildTime)
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

        public void AddToQueue(string unitName)
        {
            EntityUnit playerUnit = null;
            foreach (EntityUnit entity in buildableUnits)
            {
                if (entity.entityName == unitName)
                {
                    playerUnit = entity;
                    break;
                }
            }

            if (playerUnit == null)
            {
                Debug.LogError("PlayerTrainer.cs: AddToQueue(): playerUnit is null");
                return;
            }

            if (PlayerManager.instance.playerOre >= playerUnit.entityCostOre && PlayerManager.instance.playerWood >= playerUnit.entityCostWood)
            {
                PlayerManager.instance.playerOre -= playerUnit.entityCostOre;
                PlayerManager.instance.playerWood -= playerUnit.entityCostWood;
                
                if (unitQueue.Count == 0)
                {
                    unitQueue.Enqueue(playerUnit);
                    currentUnitTrainTime = playerUnit.entityCreationTime;
                    isTraining = true;
                }
                else
                {
                    unitQueue.Enqueue(playerUnit);
                }
            }
            else
            {
                Debug.Log("Not enough resources to train unit");
            }
        }

        private void FinishTrainingUnit()
        {
            EntityUnit entity = unitQueue.Dequeue();

            GameObject unit = Instantiate(entity.entityPrefab, spawnPoint.position, Quaternion.identity);
            unit.transform.SetParent(PlayerManager.instance.playerUnits);
            PlayerUnit pU = unit.GetComponent<PlayerUnit>();

            EntityHandler.instance.SetPlayerUnitStats(pU, entity.entityName);
            
            if (pU.unitAttackRange > 5)
            {
                PlayerManager.instance.rangedSoldiers.Add(pU);
            }
            else
            {
                PlayerManager.instance.meleeSoldiers.Add(pU);
            }
            
            if (unitQueue.Count > 0)
            {
                currentUnitTrainTime = unitQueue.Peek().entityCreationTime;
            }
            else
            {
                isTraining = false;
            }
            
            SoundHandler.instance.PlaySoundEffect(barracksTrainSound);
        }

        public void TakeDamage(float damage)
        {
            float totalDamage = damage - barracksArmor;
            barracksCurrentHealth -= Math.Max(totalDamage, 1);
            SoundHandler.instance.PlaySoundEffect(barracksDamagedSound);

            if (barracksCurrentHealth <= 0)
            {
                SoundHandler.instance.PlaySoundEffect(barracksDeathSound);
                Destroy(gameObject);
            }
        }
    }
}