using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.GameFlow.Menu
{
    public class ControlViewHandler : MonoBehaviour
    {
        public static ControlViewHandler instance;
        
        public GameObject controlViewUI;
        public bool isControlViewActive;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            Pause();
        }
        
        public void Resume()
        {
            controlViewUI.SetActive(false);
            Time.timeScale = 1f;
            isControlViewActive = false;
            PauseMenuHandler.instance.isGamePaused = false;
        }

        public void Pause()
        {
            controlViewUI.SetActive(true);
            Time.timeScale = 0f;
            isControlViewActive = true;
            PauseMenuHandler.instance.isGamePaused = true;
        }
    }
}
