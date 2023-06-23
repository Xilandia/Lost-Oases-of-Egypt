using _Scripts.GameFlow.Menu;
using _Scripts.GameFlow.Objective;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Player.Unit
{
    public class PlayerHero : MonoBehaviour
    {
        [SerializeField] private PlayerUnit heroUnit;

        void OnDisable()
        {
            if (!PauseMenuHandler.instance.isGamePaused)
            {
                ScoreHandler.instance.GameEndScoreCalculation(false);
                SceneManager.LoadScene("Lose Screen");
            }
        }
    }
}