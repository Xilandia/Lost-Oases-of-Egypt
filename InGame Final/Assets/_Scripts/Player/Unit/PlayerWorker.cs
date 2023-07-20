using System;
using _Scripts.GameFlow.Objective;
using _Scripts.GameFlow.Sound;
using _Scripts.GameFlow.Transition;
using UnityEngine;
using UnityEngine.AI;
using _Scripts.Interaction.Interactable;
using _Scripts.Interaction.Management;
using _Scripts.Player.Management;
using _Scripts.Player.Structure;
using _Scripts.Player.Resources;
using _Scripts.Utility.Entity;
using _Scripts.Utility.Interface;
using _Scripts.Utility.Popup;
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
        public int workerOffset;
        public AudioClip workerGatherSound;
        public AudioClip workerDeathSound;
        public AudioClip workerJobDoneSound;

        public GameObject workerPrefab;
        public Transform workerTransform;
        [SerializeField] private Transform workerPopupSpawnPosition;
        public InteractableWorker interactable;

        private float distanceToResourceNode;

        private Collider[] rangeColliders;
        public bool isAttemptingToGather;
        public bool isAttemptingToBuild;
        public bool isConstructing;
        public bool isBuildingTower;
        public bool isBuildingStatue;
        public Transform resourceTarget;
        public ResourceNode resourceNode;
        public Transform structureTarget;
        public PlayerTower constructionTower;
        public PlayerBarracks constructionBarracks;
        public Statue constructionStatue;
        
        public PlayerBarracks[] buildableBarracks;
        public PlayerTower[] buildableTowers;
        public EntityUI[] buildableStructureInfo;
        
        private int hasLifeHash;
        private int isMovingHash;
        private int inRangeHash;

        private float timer;
        private float amountGathered;

        void Start()
        {
            navAgent.speed = workerMoveSpeed;
            navAgent.acceleration = workerMoveSpeed;
            rangeCollider.radius = workerOperationRange;
            MoveWorker(transform.position + new Vector3(1, 0, 1));
            
            hasLifeHash = Animator.StringToHash("HasLife");
            isMovingHash = Animator.StringToHash("IsMoving");
            inRangeHash = Animator.StringToHash("InRange");
            animator.SetBool(hasLifeHash, true);
        }

        void Update()
        {
            animator.SetBool(isMovingHash, navAgent.velocity.magnitude > 0.000001f);

            if (isAttemptingToGather)
            {
                ConsiderGathering();

                if (PlayerManager.instance.roundTimer[2] - timer >= 0.25)
                {
                    timer = PlayerManager.instance.roundTimer[2];
                    MoveToResourceTarget();
                }
            }

            if (isConstructing)
            {
                TickConstructionProject();
            }
        }

        public void MoveWorker(Vector3 _destination)
        {
            navAgent.SetDestination(_destination);
            isAttemptingToGather = false;
        }
        
        public void CheckForResourceTargets()
        {
            rangeColliders =
                Physics.OverlapSphere(transform.position, workerOperationRange * 2, EntityHandler.instance.resourceLayer);

            foreach (Collider col in rangeColliders)
            {
                isAttemptingToGather = true;
                resourceTarget = col.transform;
                resourceNode = col.gameObject.GetComponent<ResourceNode>();
                PopupHandler.instance.CreatePopup("!Resource node sighted!", Color.black, workerPopupSpawnPosition.position);

                break;
            }

            if (!isAttemptingToGather)
            {
                SoundHandler.instance.PlaySoundEffect(workerJobDoneSound);
            }
        }

        private void MoveToResourceTarget()
        {
            if (resourceTarget == null)
            {
                isAttemptingToGather = false;
                animator.SetBool(inRangeHash, false);
                animator.SetBool(isMovingHash, false);
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
            if (distanceToResourceNode <= workerOperationRange + resourceNode.offset && PlayerManager.instance.canGatherResources)
            {
                animator.SetBool(inRangeHash, true);
                animator.SetBool(isMovingHash, false);
                
                if (workerGatherCooldown <= 0)
                {
                    SoundHandler.instance.PlaySoundEffect(workerGatherSound);
                    workerGatherCooldown = workerGatherSpeed;
                    if (resourceNode != null)
                    {
                        switch (resourceNode.resourceType)
                        {
                            case ResourceNode.ResourceTypes.Ore:
                                amountGathered = resourceNode.GatherResource();
                                PlayerManager.instance.PlayerOre += amountGathered;
                                PopupHandler.instance.CreatePopup("+" + amountGathered + " Ore!", Color.green, workerPopupSpawnPosition.position);
                                ScoreHandler.instance.ResourceCollected("Ore", amountGathered);
                                break;
                            case ResourceNode.ResourceTypes.Wood:
                                amountGathered = resourceNode.GatherResource();
                                PlayerManager.instance.PlayerWood += amountGathered;
                                PopupHandler.instance.CreatePopup("+" + amountGathered + " Wood!", Color.green, workerPopupSpawnPosition.position);
                                ScoreHandler.instance.ResourceCollected("Wood", amountGathered);
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
            else if (isBuildingStatue)
            {
                constructionStatue.TickConstruction();
            }
            else
            {
                constructionBarracks.TickConstruction();
            }
        }

        public void TakeDamage(float damage)
        {
            float totalDamage = Math.Max(damage - workerArmor, 1);
            workerCurrentHealth -= totalDamage;
            PopupHandler.instance.CreatePopup("-" + totalDamage + " Health!", Color.red, workerPopupSpawnPosition.position);

            CheckIfDead();
        }
        
        public int GetOffset()
        {
            return workerOffset;
        }

        private void CheckIfDead()
        {
            if (workerCurrentHealth <= 0)
            {
                InputHandler.instance.selectedWorkers.Remove(this);
            
                PlayerManager.instance.workers.Remove(this);

                animator.SetBool(hasLifeHash, false);
                SoundHandler.instance.PlaySoundEffect(workerDeathSound);
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Utilities.IsInLayerMask(other.gameObject.layer, EntityHandler.instance.resourceLayer))
            {
                if (!isAttemptingToGather && !isAttemptingToBuild)
                {
                    isAttemptingToGather = true;
                    resourceTarget = other.transform;
                    resourceNode = other.gameObject.GetComponent<ResourceNode>();
                    distanceToResourceNode = Vector3.Distance(transform.position, resourceTarget.position);
                    PopupHandler.instance.CreatePopup("!Resource node sighted!", Color.black, workerPopupSpawnPosition.position);
                }
            }
            else if (Utilities.IsInLayerMask(other.gameObject.layer, EntityHandler.instance.playerInteractableLayer))
            {
                if (other.CompareTag("Tower"))
                {
                    if (isAttemptingToBuild)
                    {
                        isConstructing = true;
                        navAgent.SetDestination((transform.position + 4 * structureTarget.position) / 5);
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
                            structureTarget = tower.transform;
                            constructionTower.workersInvolvedInConstruction.Add(this);
                            navAgent.SetDestination((transform.position + 4 * structureTarget.position) / 5);
                        }
                    }
                }
                else if (other.CompareTag("Barracks"))
                {
                    if (isAttemptingToBuild)
                    {
                        isConstructing = true;
                        navAgent.SetDestination((transform.position + 4 * structureTarget.position) / 5);
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
                            structureTarget = barracks.transform;
                            constructionBarracks.workersInvolvedInConstruction.Add(this);
                            navAgent.SetDestination((transform.position + 4 * structureTarget.position) / 5);
                        }
                    }
                }
            }
            else if (Utilities.IsInLayerMask(other.gameObject.layer, EntityHandler.instance.objectiveLayer))
            {
                if (other.CompareTag("Statue"))
                {
                    if (StageTransitionHandler.instance.currentStage == 4)
                    {
                        isConstructing = true;
                        isBuildingStatue = true;
                        isAttemptingToBuild = true;
                        structureTarget = other.transform;
                        constructionStatue = other.GetComponent<Statue>();
                        constructionStatue.workersInvolvedInConstruction.Add(this);
                        navAgent.SetDestination((transform.position + 3 * structureTarget.position) / 4);
                    }
                }
            }
        }
    }
}
