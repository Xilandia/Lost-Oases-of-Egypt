using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyUnit : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private Camera cam;
    
    private Collider[] rangeColliders;
    private Transform aggroTarget;
    private PlayerUnit aggroPlayerUnit;
    private bool hasAggro = false;
    private float distanceToTarget;

    public string enemyName;
    public float enemyHealth, enemyArmor, enemyAttack, enemyTimeBetweenAttacks, enemyAttackRange, enemyMoveSpeed, enemyAggroRange;
    public float enemyCurrentHealth;
    public float enemyAttackCooldown;
    
    public GameObject enemyStatDisplay;
    public Image enemyHealthBarImage;
    
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
        
        enemyCurrentHealth = enemyHealth; // temporary until I properly handle enemy stats
        enemyAttackCooldown = enemyTimeBetweenAttacks;
    }

    void Update()
    {
        HandleHealth();
        
        if (!hasAggro)
        {
            CheckForEnemyTargets();
        }
        else
        {
            MoveToAggroTarget();
            ConsiderAttack();
        }
    }
    
    private void MoveToAggroTarget()
    {
        if (aggroTarget == null)
        {
            navAgent.SetDestination(transform.position);
            hasAggro = false;
        }
        else
        {
            distanceToTarget = Vector3.Distance(transform.position, aggroTarget.position);
            navAgent.stoppingDistance = enemyAttackRange + 1; // consider moving to where it will only run once

            if (distanceToTarget <= enemyAggroRange)
            {
                navAgent.SetDestination(aggroTarget.position);
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
                if (aggroPlayerUnit)
                {
                    aggroPlayerUnit.TakeDamage(enemyAttack);
                }
                else 
                {
                    //Debug.Log("Has target, but no player unit component");
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

    private void CheckForEnemyTargets()
    {
        rangeColliders = Physics.OverlapSphere(transform.position, enemyAggroRange);

        foreach (Collider col in rangeColliders)
        {
            if (col.gameObject.layer == EntityHandler.instance.playerInteractableLayer)
            {
                hasAggro = true;
                aggroTarget = col.transform;
                aggroPlayerUnit = aggroTarget.gameObject.GetComponent<PlayerUnit>();
                break;
            }
            else if (col.gameObject.layer == EntityHandler.instance.playerInteractableLayer)
            {
                // handle trainer aggro
            }
        }
    }
    
    public void TakeDamage(float damage)
    {
        float totalDamage = damage - enemyArmor;
        enemyCurrentHealth -= totalDamage;
    }
    
    private void HandleHealth()
    {
        enemyStatDisplay.transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
        
        enemyHealthBarImage.fillAmount = enemyCurrentHealth / enemyHealth;
        
        if (enemyCurrentHealth <= 0)
        {
            UnitDeath();
        }
    }

    private void UnitDeath()
    {
        Destroy(gameObject);
    }
}
