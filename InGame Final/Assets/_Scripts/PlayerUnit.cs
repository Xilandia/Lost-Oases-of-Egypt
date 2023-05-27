using System;
using _Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerUnit : MonoBehaviour, Damagable
{
    private NavMeshAgent navAgent;
    private Camera cam;

    public string unitName;
    public float unitCost, unitHealth, unitArmor, unitAttack, unitTimeBetweenAttacks, unitAttackRange, unitAggroRange, unitMoveSpeed, unitTrainTime;
    public float unitCurrentHealth;
    public float unitAttackCooldown;
    
    public GameObject unitStatDisplay;
    public Image unitHealthBarImage;
    public GameObject unitPrefab;
    public Transform unitTransform;
    public IUnit interactable;

    private Collider[] rangeColliders;
    public Vector3 moveTarget;
    public Transform aggroTarget;
    public Damagable aggroDamagable;
    public bool hasAggro = false;
    public bool isMoving = false;
    private float distanceToTarget;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }

    void Update()
    {
        HandleHealth();

        /*if (!isMoving)
        {
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
        else
        {
            isMoving = CheckMovementStatus();
        }*/
    }
    
    private void CheckForEnemyTargets()
    {
        rangeColliders = Physics.OverlapSphere(transform.position, unitAggroRange, 1 << EntityHandler.instance.enemyUnitLayer);
        
        foreach (Collider col in rangeColliders)
        {
            hasAggro = true;
            aggroTarget = col.transform;
            if (col.gameObject.tag.Equals("Enemy Unit"))
            { 
                aggroDamagable = aggroTarget.gameObject.GetComponent<EnemyUnit>();
            }

            break;
        }
    }
    
    public void MoveUnit(Vector3 _destination)
    {
        navAgent.SetDestination(_destination);
        isMoving = true;
        moveTarget = _destination;
    }

    private bool CheckMovementStatus()
    {
        return Math.Abs(Vector3.Distance(moveTarget, transform.position)) < 0.5f;
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
            navAgent.stoppingDistance = unitAttackRange; // consider moving to where it will only run once

            if (distanceToTarget <= unitAggroRange)
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
        if (distanceToTarget <= unitAttackRange)
        {
            if (unitAttackCooldown <= 0)
            {
                unitAttackCooldown = unitTimeBetweenAttacks;
                if (aggroDamagable != null)
                {
                    Debug.Log("Player Unit is Attacking!", this);
                    aggroDamagable.TakeDamage(unitAttack);
                }
                else 
                {
                    Debug.Log("Has target, but no damagable component");
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
        }
    }

    public void TakeDamage(float damage)
    {
        float totalDamage = damage - unitArmor;
        unitCurrentHealth -= Math.Max(totalDamage, 1);
    }
    
    private void HandleHealth()
    {
        unitStatDisplay.transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
        
        unitHealthBarImage.fillAmount = unitCurrentHealth / unitHealth;
        
        if (unitCurrentHealth <= 0)
        {
            UnitDeath();
        }
    }

    private void UnitDeath()
    {
        InputHandler.instance.selectedUnits.Remove(this);
        Destroy(gameObject);
    } 
    
    private void OnTriggerEnter(Collider other)
    {
        if (Helper.IsInLayerMask(other.gameObject.layer, EntityHandler.instance.enemyUnitLayer))
        {
            //Debug.Log("Player Collider entered");
        }
    }
}
