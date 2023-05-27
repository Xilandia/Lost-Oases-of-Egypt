using System;
using _Scripts;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyUnit : MonoBehaviour, Damagable
{
    
    [SerializeField] NavMeshAgent navAgent;
    [SerializeField] private SphereCollider rangeCollider;
    
    private Collider[] rangeColliders;
    private Transform aggroTarget;
    private Damagable aggroDamagable;
    private bool hasAggro;
    private float distanceToTarget;
    public bool stageOne = true;

    public string enemyName;
    public float enemyHealth, enemyArmor, enemyAttack, enemyTimeBetweenAttacks, enemyAttackRange, enemyMoveSpeed, enemyAggroRange;
    public float enemyCurrentHealth;
    public float enemyAttackCooldown;
    
    public GameObject enemyPrefab;
    public Transform enemyTransform;
    
    public delegate void OnDisableCallback(EnemyUnit Instance);
    public OnDisableCallback disable;
    
    void Start() // might need to change this to awake
    {
        navAgent.SetDestination(EnemySpawnManager.instance.enemyGoal.position);
        rangeCollider.radius = enemyAggroRange;
        //CheckForPlayerTargets(); might not need if the collider automatically checks on awake
    }

    void Update()
    {
        HandleHealth();

        if (stageOne)
        {
            if (hasAggro)
            {
                MoveToAggroTarget();
                ConsiderAttack();
            }
        }
    }
    
    private void MoveToAggroTarget()
    {
        if (aggroTarget.Equals(null))
        {
            hasAggro = false;
            CheckForPlayerTargets();
        }
        else
        {
            distanceToTarget = Vector3.Distance(transform.position, aggroTarget.position);
            
            if (distanceToTarget <= enemyAggroRange + 2) // range offset
            {
                navAgent.SetDestination(aggroTarget.position);
            }
            else
            {
                aggroTarget = null;
                hasAggro = false;
                Debug.Log("Entered aggro range, but target is out of range");
            }
        }
    }

    private void ConsiderAttack()
    {
        if (distanceToTarget <= enemyAttackRange + 1)
        {
            if (enemyAttackCooldown <= 0)
            {
                enemyAttackCooldown = enemyTimeBetweenAttacks;
                if (aggroDamagable != null)
                {
                    aggroDamagable.TakeDamage(enemyAttack);
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
        }
    }

    private void CheckForPlayerTargets()
    {
        rangeColliders = Physics.OverlapSphere(transform.position, enemyAggroRange, EntityHandler.instance.playerInteractableLayer);

        foreach (Collider col in rangeColliders)
        {
            hasAggro = true;
            aggroTarget = col.transform;
            if (col.gameObject.tag.Equals("Unit"))
            { 
                aggroDamagable = aggroTarget.gameObject.GetComponent<PlayerUnit>();
            }
            else if (col.gameObject.tag.Equals("Trainer"))
            {
                aggroDamagable = aggroTarget.gameObject.GetComponent<PlayerTrainer>();
            }
            navAgent.stoppingDistance = enemyAttackRange + 1;

            break;
        }

        if (!hasAggro)
        {
            navAgent.SetDestination(EnemySpawnManager.instance.enemyGoal.position);
            aggroTarget = null;
            navAgent.stoppingDistance = 0;
        }
    }

    private void CheckForClosestTarget()
    {
        
    }
    
    public void TakeDamage(float damage)
    {
        float totalDamage = damage - enemyArmor;
        enemyCurrentHealth -= Math.Max(totalDamage, 1);
    }
    
    private void HandleHealth()
    {
        enemyCurrentHealth -= Time.deltaTime;
        
        if (enemyCurrentHealth <= 0)
        {
            UnitDeath();
        }
    }

    private void UnitDeath()
    {
        disable?.Invoke(this);
        //Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Helper.IsInLayerMask(other.gameObject.layer, EntityHandler.instance.playerInteractableLayer))
        {
            if (!hasAggro)
            {
                Debug.Log("No target now, giving target");
                hasAggro = true;
                aggroTarget = other.transform;
                if (other.gameObject.tag.Equals("Unit"))
                { 
                    aggroDamagable = aggroTarget.gameObject.GetComponent<PlayerUnit>();
                }
                else if (other.gameObject.tag.Equals("Trainer"))
                {
                    aggroDamagable = aggroTarget.gameObject.GetComponent<PlayerTrainer>();
                }
                navAgent.stoppingDistance = enemyAttackRange + 1;
            }
        }
    }
}
