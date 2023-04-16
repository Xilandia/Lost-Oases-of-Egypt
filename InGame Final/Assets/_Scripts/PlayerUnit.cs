using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerUnit : MonoBehaviour
{
    private NavMeshAgent navAgent;
    
    public int unitCost, unitHealth, unitArmor, unitAttack, unitAttackSpeed, unitAttackRange, unitMoveSpeed;
    
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void MoveUnit(Vector3 _destination)
    {
        navAgent.SetDestination(_destination);
    }
}
