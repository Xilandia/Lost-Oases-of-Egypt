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

        if (hasAggro)
        {
            MoveToAggroTarget(); 
            ConsiderAttack();
        }
    }
    
    public void MoveUnit(Vector3 _destination)
    {
        navAgent.SetDestination(_destination);
        isMoving = true;
        moveTarget = _destination;
    }
    
    private void CheckForEnemyTargets()
    {
        rangeColliders = Physics.OverlapSphere(transform.position, unitAggroRange, EntityHandler.instance.enemyUnitLayer);

        foreach (Collider col in rangeColliders)
        {
            hasAggro = true;
            aggroTarget = col.transform;
            aggroDamagable = aggroTarget.gameObject.GetComponent<EnemyUnit>();
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
            
            if (distanceToTarget <= unitAggroRange + 2) // range offset or in second stage of behavior
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
        if (distanceToTarget <= unitAttackRange + 1)
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
        
        PlayerManager.instance.meleeSoldiers.Remove(this);
        PlayerManager.instance.rangedSoldiers.Remove(this);
        PlayerManager.instance.workers.Remove(this);
        
        Destroy(gameObject);
    } 
    
    private void OnTriggerEnter(Collider other)
    {
        if (Helper.IsInLayerMask(other.gameObject.layer, EntityHandler.instance.enemyUnitLayer))
        {
            //Debug.Log("Player Collider entered");
            if (!hasAggro)
            {
                Debug.Log("No target now, giving target");
                hasAggro = true;
                aggroTarget = other.transform;
                aggroDamagable = aggroTarget.gameObject.GetComponent<EnemyUnit>();
                navAgent.stoppingDistance = unitAttackRange;
            }
        }
    }
}
