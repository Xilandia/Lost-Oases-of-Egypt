using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Utility.Scriptable
{
    [CreateAssetMenu(fileName = "New Stage", menuName = "Enemy Spawning/Stage")]
    public class EnemyStage : ScriptableObject
    {
        public List<EnemyWave> waves;
        public float waveLength;
        public int enemyStageGoal;
    }
}
