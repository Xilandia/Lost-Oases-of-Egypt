using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyUnit : MonoBehaviour, Damagable
{
    private NavMeshAgent navAgent;
    private Camera cam;
    
    private Collider[] rangeColliders;
    private Transform aggroTarget;
    private Damagable aggroDamagable;
    private bool hasAggro = false;
    private bool targettingBase = false;
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
    }

    void Update()
    {
        HandleHealth();
        
        if (!hasAggro)
        {
            CheckForEnemyTargets();

            if (targettingBase)
            {
                MoveToAggroTarget();
                ConsiderAttack();
            }
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

            if (distanceToTarget <= enemyAggroRange || targettingBase)
            {
                navAgent.SetDestination(aggroTarget.position);
            }
            else
            {
                aggroTarget = null;
                hasAggro = false;
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
                    Debug.Log("Has target, but no damagable component");
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
        rangeColliders = Physics.OverlapSphere(transform.position, enemyAggroRange, EntityHandler.instance.playerInteractableLayer);
        Debug.Log("Checking for targets, found " + rangeColliders.Length + " targets");
        foreach (Collider col in rangeColliders)
        {
            hasAggro = true;
            targettingBase = false; 
            aggroTarget = col.transform;
            if (col.gameObject.tag.Equals("PlayerUnit"))
            { 
                aggroDamagable = aggroTarget.gameObject.GetComponent<PlayerUnit>();
            }
            else if (col.gameObject.tag.Equals("PlayerTrainer"))
            {
                aggroDamagable = aggroTarget.gameObject.GetComponent<PlayerTrainer>();
            }

            break;
        }
        if (!hasAggro)
        {
            aggroTarget = PlayerManager.instance.baseStructure;
            aggroDamagable = PlayerManager.instance.baseStructureScript;
            targettingBase = true;
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
