using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using _Scripts.Enemy.Unit;
using _Scripts.Player.Management;
using _Scripts.Utility.Scriptable;

namespace _Scripts.Enemy.Management
{
    public class EnemySpawnManager : MonoBehaviour
    {
        public static EnemySpawnManager instance;

        public List<Transform> enemySpawnPoints;
        
        [SerializeField] private List<EnemyStage> enemyStages;
        private EnemyStage currentStage;
        private int currentWaveIndex;
        private int currentStageIndex;
        private int numberOfEnemiesToSpawn;
        private float previousPeriodTick;
        private bool stageIsActive;
        [SerializeField] private int periodNumber;
        public int currentSpawnPointIndex;

        public Transform enemyBehaviorTransitionTransform;
        
        private readonly List<ObjectPool<EnemyUnit>> enemyPools = new List<ObjectPool<EnemyUnit>>();

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            enemyPools.Add(EnemyPoolHandler.instance.basicEnemyPool);
            enemyPools.Add(EnemyPoolHandler.instance.climberEnemyPool);
            enemyPools.Add(EnemyPoolHandler.instance.fastEnemyPool);
            enemyPools.Add(EnemyPoolHandler.instance.tankEnemyPool);
            enemyPools.Add(EnemyPoolHandler.instance.bossEnemyPool);
            
            currentStage = enemyStages[currentStageIndex];
            stageIsActive = true;
            currentWaveIndex = 0;
        }


        void Update()
        {
            if (stageIsActive)
            {
                if (PlayerManager.instance.roundTimer[2] - previousPeriodTick >= currentStage.waveLength)
                {
                    periodNumber++;
                    SpawnEnemies();
                    previousPeriodTick = PlayerManager.instance.roundTimer[2];
                }
            }
        }

        private void SpawnEnemies()
        {
            if (currentWaveIndex < currentStage.waves.Count)
            {
                EnemyWave currentWave = currentStage.waves[currentWaveIndex];

                for (int i = 0; i < currentWave.waveEnemyIndex.Count; i++)
                {
                    currentSpawnPointIndex = currentWave.waveEnemySides[i];
                    enemyPools[(int) currentWave.waveEnemyIndex[i]].Get();
                }
                
                currentWaveIndex++;
            }
            else
            {
                stageIsActive = false;
                /*currentWaveIndex = 0;
                currentStageIndex++;
                currentStage = enemyStages[currentStageIndex];
                Debug.Log("Done with stage");*/
            }
        }
    }
}
