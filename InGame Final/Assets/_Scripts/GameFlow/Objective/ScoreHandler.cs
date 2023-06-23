using System;
using _Scripts.Player.Management;
using _Scripts.Player.Structure;
using _Scripts.Player.Unit;
using UnityEngine;

namespace _Scripts.GameFlow.Objective
{
    public class ScoreHandler : MonoBehaviour
    {
        public static ScoreHandler instance;

        public float totalScore;
        public int[] totalEnemiesKilled = new int[5];
        public float[] totalResourcesCollected = new float[2];
        public int[] totalUnitsSurvived = new int[5];
        public int[] totalBuildingsSurvived = new int[3];

        public int[] gameTime = new int[2];

        void Awake()
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        public void EnemyKilled(string enemyName)
        {
            switch (enemyName)
            {
                case "Basic":
                    totalEnemiesKilled[0] += 1;
                    totalScore += 5;
                    break;
                case "Climber":
                    totalEnemiesKilled[1] += 1;
                    totalScore += 8;
                    break;
                case "Fast":
                    totalEnemiesKilled[2] += 1;
                    totalScore += 8;
                    break;
                case "Tank":
                    totalEnemiesKilled[3] += 1;
                    totalScore += 15;
                    break;
                case "Boss":
                    totalEnemiesKilled[4] += 1;
                    totalScore += 500;
                    break;
            }
        }
        
        public void ResourceCollected(string resourceName, float resourceAmount)
        {
            switch (resourceName)
            {
                case "Wood":
                    totalResourcesCollected[0] += resourceAmount;
                    totalScore += resourceAmount / 2f;
                    break;
                case "Ore":
                    totalResourcesCollected[1] += resourceAmount;
                    totalScore += resourceAmount / 2;
                    break;
            }
        }

        public void StatueBuilt()
        {
            totalScore += 200;
        }

        public void GameEndScoreCalculation(bool isWin)
        {
            foreach (PlayerUnit pU in PlayerManager.instance.meleeSoldiers)
            {
                switch (pU.unitName)
                {
                    case "Axeman":
                        totalUnitsSurvived[0] += 1;
                        totalScore += 40;
                        break;
                    case "Knight":
                        totalUnitsSurvived[1] += 1;
                        totalScore += 20;
                        break;
                }
            }
            foreach (PlayerUnit pU in PlayerManager.instance.rangedSoldiers)
            {
                switch (pU.unitName)
                {
                    case "Archer":
                        totalUnitsSurvived[2] += 1;
                        totalScore += 25;
                        break;
                    case "Wizard":
                        totalUnitsSurvived[3] += 1;
                        totalScore += 60;
                        break;
                }
            }
            foreach (PlayerTower pT in PlayerManager.instance.towers)
            {
                switch (pT.towerName)
                {
                    case "Small Tower":
                        totalBuildingsSurvived[0] += 1;
                        totalScore += 100;
                        break;
                    case "Big Tower":
                        totalBuildingsSurvived[1] += 1;
                        totalScore += 300;
                        break;
                }
            }

            totalBuildingsSurvived[2] = PlayerManager.instance.barracks.Count;
            totalScore += totalBuildingsSurvived[2] * 240;
            totalUnitsSurvived[4] = PlayerManager.instance.workers.Count;
            totalScore += totalUnitsSurvived[4] * 100;
            gameTime[0] = (int) PlayerManager.instance.roundTimer[0];
            gameTime[1] = (int) PlayerManager.instance.roundTimer[1];
            
            if (!isWin)
            {
                totalScore /= 2;
            }
        }
    }
}