using UnityEngine;
using _Scripts.Enemy.Unit;
using _Scripts.Player.Structure;
using _Scripts.Player.Unit;

namespace _Scripts.Utility.Entity
{
    public class EntityHandler : MonoBehaviour
    {
        public static EntityHandler instance;
        public LayerMask playerInteractableLayer, enemyUnitLayer, resourceLayer;

        [SerializeField] private EntityEnemy[] enemies;
        [SerializeField] private EntityUnit[] units;
        [SerializeField] private EntityWorker[] workers;
        [SerializeField] private EntityBarracks[] barracks;
        [SerializeField] private EntityTower[] towers;

        void Awake()
        {
            instance = this;

            playerInteractableLayer = LayerMask.GetMask("Interactables");
            enemyUnitLayer = LayerMask.GetMask("Enemies");
            resourceLayer = LayerMask.GetMask("Resource Nodes");
        }

        public void SetPlayerUnitStats(PlayerUnit pU, string unitType)
        {
            EntityUnit entity = null;
            
            if (unitType.Contains("Clone"))
            {
                unitType = unitType.Substring(0, unitType.Length - 7);
            }

            foreach (EntityUnit unit in units)
            {
                if (unit.entityName == unitType)
                {
                    entity = unit;
                    break;
                }
            }
            
            pU.unitName = entity.entityName;
            pU.unitCostOre = entity.entityCostOre;
            pU.unitCostWood = entity.entityCostWood;
            pU.unitHealth = entity.entityHealth;
            pU.unitCurrentHealth = entity.entityHealth;
            pU.unitArmor = entity.entityArmor;
            pU.unitAttack = entity.entityAttack;
            pU.unitTimeBetweenAttacks = entity.entityTimeBetweenAttacks;
            pU.unitAttackRange = entity.entityAttackRange;
            pU.unitAggroRange = entity.entityAggroRange;
            pU.unitMoveSpeed = entity.entityMoveSpeed;
            pU.unitTrainTime = entity.entityCreationTime;
        }

        public void SetPlayerWorkerStats(PlayerWorker pW, string workerType)
        {
            EntityWorker entity = null;
            
            if (workerType.Contains("Clone"))
            {
                workerType = workerType.Substring(0, workerType.Length - 7);
            }

            foreach (EntityWorker worker in workers)
            {
                if (worker.entityName == workerType)
                {
                    entity = worker;
                    break;
                }
            }
            
            pW.workerName = entity.entityName;
            pW.workerHealth = entity.entityHealth;
            pW.workerCurrentHealth = entity.entityHealth;
            pW.workerArmor = entity.entityArmor;
            pW.workerOperationRange = entity.entityOperationRange;
            pW.workerMoveSpeed = entity.entityMoveSpeed;
            pW.workerGatherSpeed = entity.entityTimeBetweenGathering;
            pW.buildableBarracks = entity.buildableBarracks;
            pW.buildableTowers = entity.buildableTowers;
            pW.buildableStructureNames = entity.buildableStructureNames;
        }
        
        public void SetPlayerBarracksStats(PlayerBarracks pB, string barracksType)
        {
            EntityBarracks entity = null;
            
            if (barracksType.Contains("Clone"))
            {
                barracksType = barracksType.Substring(0, barracksType.Length - 7);
            }

            foreach (EntityBarracks barrack in barracks)
            {
                if (barrack.entityName == barracksType)
                {
                    entity = barrack;
                    break;
                }
            }

            pB.barracksName = entity.entityName;
            pB.barracksCostOre = entity.entityCostOre;
            pB.barracksCostWood = entity.entityCostWood;
            pB.barracksHealth = entity.entityHealth;
            pB.barracksCurrentHealth = entity.entityHealth;
            pB.barracksArmor = entity.entityArmor;
            pB.barracksBuildTime = entity.entityCreationTime;
            pB.buildableUnits = entity.buildableUnits;
            pB.buildableUnitNames = entity.buildableUnitNames;
        }
        
        public void SetPlayerTowerStats(PlayerTower pT, string towerType)
        {
            EntityTower entity = null;
            
            if (towerType.Contains("Clone"))
            {
                towerType = towerType.Substring(0, towerType.Length - 7);
            }

            foreach (EntityTower tower in towers)
            {
                if (tower.entityName == towerType)
                {
                    entity = tower;
                    break;
                }
            }

            pT.towerName = entity.entityName;
            pT.towerCostOre = entity.entityCostOre;
            pT.towerCostWood = entity.entityCostWood;
            pT.towerHealth = entity.entityHealth;
            pT.towerCurrentHealth = entity.entityHealth;
            pT.towerArmor = entity.entityArmor;
            pT.towerAttack = entity.entityAttack;
            pT.towerTimeBetweenAttacks = entity.entityTimeBetweenAttacks;
            pT.towerAttackRange = entity.entityAttackRange;
            pT.towerBuildTime = entity.entityCreationTime;
        }

        public void SetEnemyStats(EnemyUnit eU, string enemyType)
        {
            EntityEnemy entity = null;
            
            if (enemyType.Contains("Clone"))
            {
                enemyType = enemyType.Substring(0, enemyType.Length - 7);
            }

            foreach (EntityEnemy enemy in enemies)
            {
                if (enemy.entityName == enemyType)
                {
                    entity = enemy;
                    break;
                }
            }
            
            eU.enemyName = entity.entityName;
            eU.enemyHealth = entity.entityHealth;
            eU.enemyCurrentHealth = entity.entityHealth;
            eU.enemyArmor = entity.entityArmor;
            eU.enemyAttack = entity.entityAttack;
            eU.enemyTimeBetweenAttacks = entity.entityTimeBetweenAttacks;
            eU.enemyAttackCooldown = entity.entityTimeBetweenAttacks;
            eU.enemyAttackRange = entity.entityAttackRange;
            eU.enemyAggroRange = entity.entityAggroRange;
            eU.enemyMoveSpeed = entity.entityMoveSpeed;
        }
    }
}