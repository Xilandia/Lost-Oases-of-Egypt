using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    public static UnitHandler instance;

    [SerializeField]
    private Entity arbalist, gladiator, guardian, warlock, worker;
    
    void Start()
    {
        instance = this;
    }
    
    public Entity GetEntityStats(string _entityName)
    {
        switch (_entityName)
        {
            case "Arbalist":
                return arbalist;
            case "Gladiator":
                return gladiator;
            case "Guardian":
                return guardian;
            case "Warlock":
                return warlock;
            case "Worker":
                return worker;
            default:
                Debug.Log($"Entity {_entityName} not found!");
                return null;
        }
    }

    public void SetEntityStats(Transform type)
    {
        foreach (Transform child in type)
        {
            foreach (Transform grandchild in child)
            {
                string entityName = child.name.Substring(0, child.name.Length - 1);
                Entity entityStats = GetEntityStats(entityName);
                PlayerUnit pU = grandchild.GetComponent<PlayerUnit>();
                
                pU.unitCost = entityStats.entityCost;
                pU.unitHealth = entityStats.entityHealth;
                pU.unitArmor = entityStats.entityArmor;
                pU.unitAttack = entityStats.entityAttack;
                pU.unitAttackSpeed = entityStats.entityAttackSpeed;
                pU.unitAttackRange = entityStats.entityAttackRange;
                pU.unitMoveSpeed = entityStats.entityMoveSpeed;
            }
        }
    }
    
}
