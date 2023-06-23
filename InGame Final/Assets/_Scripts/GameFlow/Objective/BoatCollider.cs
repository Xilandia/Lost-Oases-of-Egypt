using UnityEngine;

namespace _Scripts.GameFlow.Objective
{
    public class BoatCollider : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hero"))
            {
                Debug.Log("You win!");
                // implement proper victory
            }
        }
    }
}
