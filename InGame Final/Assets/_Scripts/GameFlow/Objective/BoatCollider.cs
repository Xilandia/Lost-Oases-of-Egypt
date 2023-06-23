using UnityEngine;

namespace _Scripts.GameFlow.Objective
{
    public class BoatCollider : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hero"))
            {
                ScoreHandler.instance.GameEndScoreCalculation(true);
                Debug.Log("You win!");
                // implement proper victory
            }
        }
    }
}
