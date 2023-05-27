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
    private float distanceToClosestTarget;
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
        
        if (hasAggro)
        {
            MoveToAggroTarget(); 
            ConsiderAttack();
        }
    }
    
    private void MoveToAggroTarget()
    {
        if (aggroTarget == null)
        {
            hasAggro = false;
            if (stageOne)
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
            distanceToTarget = Vector3.Distance(transform.position, aggroTarget.position);
            
            if (distanceToTarget <= enemyAggroRange + 2 || !stageOne) // range offset or in second stage of behavior
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
            navAgent.stoppingDistance = enemyAttackRange;

            break;
        }

        if (!hasAggro)
        {
            navAgent.SetDestination(EnemySpawnManager.instance.enemyGoal.position);
            aggroTarget = null;
            navAgent.stoppingDistance = 0;
        }
    }

    public void TransitionPhase()
    {
        stageOne = false;
        navAgent.stoppingDistance = enemyAttackRange;
        CheckForClosestTarget();
    }
    
    private void CheckForClosestTarget()
    {
        hasAggro = false;
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
                aggroTarget = pU.transform;
                aggroDamagable = pU;
                hasAggro = true;
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
                aggroTarget = pU.transform;
                aggroDamagable = pU;
                hasAggro = true;
                inRange = true;
            }
        }
        
        return inRange;
    }
    
    private bool TowerInRange()
    {
        bool inRange = false;
        
        foreach (PlayerTrainer pT in PlayerManager.instance.towers)
        {
            if (distanceToClosestTarget > Vector3.Distance(transform.position, pT.transform.position))
            {
                distanceToClosestTarget = Vector3.Distance(transform.position, pT.transform.position);
                aggroTarget = pT.transform;
                aggroDamagable = pT;
                hasAggro = true;
                inRange = true;
            }
        }
        
        return inRange;
    }
    
    private bool BarracksInRange()
    {
        bool inRange = false;
        
        foreach (PlayerTrainer pT in PlayerManager.instance.barracks)
        {
            if (distanceToClosestTarget > Vector3.Distance(transform.position, pT.transform.position))
            {
                distanceToClosestTarget = Vector3.Distance(transform.position, pT.transform.position);
                aggroTarget = pT.transform;
                aggroDamagable = pT;
                hasAggro = true;
                inRange = true;
            }
        }
        
        return inRange;
    }
    
    private bool WorkerInRange()
    {
        bool inRange = false;
        
        foreach (PlayerUnit pU in PlayerManager.instance.workers)
        {
            if (distanceToClosestTarget > Vector3.Distance(transform.position, pU.transform.position))
            {
                distanceToClosestTarget = Vector3.Distance(transform.position, pU.transform.position);
                aggroTarget = pU.transform;
                aggroDamagable = pU;
                hasAggro = true;
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
