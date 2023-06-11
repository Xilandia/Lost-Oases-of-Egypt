using UnityEngine;

namespace _Scripts.Utility.Entity
{
	[CreateAssetMenu(fileName = "New Enemy", menuName = "Entity/Enemy")]
	public class EntityEnemy : ScriptableObject
	{
		public string entityName;
		public GameObject entityPrefab;
		public float entityHealth;
		public float entityArmor;
		public float entityAttack;
		public float entityTimeBetweenAttacks;
		public float entityAttackRange;
		public float entityAggroRange;
		public float entityMoveSpeed;
	}
}