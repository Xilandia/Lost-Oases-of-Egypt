using _Scripts.Enemy.Unit;
using _Scripts.Player.Management;
using _Scripts.Utility.Entity;
using UnityEngine;
using UnityEngine.Pool;

namespace _Scripts.Enemy.Management
{
    public class EnemyPoolHandler : MonoBehaviour
    {
        public static EnemyPoolHandler instance;
        
        public ObjectPool<EnemyUnit> basicEnemyPool;
        public ObjectPool<EnemyUnit> climberEnemyPool;
        public ObjectPool<EnemyUnit> fastEnemyPool;
        public ObjectPool<EnemyUnit> tankEnemyPool;
        public ObjectPool<EnemyUnit> bossEnemyPool;

        [SerializeField] private EnemyUnit[] enemyPrefabs;
        
        void Awake()
        {
            instance = this;
            basicEnemyPool = new ObjectPool<EnemyUnit>(CreatePooledObjectBasic, OnTakeFromPool, OnReturnToPool, OnDestroyObject,
                false, 40, 1000);
            climberEnemyPool = new ObjectPool<EnemyUnit>(CreatePooledObjectClimber, OnTakeFromPool, OnReturnToPool, OnDestroyObject,
                false, 20, 100);
            fastEnemyPool = new ObjectPool<EnemyUnit>(CreatePooledObjectFast, OnTakeFromPool, OnReturnToPool, OnDestroyObject,
                false, 15, 30);
            tankEnemyPool = new ObjectPool<EnemyUnit>(CreatePooledObjectTank, OnTakeFromPool, OnReturnToPool, OnDestroyObject,
                false, 10, 30);
            bossEnemyPool = new ObjectPool<EnemyUnit>(CreatePooledObjectBoss, OnTakeFromPool, OnReturnToPool, OnDestroyObject,
                false, 3, 10);
        }
        
        private EnemyUnit CreatePooledObjectBasic()
        {
            EnemyUnit Instance = Instantiate(enemyPrefabs[0], Vector3.zero, Quaternion.identity);
            Instance.disable += ReturnObjectToPool;
            Instance.gameObject.SetActive(false);

            return Instance;
        }
        
        private EnemyUnit CreatePooledObjectClimber()
        {
            EnemyUnit Instance = Instantiate(enemyPrefabs[1], Vector3.zero, Quaternion.identity);
            Instance.disable += ReturnObjectToPool;
            Instance.gameObject.SetActive(false);

            return Instance;
        }
        
        private EnemyUnit CreatePooledObjectFast()
        {
            EnemyUnit Instance = Instantiate(enemyPrefabs[2], Vector3.zero, Quaternion.identity);
            Instance.disable += ReturnObjectToPool;
            Instance.gameObject.SetActive(false);

            return Instance;
        }

        private EnemyUnit CreatePooledObjectTank()
        {
            EnemyUnit Instance = Instantiate(enemyPrefabs[3], Vector3.zero, Quaternion.identity);
            Instance.disable += ReturnObjectToPool;
            Instance.gameObject.SetActive(false);

            return Instance;
        }
        
        private EnemyUnit CreatePooledObjectBoss()
        {
            EnemyUnit Instance = Instantiate(enemyPrefabs[4], Vector3.zero, Quaternion.identity);
            Instance.disable += ReturnObjectToPool;
            Instance.gameObject.SetActive(false);

            return Instance;
        }

        private void ReturnObjectToPool(EnemyUnit Instance)
        {
            basicEnemyPool.Release(Instance);
        }

        private void OnTakeFromPool(EnemyUnit Instance)
        {
            Instance.gameObject.SetActive(true);
            SpawnEnemy(Instance);
            Instance.transform.SetParent(PlayerManager.instance.enemyUnits, true);
        }

        private void OnReturnToPool(EnemyUnit Instance)
        {
            Instance.gameObject.SetActive(false);
        }

        private void OnDestroyObject(EnemyUnit Instance)
        {
            Destroy(Instance.gameObject);
        }
        
        private void SpawnEnemy(EnemyUnit Instance)
        {
            int spawnPointIndex = Random.Range(0, EnemySpawnManager.instance.enemySpawnPoints.Count);
            Instance.transform.position = EnemySpawnManager.instance.enemySpawnPoints[spawnPointIndex].position;
            EntityHandler.instance.SetEnemyStats(Instance, Instance.gameObject.name);
        }
    }
}
