using System;
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
            CheckForPlayerTargets();
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
        rangeColliders = Physics.OverlapSphere(transform.position, enemyAggroRange, 1 << EntityHandler.instance.playerInteractableLayer);
        
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

            break;
        }
    }
    
    public void TakeDamage(float damage)
    {
        float totalDamage = damage - enemyArmor;
        enemyCurrentHealth -= Math.Max(totalDamage, 1);
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
