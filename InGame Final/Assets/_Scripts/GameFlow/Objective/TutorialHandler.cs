using System;
using System.Collections;
using _Scripts.Enemy.Unit;
using _Scripts.GameFlow.Sound;
using _Scripts.Player.Management;
using _Scripts.Player.Structure;
using _Scripts.Player.Unit;
using _Scripts.Utility.Entity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.GameFlow.Objective
{
    public class TutorialHandler : MonoBehaviour
    {
        public PlayerUnit hero;
        public PlayerWorker[] workers;
        public EnemyUnit[] enemies;
        public EnemyUnit firstEnemy;
        public Transform firstEnemyTransform;
        public GameObject firstBoundary;
        public GameObject secondBoundary;
        public GameObject firstBoundaryArrow;
        public GameObject congratulatoryText;
        public Image blackoutImage;
        public AudioClip boundaryDing;

        public int tutorialPart;

        void Start()
        {
            InitializeUnits();
            InitializeEnemies();
            
            PlayerManager.instance.PlayerOre = 50;
            PlayerManager.instance.PlayerWood = 50;
        }

        private void InitializeUnits()
        {
            EntityHandler.instance.SetPlayerUnitStats(hero, hero.unitName);
            foreach (PlayerWorker worker in workers)
            {
                EntityHandler.instance.SetPlayerWorkerStats(worker, worker.workerName);
                worker.buildableBarracks = Array.Empty<PlayerBarracks>();
                worker.buildableTowers = new [] { worker.buildableTowers[0] };
                worker.buildableStructureInfo = new [] { worker.buildableStructureInfo[1] };
                worker.workerOperationRange *= 2;
            }
        }

        private void InitializeEnemies()
        {
            foreach (EnemyUnit eU in enemies)
            {
                EntityHandler.instance.SetEnemyStats(eU, eU.enemyName);
                eU.gameObject.SetActive(false);
            }

            firstEnemy.enemyCurrentHealth *= 2;
            firstEnemy.behaviorOne = false;
            firstEnemy.gameObject.SetActive(true);
        }

        void Update()
        {
            switch (tutorialPart)
            {
                case 0:
                    if (firstEnemyTransform == null)
                    {
                        TransitionPart();
                    }
                    break;
                case 1:
                    if (PlayerManager.instance.towers.Count > 0)
                    {
                        TransitionPart();
                    }
                    break;
                case 2:
                    if (PlayerManager.instance.enemyUnits.childCount == 0)
                    {
                        TransitionPart();
                    }
                    break;
                case 3:
                    break;
            }
        }
        
        private void TransitionPart()
        {
            tutorialPart++;
            
            switch (tutorialPart)
            {
                case 1:
                    firstBoundary.SetActive(false);
                    firstBoundaryArrow.SetActive(true);
                    SoundHandler.instance.PlaySoundEffect(boundaryDing);
                    PlayerManager.instance.canGatherResources = true;
                    
                    foreach (PlayerWorker worker in workers)
                    {
                        PlayerManager.instance.workers.Add(worker);
                    }
                    
                    break;
                case 2:
                    secondBoundary.SetActive(false);
                    SoundHandler.instance.PlaySoundEffect(boundaryDing);
                    for (int i = 0; i < enemies.Length - 1; i++) 
                    {
                        enemies[i].gameObject.SetActive(true);
                        enemies[i].TransitionPhase();
                    }
                    break;
                case 3:
                    congratulatoryText.SetActive(true);
                    StartCoroutine(Fadeout());
                    break;
            }
        }

        private IEnumerator Fadeout()
        {
            Color objectColor = blackoutImage.color;
            
            while (objectColor.a < 1f)
            {
                float fadeAmount = objectColor.a + Time.deltaTime / 5;
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackoutImage.color = objectColor;
                yield return null;
            }

            Destroy(ScoreHandler.instance.gameObject);
            SceneManager.LoadScene("Game Map");
        }
    }
}
