using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.GameFlow.Menu
{
    public class PauseMenuHandler : MonoBehaviour
    {
        public static PauseMenuHandler instance;
        
        public GameObject pauseMenuUI;
        public bool isGamePaused = false;
        
        void Awake()
        {
            instance = this;
        }
        
        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            isGamePaused = false;
        }

        public void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            isGamePaused = true;
        }

        public void Controls()
        {
            pauseMenuUI.SetActive(false);
            ControlViewHandler.instance.Pause();
        }
        
        public void LoadMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Menu");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
