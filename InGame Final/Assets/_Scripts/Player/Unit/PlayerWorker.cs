using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using _Scripts.Interaction.Interactable;
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
        private NavMeshAgent navAgent;
        private Camera cam;

        public string workerName;

        public float workerHealth,
            workerArmor,
            workerOperationRange,
            workerMoveSpeed,
            workerGatherSpeed;
        public float workerCurrentHealth;
        public float workerGatherCooldown;

        public GameObject workerStatDisplay;
        public Image workerHealthBarImage;
        public GameObject workerPrefab;
        public Transform workerTransform;
        public InteractableWorker interactable;

        private float distanceToSite;

        private Collider[] rangeColliders;
        public bool isAttemptingToGather;
        public bool isAttemptingToBuild;
        public bool isBuildingTower;
        public Transform resourceTarget;
        public ResourceNode resourceNode;
        public Transform structureTarget;
        public PlayerTower constructionTower;
        public PlayerBarracks constructionBarracks;

        public PlayerTower[] buildableTowers;
        public PlayerBarracks[] buildableBarracks;
        public string[] buildableStructureNames;

        void Start()
        {
            navAgent = GetComponent<NavMeshAgent>();
            cam = Camera.main;
        }

        void Update()
        {
            HandleHealth();

            if (isAttemptingToGather)
            {
                MoveToResourceTarget();
                ConsiderGathering();
            }

            if (isAttemptingToBuild)
            {
                MoveToStructureTarget();
                ConsiderBuildTicking();
            }
        }

        public void MoveWorker(Vector3 _destination)
        {
            navAgent.SetDestination(_destination);
        }
        
        public void CheckForResourceTargets()
        {
            rangeColliders =
                Physics.OverlapSphere(transform.position, workerOperationRange, EntityHandler.instance.enemyUnitLayer);

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
                distanceToSite = Vector3.Distance(transform.position, resourceTarget.position);
                
                navAgent.SetDestination(resourceTarget.position);
            }
        }

        private void ConsiderGathering()
        {
            if (distanceToSite <= workerOperationRange + 1)
            {
                if (workerGatherCooldown <= 0)
                {
                    workerGatherCooldown = workerGatherSpeed;
                    if (resourceNode != null)
                    {
                        switch (resourceNode.resourceType)
                        {
                            case ResourceNode.ResourceTypes.Ore:
                                PlayerManager.instance.playerOre += resourceNode.resourceAmount;
                                break;
                            case ResourceNode.ResourceTypes.Wood:
                                PlayerManager.instance.playerWood += resourceNode.resourceAmount;
                                break;
                            case ResourceNode.ResourceTypes.Gold:
                                PlayerManager.instance.playerGold += resourceNode.resourceAmount;
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
            }
        }
        
        private void MoveToStructureTarget()
        {
            distanceToSite = Vector3.Distance(transform.position,  structureTarget.position);

            navAgent.SetDestination(resourceTarget.position);
        }

        private void ConsiderBuildTicking()
        {
            if (distanceToSite <= workerOperationRange + 1)
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
        }

        public void TakeDamage(float damage)
        {
            float totalDamage = damage - workerArmor;
            workerCurrentHealth -= Math.Max(totalDamage, 1);
        }

        private void HandleHealth()
        {
            workerStatDisplay.transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                cam.transform.rotation * Vector3.up);

            workerHealthBarImage.fillAmount = workerCurrentHealth / workerHealth;

            if (workerCurrentHealth <= 0)
            {
                UnitDeath();
            }
        }

        private void UnitDeath()
        {
            //InputHandler.instance.selectedWorkers.Remove(this); // uncomment once supported properly in InputHandler
            
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
        }
    }
}
