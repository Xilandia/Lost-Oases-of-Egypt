using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;
    
    [SerializeField] private List<Transform> spawnPoints;
    
    public List<Entity> enemyUnits = new List<Entity>();

    private int numberOfEnemiesToSpawn;
    private float previousPeriodTick;
    private int periodNumber;
    public float periodLength;
    public float progressionMultiplier = 1;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.instance.roundTimer[2] - previousPeriodTick >= periodLength)
        {
            // spawn enemies
            periodNumber++;
            CalculateWaveSize();
            SpawnEnemies();
            previousPeriodTick = PlayerManager.instance.roundTimer[2];
        }
    }

    private void SpawnEnemies()
    {
        Debug.Log(numberOfEnemiesToSpawn);
        
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            int spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            int unitIndex = UnityEngine.Random.Range(0, enemyUnits.Count);
            Vector3 spawnPos = spawnPoints[spawnPointIndex].position;
            float xScale = spawnPoints[spawnPointIndex].localScale.x;
            float zScale = spawnPoints[spawnPointIndex].localScale.z;
            
            spawnPos += new Vector3(UnityEngine.Random.Range(-xScale, xScale), 0, UnityEngine.Random.Range(-zScale, zScale));
            GameObject enemy = Instantiate(enemyUnits[unitIndex].entityPrefab, spawnPos, Quaternion.identity);
            enemy.transform.SetParent(PlayerManager.instance.enemyUnits);
            EntityHandler.instance.SetEnemyStats(enemy.gameObject.GetComponent<EnemyUnit>(), enemyUnits[unitIndex].entityName);
        }
    }

    private void CalculateWaveSize()
    {
        numberOfEnemiesToSpawn = (int) Math.Ceiling(periodNumber * 2 * Math.Pow(1.1, periodNumber) * progressionMultiplier);
    }
}
