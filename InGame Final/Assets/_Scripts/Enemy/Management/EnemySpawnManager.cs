using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using _Scripts.Enemy.Unit;
using _Scripts.Player.Management;
using _Scripts.Utility.Entity;

namespace _Scripts.Enemy.Management
{
    public class EnemySpawnManager : MonoBehaviour
    {
        public static EnemySpawnManager instance;

        public List<Transform> enemySpawnPoints;

        private int numberOfEnemiesToSpawn;
        private float previousPeriodTick;
        private int periodNumber;
        public float periodLength;
        public float progressionMultiplier = 1;

        public Transform enemyBehaviorTransitionTransform;
        
        private readonly List<ObjectPool<EnemyUnit>> enemyPools = new List<ObjectPool<EnemyUnit>>();

        void Awake()
        {
            instance = this;
            
            enemyPools.Add(EnemyPoolHandler.instance.basicEnemyPool);
            enemyPools.Add(EnemyPoolHandler.instance.climberEnemyPool);
            enemyPools.Add(EnemyPoolHandler.instance.fastEnemyPool);
            enemyPools.Add(EnemyPoolHandler.instance.slowEnemyPool);
            enemyPools.Add(EnemyPoolHandler.instance.tankEnemyPool);
            enemyPools.Add(EnemyPoolHandler.instance.juggernautEnemyPool);
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

        private void SpawnEnemies()
        {
            Debug.Log(numberOfEnemiesToSpawn);

            for (int i = 0; i < numberOfEnemiesToSpawn; i++)
            {
                for (int j = 0; j < enemyPools.Count; j++)
                {
                    enemyPools[j].Get();
                }
            }
        }

        private void CalculateWaveSize()
        {
            numberOfEnemiesToSpawn =
                (int)Math.Ceiling(periodNumber * 2 * Math.Pow(1.1, periodNumber) * progressionMultiplier);
        }
    }
}
