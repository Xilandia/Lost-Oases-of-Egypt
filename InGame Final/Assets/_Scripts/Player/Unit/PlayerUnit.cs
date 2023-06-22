using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using _Scripts.Enemy.Unit;
using _Scripts.GameFlow.Sound;
using _Scripts.Interaction.Interactable;
using _Scripts.Interaction.Management;
using _Scripts.Player.Management;
using _Scripts.Utility.Entity;
using _Scripts.Utility.Interface;
using _Scripts.Utility.Static;


namespace _Scripts.Player.Unit
{
    public class PlayerUnit : MonoBehaviour, IDamageable
    {
        [SerializeField] NavMeshAgent navAgent;
        [SerializeField] private SphereCollider rangeCollider;
        [SerializeField] private Animator animator;
        private Camera cam;

        public string unitName;

        public float unitCostOre,
            unitCostWood,
            unitHealth,
            unitArmor,
            unitAttack,
            unitTimeBetweenAttacks,
            unitAttackRange,
            unitAggroRange,
            unitMoveSpeed,
            unitTrainTime;

        public float unitCurrentHealth;
        public float unitAttackCooldown;
        public int unitOffset;
        public AudioClip unitAttackSound;
        public AudioClip unitDeathSound;

        public GameObject unitStatDisplay;
        public Image unitHealthBarImage;
        public GameObject unitPrefab;
        public Transform unitTransform;
        public InteractableUnit interactable;

        private Collider[] rangeColliders;
        public Vector3 moveTarget;
        public Transform aggroTarget;
        public IDamageable aggroDamageable;
        public int aggroOffset;
        public bool hasAggro;
        private float distanceToTarget;

        private int hasLifeHash;
        private int isMovingHash;
        private int inRangeHash;

        private float timer;

        void Start()
        {
            cam = Camera.main;
            navAgent.speed = unitMoveSpeed;
            navAgent.acceleration = unitMoveSpeed;
            navAgent.stoppingDistance = unitAttackRange;
            rangeCollider.radius = unitAggroRange;
            unitAttackRange *= transform.position.x;
            unitAggroRange *= transform.position.x;
            MoveUnit(transform.position + new Vector3(1, 0, 1));
            hasLifeHash = Animator.StringToHash("HasLife");
            isMovingHash = Animator.StringToHash("IsMoving");
            inRangeHash = Animator.StringToHash("InRange");
            animator.SetBool(hasLifeHash, true);
        }

        void Update()
        {
            animator.SetBool(isMovingHash, navAgent.velocity.magnitude > 0.000001f);

            if (hasAggro)
            {
                MoveToAggroTarget();
                ConsiderAttacking();

                if (PlayerManager.instance.roundTimer[2] - timer >= 0.25)
                {
                    timer = PlayerManager.instance.roundTimer[2];
                    MoveToAggroTarget();
                }
            }
        }

        public void MoveUnit(Vector3 _destination)
        {
            navAgent.SetDestination(_destination);
            moveTarget = _destination;
        }

        private void CheckForEnemyTargets()
        {
            rangeColliders =
                Physics.OverlapSphere(transform.position, unitAggroRange, EntityHandler.instance.enemyUnitLayer);

            foreach (Collider col in rangeColliders)
            {
                hasAggro = true;
                aggroTarget = col.transform;
                aggroDamageable = aggroTarget.gameObject.GetComponent<EnemyUnit>();
                aggroOffset = aggroDamageable.GetOffset();
                navAgent.stoppingDistance = unitAttackRange;

                break;
            }

            if (!hasAggro)
            {
                navAgent.SetDestination(moveTarget);
                aggroTarget = null;
                navAgent.stoppingDistance = 0;
            }
        }

        private void MoveToAggroTarget()
        {
            if (aggroTarget == null)
            {
                hasAggro = false;
                CheckForEnemyTargets();
            }
            else
            {
                distanceToTarget = Vector3.Distance(transform.position, aggroTarget.position);
                
                navAgent.SetDestination(aggroTarget.position);
            }
        }

        private void ConsiderAttacking()
        {
            if (distanceToTarget <= unitAttackRange + aggroOffset)
            {
                animator.SetBool(inRangeHash, true);
                animator.SetBool(isMovingHash, false);
                
                if (unitAttackCooldown <= 0)
                {
                    unitAttackCooldown = unitTimeBetweenAttacks;
                    if (aggroDamageable != null)
                    {
                        aggroDamageable.TakeDamage(unitAttack);
                        SoundHandler.instance.PlaySoundEffect(unitAttackSound);
                    }
                    else
                    {
                        Debug.Log("Has target, but no damageable component");
                    }
                }
                else
                {
                    unitAttackCooldown -= Time.deltaTime;
                }
            }
            else
            {
                unitAttackCooldown = (unitAttackCooldown + unitTimeBetweenAttacks) / 2;
                animator.SetBool(inRangeHash, false);
                animator.SetBool(isMovingHash, true);
            }
        }

        public void TakeDamage(float damage)
        {
            float totalDamage = damage - unitArmor;
            unitCurrentHealth -= Math.Max(totalDamage, 1);
            
            
            HandleHealth();
        }

        public int GetOffset()
        {
            return unitOffset;
        }

        private void HandleHealth()
        {
            unitStatDisplay.transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                cam.transform.rotation * Vector3.up);

            unitHealthBarImage.fillAmount = unitCurrentHealth / unitHealth;

            if (unitCurrentHealth <= 0)
            {
                UnitDeath();
            }
        }

        private void UnitDeath()
        {
            InputHandler.instance.selectedUnits.Remove(this);

            PlayerManager.instance.meleeSoldiers.Remove(this);
            PlayerManager.instance.rangedSoldiers.Remove(this);

            animator.SetBool(hasLifeHash, false);
            SoundHandler.instance.PlaySoundEffect(unitDeathSound);
            Destroy(gameObject);
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
                    aggroOffset = aggroDamageable.GetOffset();
                    navAgent.stoppingDistance = unitAttackRange;
                }
            }
        }
    }
}
