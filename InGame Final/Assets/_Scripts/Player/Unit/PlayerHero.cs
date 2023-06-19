using _Scripts.GameFlow.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Player.Unit
{
    public class PlayerHero : MonoBehaviour
    {
        [SerializeField] private PlayerUnit heroUnit;

        void OnDisable() // when hero dies
        {
            // lose game
            if (!PauseMenuHandler.instance.isGamePaused)
            {
                SceneManager.LoadScene("Lose Screen");
            }
        }
    }
}