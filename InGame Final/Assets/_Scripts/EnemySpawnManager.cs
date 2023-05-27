using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;
    
    public List<Transform> spawnPoints;
    
    public List<EnemyUnit> enemyUnits = new List<EnemyUnit>();
    
    private int numberOfEnemiesToSpawn;
    private float previousPeriodTick;
    private int periodNumber;
    public float periodLength;
    public float progressionMultiplier = 1;

    private ObjectPool<EnemyUnit> enemyPool;

    public Transform enemyGoal;

    [SerializeField] 
    private EnemyUnit enemyOnePrefab;
    
    void Awake()
    {
        instance = this;
        enemyPool = new ObjectPool<EnemyUnit>(CreatePooledObject, OnTakeFromPool, OnReturnToPool, OnDestroyObject, false, 40, 1000);
    }

    private EnemyUnit CreatePooledObject()
    {
        EnemyUnit Instance = Instantiate(enemyOnePrefab, Vector3.zero, Quaternion.identity);
        Instance.disable += ReturnObjectToPool;
        Instance.gameObject.SetActive(false);
        
        return Instance;
    }

    private void ReturnObjectToPool(EnemyUnit Instance)
    {
        enemyPool.Release(Instance);
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
    
    
    void Update()
    {
        if (PlayerManager.instance.roundTimer[2] - previousPeriodTick >= periodLength)
        {
            periodNumber++;
            CalculateWaveSize();
            SpawnEnemies();
            previousPeriodTick = PlayerManager.instance.roundTimer[2];
        }
    }
    
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 30), $"Total Pool Size: {enemyPool.CountAll}");
        GUI.Label(new Rect(10, 30, 200, 30), $"Active Objects: {enemyPool.CountActive}");
    }

    private void SpawnEnemies()
    {
        Debug.Log(numberOfEnemiesToSpawn);
        
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            /* Previous enemy spawning system
             Random enemy type in random position within spawn point.
             
            int spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            int unitIndex = UnityEngine.Random.Range(0, enemyUnits.Count);
            Vector3 spawnPos = spawnPoints[spawnPointIndex].position;
            float xScale = spawnPoints[spawnPointIndex].localScale.x;
            float zScale = spawnPoints[spawnPointIndex].localScale.z;
            
            spawnPos += new Vector3(UnityEngine.Random.Range(-xScale, xScale), 0, UnityEngine.Random.Range(-zScale, zScale));
            EnemyUnit eU = Instantiate(enemyUnits[unitIndex], spawnPos, Quaternion.identity);
            eU.transform.SetParent(PlayerManager.instance.enemyUnits);
            EntityHandler.instance.SetEnemyStats(eU, enemyUnits[unitIndex].enemyPrefab.name);*/
            
            enemyPool.Get();
        }
    }

    private void SpawnEnemy(EnemyUnit Instance)
    {
        int spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
        Instance.transform.position = spawnPoints[spawnPointIndex].position;
        EntityHandler.instance.SetEnemyStats(Instance, Instance.gameObject.name);
    }

    private void CalculateWaveSize()
    {
        numberOfEnemiesToSpawn = (int) Math.Ceiling(periodNumber * 2 * Math.Pow(1.1, periodNumber) * progressionMultiplier);
    }
}
