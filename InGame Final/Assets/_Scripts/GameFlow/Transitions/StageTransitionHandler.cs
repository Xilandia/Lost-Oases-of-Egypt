using System.Collections.Generic;
using _Scripts.Enemy.Management;
using _Scripts.GameFlow.Sound;
using _Scripts.Player.Management;
using _Scripts.Utility.Scriptable;
using UnityEngine;

namespace _Scripts.GameFlow.Transitions
{
    public class StageTransitionHandler : MonoBehaviour
    {
        public static StageTransitionHandler instance;

        [SerializeField] private GameObject middleBoundary;
        [SerializeField] private GameObject upperLeftBoundary;
        [SerializeField] private GameObject upperRightBoundary;
        [SerializeField] private List<EnemyStage> stages;
        [SerializeField] private int currentStage;
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
                
                if (currentStage < 3)
                {
                    CameraPanHandler.instance.PanCameraToPortal(currentStage);
                }
                
                EnemySpawnManager.instance.currentStage = stages[currentStage++];
                EnemySpawnManager.instance.currentWaveIndex = 0;
            }
        }

        public void StageWasDeployed()
        {
            readyToLoadStage = true;
            
            if (currentStage == 1)
            {
                middleBoundary.SetActive(false);
            }
            if (currentStage == 2)
            {
                upperLeftBoundary.SetActive(false);
                upperRightBoundary.SetActive(false);
            }
            
            Debug.Log("Stage " + currentStage + " was deployed.");
        }
    }
}