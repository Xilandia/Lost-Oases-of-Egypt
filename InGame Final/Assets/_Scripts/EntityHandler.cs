using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class EntityHandler : MonoBehaviour
{
    public static EntityHandler instance;
    
    [SerializeField]
    private Entity arbalist, gladiator, guardian, warlock, worker; // units

    [SerializeField]
    private Entity workerCenter, basicTrainer, advancedTrainer; // trainers (buildings)
    
    [HideInInspector]
    public LayerMask playerInteractableLayer, enemyUnitLayer;

    void Awake()
    {
        instance = this;
        
        playerInteractableLayer = LayerMask.NameToLayer("Interactables");
        enemyUnitLayer = LayerMask.NameToLayer("Enemy Units");
    }

    public Entity GetEntityStats(string _entityName)
    {
        switch (_entityName.Substring(0, _entityName.Length - 7))
        {
            case "Arbalists":
                return arbalist;
            case "Gladiators":
                return gladiator;
            case "Guardians":
                return guardian;
            case "Warlocks":
                return warlock;
            case "Workers":
                return worker;
            case "Worker Center":
                return workerCenter;
            case "Basic Robot Manufactory":
                return basicTrainer;
            case "Advanced Robot Manufactory":
                return advancedTrainer;
            default:
                Debug.Log($"Entity {_entityName} not found!");
                return null;
        }
    }

    public void SetPlayerUnitStats(PlayerUnit pU, string unitType)
    {
        Entity entityStats = GetEntityStats(unitType);
        
        pU.unitName = entityStats.entityName;
        pU.unitCost = entityStats.entityCost;
        pU.unitHealth = entityStats.entityHealth;
        pU.unitCurrentHealth = entityStats.entityHealth;
        pU.unitArmor = entityStats.entityArmor;
        pU.unitAttack = entityStats.entityAttack;
        pU.unitTimeBetweenAttacks = entityStats.entityTimeBetweenAttacks;
        pU.unitAttackRange = entityStats.entityAttackRange;
        pU.unitMoveSpeed = entityStats.entityMoveSpeed;
        pU.unitTrainTime = entityStats.entityCreationTime;
    }
    
    public void SetPlayerTrainerStats(PlayerTrainer pT, string trainerType)
    {
        Entity entityStats = GetEntityStats(trainerType);

        pT.trainerName = entityStats.entityName;
        pT.trainerCost = entityStats.entityCost;
        pT.trainerHealth = entityStats.entityHealth;
        pT.trainerCurrentHealth = entityStats.entityHealth;
        pT.trainerArmor = entityStats.entityArmor;
        pT.trainerBuildTime = entityStats.entityCreationTime;
                        
        pT.buildableUnits = entityStats.buildableUnits;
        pT.isBuildable = entityStats.isBuildable;
        pT.isPrototype = entityStats.isPrototype;
        pT.isPlaced = entityStats.isPlaced;
        pT.isComplete = entityStats.isComplete;
    }
    
}
