using System.Collections;
using System.Collections.Generic;
using _Scripts.Enemy.Management;
using _Scripts.GameFlow.Objective;
using _Scripts.GameFlow.Sound;
using _Scripts.Player.Management;
using _Scripts.Utility.Entity;
using _Scripts.Utility.Scriptable;
using UnityEngine;

namespace _Scripts.GameFlow.Transition
{
    public class StageTransitionHandler : MonoBehaviour
    {
        public static StageTransitionHandler instance;

        [SerializeField] private GameObject middleBoundary;
        [SerializeField] private GameObject upperLeftBoundary;
        [SerializeField] private GameObject upperRightBoundary;
        [SerializeField] private SphereCollider boatCollider;
        [SerializeField] private List<EnemyStage> stages;
        [SerializeField] private AudioClip boundaryDing;
        [SerializeField] private GameObject canAdvanceText;
        public int currentStage;
        public bool readyToLoadStage = true;

        void Awake()
        {
            instance = this;
        }

        public void LoadStage()
        {
            if (readyToLoadStage)
            {
                readyToLoadStage = false;
                SoundHandler.instance.PlayStageTrack(currentStage);

                switch (currentStage)
                {
                    case 0:
                    case 1:
                    case 2:
                        CameraPanHandler.instance.PanCameraToPortal(currentStage);
                        break;
                    case 3:
                        StatueHandler.instance.StartStatueStage();
                        EntityHandler.instance.endgame = true;
                        EnemySpawnManager.instance.stageIsActive = true;
                        break;
                    case 4:
                        boatCollider.enabled = true;
                        EnemySpawnManager.instance.stageIsActive = true;
                        break;
                }

                EnemySpawnManager.instance.currentStage = stages[currentStage++];
                EnemySpawnManager.instance.currentWaveIndex = 0;
                EnemySpawnManager.instance.currentEnemyGoalTransform =
                    EnemySpawnManager.instance.enemyGoalTransforms[EnemySpawnManager.instance.currentStage.enemyStageGoal];
            }
        }

        public void StageWasDeployed()
        {
            readyToLoadStage = true;
            
            if (currentStage == 1)
            {
                middleBoundary.SetActive(false);
                SoundHandler.instance.PlaySoundEffect(boundaryDing);
            }
            if (currentStage == 2)
            {
                upperLeftBoundary.SetActive(false);
                upperRightBoundary.SetActive(false);
                SoundHandler.instance.PlaySoundEffect(boundaryDing);
            }

            StartCoroutine(DisplayCanAdvanceText());
        }
        
        private IEnumerator DisplayCanAdvanceText()
        {
            canAdvanceText.SetActive(true);
            yield return new WaitForSeconds(10f);
            canAdvanceText.SetActive(false);
        }
    }
}