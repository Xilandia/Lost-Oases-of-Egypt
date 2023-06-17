using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using _Scripts.Interaction.Interactable;
using _Scripts.Interaction.Management;
using _Scripts.Player.Management;
using _Scripts.Player.Structure;
using _Scripts.Player.Resources;
using _Scripts.Utility.Entity;
using _Scripts.Utility.Interface;
using _Scripts.Utility.Static;

namespace _Scripts.Player.Unit
{
    public class PlayerWorker : MonoBehaviour, IDamageable
    {
        [SerializeField] private NavMeshAgent navAgent;
        [SerializeField] private SphereCollider rangeCollider;
        [SerializeField] private Animator animator;

        public string workerName;

        public float workerHealth,
            workerArmor,
            workerOperationRange,
            workerMoveSpeed,
            workerGatherSpeed;
        public float workerCurrentHealth;
        public float workerGatherCooldown;

        public GameObject workerPrefab;
        public Transform workerTransform;
        public InteractableWorker interactable;

        private float distanceToResourceNode;

        private Collider[] rangeColliders;
        public bool isAttemptingToGather;
        public bool isAttemptingToBuild;
        public bool isConstructing;
        public bool isBuildingTower;
        public Transform resourceTarget;
        public ResourceNode resourceNode;
        public Transform structureTarget;
        public PlayerTower constructionTower;
        public PlayerBarracks constructionBarracks;
        
        public PlayerBarracks[] buildableBarracks;
        public PlayerTower[] buildableTowers;
        public string[] buildableStructureNames;
        
        private int hasLifeHash;
        private int isMovingHash;
        private int inRangeHash;

        void Start()
        {
            navAgent.speed = workerMoveSpeed;
            rangeCollider.radius = workerOperationRange;
            
            hasLifeHash = Animator.StringToHash("HasLife");
            isMovingHash = Animator.StringToHash("IsMoving");
            inRangeHash = Animator.StringToHash("InRange");
            animator.SetBool(hasLifeHash, true);
        }

        void Update()
        {
            HandleHealth();
            animator.SetBool(isMovingHash, navAgent.velocity.magnitude > 0.0001f);

            if (isAttemptingToGather)
            {
                MoveToResourceTarget();
                ConsiderGathering();
            }

            if (isConstructing)
            {
                TickConstructionProject();
            }
        }

        public void MoveWorker(Vector3 _destination)
        {
            navAgent.SetDestination(_destination);
        }
        
        public void CheckForResourceTargets()
        {
            rangeColliders =
                Physics.OverlapSphere(transform.position, workerOperationRange, EntityHandler.instance.resourceLayer);

            foreach (Collider col in rangeColliders)
            {
                isAttemptingToGather = true;
                resourceTarget = col.transform;
                resourceNode = col.gameObject.GetComponent<ResourceNode>();

                break;
            }
        }

        private void MoveToResourceTarget()
        {
            if (resourceTarget == null)
            {
                isAttemptingToGather = false;
                CheckForResourceTargets();
            }
            else
            {
                distanceToResourceNode = Vector3.Distance(transform.position, resourceTarget.position);
                
                navAgent.SetDestination(resourceTarget.position);
            }
        }

        private void ConsiderGathering()
        {
            if (distanceToResourceNode <= workerOperationRange + 1)
            {
                animator.SetBool(inRangeHash, true);
                animator.SetBool(isMovingHash, false);
                
                if (workerGatherCooldown <= 0)
                {
                    workerGatherCooldown = workerGatherSpeed;
                    if (resourceNode != null)
                    {
                        switch (resourceNode.resourceType)
                        {
                            case ResourceNode.ResourceTypes.Ore:
                                PlayerManager.instance.playerOre += resourceNode.GatherResource();
                                break;
                            case ResourceNode.ResourceTypes.Wood:
                                PlayerManager.instance.playerWood += resourceNode.GatherResource();
                                break;
                        }
                    }
                    else
                    {
                        Debug.Log("Has target, but no resource node component");
                    }
                }
                else
                {
                    workerGatherCooldown -= Time.deltaTime;
                }
            }
            else
            {
                workerGatherCooldown = (workerGatherCooldown + workerGatherSpeed) / 2;
                animator.SetBool(inRangeHash, false);
                animator.SetBool(isMovingHash, true);
            }
        }

        private void TickConstructionProject()
        {
            if (isBuildingTower)
            {
                constructionTower.TickConstruction();
            }
            else
            {
                constructionBarracks.TickConstruction();
            }
        }

        public void TakeDamage(float damage)
        {
            float totalDamage = damage - workerArmor;
            workerCurrentHealth -= Math.Max(totalDamage, 1);
        }

        private void HandleHealth()
        {
            if (workerCurrentHealth <= 0)
            {
                UnitDeath();
            }
        }

        private void UnitDeath()
        {
            InputHandler.instance.selectedWorkers.Remove(this); // uncomment once supported properly in InputHandler
            
            PlayerManager.instance.workers.Remove(this);

            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Utilities.IsInLayerMask(other.gameObject.layer, EntityHandler.instance.resourceLayer)) // change to resource layer
            {
                if (!isAttemptingToGather && !isAttemptingToBuild)
                {
                    isAttemptingToGather = true;
                    resourceTarget = other.transform;
                    resourceNode = other.gameObject.GetComponent<ResourceNode>();
                }
            }
            else if (Utilities.IsInLayerMask(other.gameObject.layer, EntityHandler.instance.playerInteractableLayer))
            {
                if (other.CompareTag("Tower"))
                {
                    if (isAttemptingToBuild)
                    {
                        isConstructing = true;
                        navAgent.SetDestination(transform.position);
                    }
                    else
                    {
                        PlayerTower tower = other.GetComponent<PlayerTower>();
                        if (tower.isPlaced)
                        {
                            constructionTower = tower;
                            isAttemptingToBuild = true;
                            isConstructing = true;
                            isBuildingTower = true;
                            constructionTower.workersInvolvedInConstruction.Add(this);
                            navAgent.SetDestination(transform.position);
                        }
                    }
                }
                else if (other.CompareTag("Barracks"))
                {
                    if (isAttemptingToBuild)
                    {
                        isConstructing = true;
                        navAgent.SetDestination(transform.position);
                    }
                    else
                    {
                        PlayerBarracks barracks = other.GetComponent<PlayerBarracks>();
                        if (barracks.isPlaced)
                        {
                            constructionBarracks = barracks;
                            isAttemptingToBuild = true;
                            isConstructing = true;
                            isBuildingTower = false;
                            constructionBarracks.workersInvolvedInConstruction.Add(this);
                            navAgent.SetDestination(transform.position);
                        }
                    }
                }
            }
        }
    }
}
