using UnityEngine;

namespace _Scripts.Utility.Entity
{
	[CreateAssetMenu(fileName = "New Unit", menuName = "Entity/Unit")]
	public class EntityUnit : EntityUI
	{
		public GameObject entityPrefab;
		public float entityHealth;
		public float entityArmor;
		public float entityAttack;
		public float entityTimeBetweenAttacks;
		public float entityAttackRange;
		public float entityAggroRange;
		public float entityMoveSpeed;
		public float entityCreationTime;
		public AudioClip entityAttackSound;
		public AudioClip entityDeathSound;
	}
}