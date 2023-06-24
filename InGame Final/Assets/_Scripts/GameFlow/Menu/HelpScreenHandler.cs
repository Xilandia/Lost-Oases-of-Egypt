using System.Collections;
using System.Collections.Generic;
using _Scripts.GameFlow.Transition;
using UnityEngine;

namespace _Scripts.GameFlow.Menu
{
    public class HelpScreenHandler : MonoBehaviour
    {
        public static HelpScreenHandler instance;

        public GameObject helpScreenUI;
        public bool isHelpScreenActive;
        
        public int currentTipIndex;
        public GameObject[] tips;
        public bool isAfterStageThree;

        void Awake()
        {
            instance = this;
        }
        

        public void Resume()
        {
            helpScreenUI.SetActive(false);
            Time.timeScale = 1f;
            isHelpScreenActive = false;
            PauseMenuHandler.instance.isGamePaused = false;
        }

        public void Pause()
        {
            helpScreenUI.SetActive(true);
            Time.timeScale = 0f;
            isHelpScreenActive = true;
            PauseMenuHandler.instance.isGamePaused = true;
            SetupTip();
        }

        private void SetupTip()
        {
            if (StageTransitionHandler.instance.readyToLoadStage && StageTransitionHandler.instance.currentStage == 3)
            {
                isAfterStageThree = true;
            }
            
            currentTipIndex = StageTransitionHandler.instance.currentStage + (isAfterStageThree? 1 : 0);
            ShowCurrentTip();
        }

        public void NextTip()
        {
            if (currentTipIndex < tips.Length - 1)
            {
                currentTipIndex++;
                ShowCurrentTip();
            }
        }
        
        public void PreviousTip()
        {
            if (currentTipIndex > 0)
            {
                currentTipIndex--;
                ShowCurrentTip();
            }
        }

        private void ShowCurrentTip()
        {
            for (int i = 0; i < tips.Length; i++)
            {
                tips[i].SetActive(i == currentTipIndex);
            }
        }
    }
}
