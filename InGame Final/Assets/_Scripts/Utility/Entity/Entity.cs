using UnityEngine;

namespace _Scripts.Utility.Entity
{
	[CreateAssetMenu(fileName = "New Entity", menuName = "New Entity")]
	public class Entity : ScriptableObject
	{
		[Header("Entity Settings")] 
		public string entityName;

		public GameObject entityPrefab;

		[Space(20)] [Header("Entity Stats")] 
		public float entityCost;
		public float entityHealth;
		public float entityArmor;
		public float entityAttack;
		public float entityTimeBetweenAttacks;
		public float entityAttackRange;
		public float entityAggroRange;
		public float entityMoveSpeed;
		public float entityCreationTime;

		[Space(20)] [Header("Building Attributes")]
		public bool isBuildable;

		public bool isPrototype;
		public bool isPlaced;
		public bool isComplete;
		public Entity[] buildableUnits;
	}
}