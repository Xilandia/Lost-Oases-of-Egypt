using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.GameFlow.Objective
{
    public class BoatCollider : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hero"))
            {
                ScoreHandler.instance.GameEndScoreCalculation(true);
                SceneManager.LoadScene("Win Screen");
            }
        }
    }
}
