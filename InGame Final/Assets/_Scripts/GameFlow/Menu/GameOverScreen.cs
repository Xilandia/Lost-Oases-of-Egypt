using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.GameFlow.Menu
{
    public class GameOverScreen : MonoBehaviour
    {
        public Text timeSurvived;

        public void Setup(int time)
        {
            gameObject.SetActive(true);
            timeSurvived.text = time.ToString() + "Time Survived";
        }
    }
}