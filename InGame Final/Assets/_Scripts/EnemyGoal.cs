using UnityEngine;

namespace _Scripts
{
    public class EnemyGoal : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (Helper.IsInLayerMask(other.gameObject.layer, EntityHandler.instance.enemyUnitLayer))
            {
                other.gameObject.GetComponent<EnemyUnit>().TransitionPhase();
            }
        }
    }
}