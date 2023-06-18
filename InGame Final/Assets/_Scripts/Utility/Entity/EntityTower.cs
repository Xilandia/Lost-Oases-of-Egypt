using UnityEngine;

namespace _Scripts.Utility.Entity
{
	[CreateAssetMenu(fileName = "New Tower", menuName = "Entity/Tower")]
	public class EntityTower : EntityUI
	{
		public GameObject entityPrefab;
		public float entityHealth;
		public float entityArmor;
		public float entityAttack;
		public float entityTimeBetweenAttacks;
		public float entityAttackRange;
		public float entityCreationTime;
		public AudioClip entityAttackSound;
		public AudioClip entityBuildSound;
		public AudioClip entityDamagedSound;
		public AudioClip entityDeathSound;
	}
}