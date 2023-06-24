using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.GameFlow.Menu
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("Tutorial");
        }
    }
}