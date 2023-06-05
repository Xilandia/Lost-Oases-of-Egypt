using UnityEngine;
using _Scripts.Enemy.Unit;
using _Scripts.Player.Structure;
using _Scripts.Player.Unit;

namespace _Scripts.Utility.Entity
{
    public class EntityHandler : MonoBehaviour
    {
        public static EntityHandler instance;

        [SerializeField] private Entity axeman, archer, wizard, worker; // units

        [SerializeField] private Entity barracks; // trainers (buildings)

        [SerializeField] private Entity smallTower, bigTower; // preparation for turrets / obelisks

        [SerializeField] private Entity basicEnemy, climberEnemy, fastEnemy, slowEnemy, tankEnemy, juggernautEnemy; // enemy units (to expand)

        public LayerMask playerInteractableLayer, enemyUnitLayer, resourceLayer;

        void Awake()
        {
            instance = this;

            playerInteractableLayer = LayerMask.GetMask("Interactables");
            enemyUnitLayer = LayerMask.GetMask("Enemies");
            resourceLayer = LayerMask.GetMask("Resource Nodes");
        }

        private Entity GetEntityStats(string _entityName)
        {

            if (_entityName.Contains("Clone"))
            {
                _entityName = _entityName.Substring(0, _entityName.Length - 7);
            }

            switch (_entityName)
            {
                case "Axeman":
                    return axeman;
                case "Archer":
                    return archer;
                case "Wizard":
                    return wizard;
                case "Worker":
                    return worker;
                case "Barracks":
                    return barracks;
                case "Small Tower":
                    return smallTower;
                case "Big Tower":
                    return bigTower;
                case "Basic":
                    return basicEnemy;
                case "Climber":
                    return climberEnemy;
                case "Fast":
                    return fastEnemy;
                case "Slow":
                    return slowEnemy;
                case "Tank":
                    return tankEnemy;
                case "Juggernaut":
                    return juggernautEnemy;
                default:
                    Debug.LogError($"Entity {_entityName} not found!");
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
            pU.unitAggroRange = entityStats.entityAggroRange;
            pU.unitMoveSpeed = entityStats.entityMoveSpeed;
            pU.unitTrainTime = entityStats.entityCreationTime;
        }

        public void SetPlayerBarracksStats(PlayerBarracks pB, string trainerType)
        {
            Entity entityStats = GetEntityStats(trainerType);

            pB.barracksName = entityStats.entityName;
            pB.barracksCost = entityStats.entityCost;
            pB.barracksHealth = entityStats.entityHealth;
            pB.barracksCurrentHealth = entityStats.entityHealth;
            pB.barracksArmor = entityStats.entityArmor;
            pB.barracksBuildTime = entityStats.entityCreationTime;

            pB.trainableUnits = entityStats.buildableUnits;
            pB.isPrototype = entityStats.isPrototype;
            pB.isPlaced = entityStats.isPlaced;
            pB.isComplete = entityStats.isComplete;
        }
        
        public void SetPlayerTowerStats(PlayerTower pT, string trainerType)
        {
            Entity entityStats = GetEntityStats(trainerType);

            pT.towerName = entityStats.entityName;
            pT.towerCost = entityStats.entityCost;
            pT.towerHealth = entityStats.entityHealth;
            pT.towerCurrentHealth = entityStats.entityHealth;
            pT.towerArmor = entityStats.entityArmor;
            pT.towerAttack = entityStats.entityAttack;
            pT.towerTimeBetweenAttacks = entityStats.entityTimeBetweenAttacks;
            pT.towerAttackRange = entityStats.entityAttackRange;
            pT.towerBuildTime = entityStats.entityCreationTime;

            pT.isPrototype = entityStats.isPrototype;
            pT.isPlaced = entityStats.isPlaced;
            pT.isComplete = entityStats.isComplete;
        }

        public void SetEnemyStats(EnemyUnit eU, string enemyType)
        {
            Entity entityStats = GetEntityStats(enemyType);

            eU.enemyName = entityStats.entityName;
            eU.enemyHealth = entityStats.entityHealth;
            eU.enemyCurrentHealth = entityStats.entityHealth;
            eU.enemyArmor = entityStats.entityArmor;
            eU.enemyAttack = entityStats.entityAttack;
            eU.enemyTimeBetweenAttacks = entityStats.entityTimeBetweenAttacks;
            eU.enemyAttackCooldown = entityStats.entityTimeBetweenAttacks;
            eU.enemyAttackRange = entityStats.entityAttackRange;
            eU.enemyAggroRange = entityStats.entityAggroRange;
            eU.enemyMoveSpeed = entityStats.entityMoveSpeed;

        }
    }
}