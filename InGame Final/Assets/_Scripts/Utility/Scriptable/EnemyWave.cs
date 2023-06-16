using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Utility.Scriptable
{
    [CreateAssetMenu(fileName = "New Wave", menuName = "Enemy Spawning/Wave")]
    public class EnemyWave : ScriptableObject
    {
        public enum EnemyType
        {
            Basic = 0,
            Climber = 1,
            Fast = 2,
            Tank = 3,
            Boss = 4
        }

        public List<EnemyType> waveEnemyIndex;
        public List<int> waveEnemySides;
    }
}
