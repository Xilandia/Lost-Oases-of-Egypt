using UnityEngine;

namespace _Scripts.Utility.Entity
{
	[CreateAssetMenu(fileName = "New Tower", menuName = "Entity/Tower")]
	public class EntityTower : ScriptableObject
	{
		public string entityName;
		public GameObject entityPrefab;
		public float entityCostOre;
		public float entityCostWood;
		public float entityHealth;
		public float entityArmor;
		public float entityAttack;
		public float entityTimeBetweenAttacks;
		public float entityAttackRange;
		public float entityCreationTime;
	}
}