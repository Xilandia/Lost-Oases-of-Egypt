using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using _Scripts.Enemy.Unit;
using _Scripts.GameFlow.Sound;
using _Scripts.GameFlow.Transition;
using _Scripts.Player.Management;
using _Scripts.Utility.Scriptable;

namespace _Scripts.Enemy.Management
{
    public class EnemySpawnManager : MonoBehaviour
    {
        public static EnemySpawnManager instance;

        public List<Transform> enemySpawnPoints;
        
        public EnemyStage currentStage;
        public int currentWaveIndex;
        public float previousPeriodTick;
        public bool stageIsActive;
        public int currentSpawnPointIndex;

        public Transform currentEnemyGoalTransform;
        public Transform[] enemyGoalTransforms;
        
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
        }


        void Update()
        {
            if (stageIsActive)
            {
                if (PlayerManager.instance.roundTimer[2] - previousPeriodTick >= currentStage.waveLength)
                {
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
                    for (int j = 0; j < PlayerManager.instance.roundTimer[1] / 2 + 1; j++)
                    {
                        enemyPools[(int) currentWave.waveEnemyIndex[i]].Get();
                    }
                }
                
                currentWaveIndex++;
            }
            else
            {
                stageIsActive = false;
                SoundHandler.instance.PlayTransitionTrack();
                StageTransitionHandler.instance.StageWasDeployed();
            }
        }
    }
}
