using System;
using UnityEngine;
using UnityEngine.AI;
using _Scripts.Enemy.Management;
using _Scripts.GameFlow.Sound;
using _Scripts.Player.Management;
using _Scripts.Player.Structure;
using _Scripts.Player.Unit;
using _Scripts.Utility.Static;
using _Scripts.Utility.Entity;
using _Scripts.Utility.Interface;

namespace _Scripts.Enemy.Unit
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyUnit : MonoBehaviour, IDamageable
    {

        [SerializeField] NavMeshAgent navAgent;
        [SerializeField] private SphereCollider rangeCollider;
        [SerializeField] private Animator animator;

        private Collider[] rangeColliders;
        private Transform targetTransform;
        private IDamageable targetDamageable;
        private bool hasTarget;
        private float distanceToTarget;
        private float distanceToClosestTarget;
        public bool behaviorOne = true;

        public string enemyName;

        public float enemyHealth,
            enemyArmor,
            enemyAttack,
            enemyTimeBetweenAttacks,
            enemyAttackRange,
            enemyMoveSpeed,
            enemyAggroRange;

        public float enemyCurrentHealth;
        public float enemyAttackCooldown;
        public AudioClip enemyAttackSound;
        public AudioClip enemyDeathSound;

        public GameObject enemyPrefab;
        public Transform enemyTransform;

        public delegate void OnDisableCallback(EnemyUnit Instance);

        public OnDisableCallback disable;

        private int hasLifeHash;
        private int isMovingHash;
        private int inRangeHash;
        
        void Start() // might need to change this to awake
        {
            navAgent.enabled = true;
            navAgent.SetDestination(EnemySpawnManager.instance.enemyBehaviorTransitionTransform.position);
            navAgent.speed = enemyMoveSpeed;
            rangeCollider.radius = enemyAggroRange;
            hasLifeHash = Animator.StringToHash("HasLife");
            isMovingHash = Animator.StringToHash("IsMoving");
            inRangeHash = Animator.StringToHash("InRange");
            animator.SetBool(hasLifeHash, true);
            //CheckForPlayerTargets(); might not need if the collider automatically checks on awake
        }

        void Update()
        {
            HandleHealth();
            animator.SetBool(isMovingHash, navAgent.velocity.magnitude > 0.0001f);

            if (hasTarget)
            {
                MoveToAggroTarget();
                ConsiderAttacking();
            }
        }

        private void MoveToAggroTarget()
        {
            if (targetTransform == null)
            {
                hasTarget = false;
                if (behaviorOne)
                {
                    CheckForPlayerTargets();
                }
                else
                {
                    CheckForClosestTarget();
                }
            }
            else
            {
                distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

                if (distanceToTarget <= enemyAggroRange + 2 || !behaviorOne) // range offset or in second stage of behavior
                {
                    navAgent.SetDestination(targetTransform.position);
                }
                else
                {
                    targetTransform = null;
                    hasTarget = false;
                    Debug.Log("Entered aggro range, but target is out of range");
                }
            }
        }

        private void ConsiderAttacking()
        {
            if (distanceToTarget <= enemyAttackRange + 1)
            {
                animator.SetBool(inRangeHash, true);
                animator.SetBool(isMovingHash, false);
                
                if (enemyAttackCooldown <= 0)
                {
                    enemyAttackCooldown = enemyTimeBetweenAttacks;
                    if (targetDamageable != null)
                    {
                        targetDamageable.TakeDamage(enemyAttack);
                        SoundHandler.instance.PlaySoundEffect(enemyAttackSound);
                    }
                    else
                    {
                        Debug.Log("Has target, but no damageable component");
                    }
                }
                else
                {
                    enemyAttackCooldown -= Time.deltaTime;
                }
            }
            else
            {
                enemyAttackCooldown = (enemyAttackCooldown + enemyTimeBetweenAttacks) / 2;
                animator.SetBool(inRangeHash, false);
                animator.SetBool(isMovingHash, true);
            }
        }

        private void CheckForPlayerTargets()
        {
            rangeColliders = Physics.OverlapSphere(transform.position, enemyAggroRange,
                EntityHandler.instance.playerInteractableLayer);

            foreach (Collider col in rangeColliders)
            {
                hasTarget = true;
                targetTransform = col.transform;
                if (col.gameObject.tag.Equals("Unit"))
                {
                    targetDamageable = targetTransform.gameObject.GetComponent<PlayerUnit>();
                }
                else if (col.gameObject.tag.Equals("Trainer"))
                {
                    targetDamageable = targetTransform.gameObject.GetComponent<PlayerBarracks>();
                }

                navAgent.stoppingDistance = enemyAttackRange;

                break;
            }

            if (!hasTarget)
            {
                navAgent.SetDestination(EnemySpawnManager.instance.enemyBehaviorTransitionTransform.position);
                targetTransform = null;
                navAgent.stoppingDistance = 0;
            }
        }

        public void TransitionPhase()
        {
            behaviorOne = false;
            navAgent.stoppingDistance = enemyAttackRange;
            CheckForClosestTarget();
        }

        private void CheckForClosestTarget()
        {
            hasTarget = false;
            distanceToClosestTarget = 1000;

            if (MeleeSoldierInRange())
            {

            }
            else if (RangedSoldierInRange())
            {

            }
            else if (TowerInRange())
            {

            }
            else if (BarracksInRange())
            {

            }
            else if (WorkerInRange())
            {

            }
            else
            {
                Debug.Log("No targets in lists");
            }
        }

        private bool MeleeSoldierInRange()
        {
            bool inRange = false;

            foreach (PlayerUnit pU in PlayerManager.instance.meleeSoldiers)
            {
                if (distanceToClosestTarget > Vector3.Distance(transform.position, pU.transform.position))
                {
                    distanceToClosestTarget = Vector3.Distance(transform.position, pU.transform.position);
                    targetTransform = pU.transform;
                    targetDamageable = pU;
                    hasTarget = true;
                    inRange = true;
                }
            }

            return inRange;
        }

        private bool RangedSoldierInRange()
        {
            bool inRange = false;

            foreach (PlayerUnit pU in PlayerManager.instance.rangedSoldiers)
            {
                if (distanceToClosestTarget > Vector3.Distance(transform.position, pU.transform.position))
                {
                    distanceToClosestTarget = Vector3.Distance(transform.position, pU.transform.position);
                    targetTransform = pU.transform;
                    targetDamageable = pU;
                    hasTarget = true;
                    inRange = true;
                }
            }

            return inRange;
        }

        private bool TowerInRange()
        {
            bool inRange = false;

            foreach (PlayerTower pT in PlayerManager.instance.towers)
            {
                if (distanceToClosestTarget > Vector3.Distance(transform.position, pT.transform.position))
                {
                    distanceToClosestTarget = Vector3.Distance(transform.position, pT.transform.position);
                    targetTransform = pT.transform;
                    targetDamageable = pT;
                    hasTarget = true;
                    inRange = true;
                }
            }

            return inRange;
        }

        private bool BarracksInRange()
        {
            bool inRange = false;

            foreach (PlayerBarracks pB in PlayerManager.instance.barracks)
            {
                if (distanceToClosestTarget > Vector3.Distance(transform.position, pB.transform.position))
                {
                    distanceToClosestTarget = Vector3.Distance(transform.position, pB.transform.position);
                    targetTransform = pB.transform;
                    targetDamageable = pB;
                    hasTarget = true;
                    inRange = true;
                }
            }

            return inRange;
        }

        private bool WorkerInRange()
        {
            bool inRange = false;

            foreach (PlayerWorker pW in PlayerManager.instance.workers)
            {
                if (distanceToClosestTarget > Vector3.Distance(transform.position, pW.transform.position))
                {
                    distanceToClosestTarget = Vector3.Distance(transform.position, pW.transform.position);
                    targetTransform = pW.transform;
                    targetDamageable = pW;
                    hasTarget = true;
                    inRange = true;
                }
            }

            return inRange;
        }


        public void TakeDamage(float damage)
        {
            float totalDamage = damage - enemyArmor;
            enemyCurrentHealth -= Math.Max(totalDamage, 1);
        }

        private void HandleHealth()
        {
            if (enemyCurrentHealth <= 0)
            {
                UnitDeath();
            }
        }

        private void UnitDeath()
        {
            animator.SetBool(hasLifeHash, false);
            SoundHandler.instance.PlaySoundEffect(enemyDeathSound);
            disable?.Invoke(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Utilities.IsInLayerMask(other.gameObject.layer, EntityHandler.instance.playerInteractableLayer))
            {
                if (!hasTarget)
                {
                    Debug.Log("No target now, giving target");
                    hasTarget = true;
                    targetTransform = other.transform;
                    
                    if (other.gameObject.tag.Equals("Unit"))
                    {
                        targetDamageable = targetTransform.gameObject.GetComponent<PlayerUnit>();
                    }
                    else if (other.gameObject.tag.Equals("Barracks"))
                    {
                        targetDamageable = targetTransform.gameObject.GetComponent<PlayerBarracks>();
                    }
                    else if (other.gameObject.tag.Equals("Tower"))
                    {
                        targetDamageable = targetTransform.gameObject.GetComponent<PlayerTower>();
                    }
                    else if (other.gameObject.tag.Equals("Worker"))
                    {
                        targetDamageable = targetTransform.gameObject.GetComponent<PlayerWorker>();
                    }

                    navAgent.stoppingDistance = enemyAttackRange;
                }
            }
        }
    }
}