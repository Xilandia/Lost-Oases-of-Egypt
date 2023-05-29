using UnityEngine;
using _Scripts.Utility.Static;
using _Scripts.Enemy.Unit;
using _Scripts.Utility.Entity;

namespace _Scripts.Enemy.Management
{
    public class EnemyBehaviorTransition : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (Utilities.IsInLayerMask(other.gameObject.layer, EntityHandler.instance.enemyUnitLayer))
            {
                other.gameObject.GetComponent<EnemyUnit>().TransitionPhase();
            }
        }
    }
}