using UnityEngine;

namespace _Scripts.Utility.Entity
{
	[CreateAssetMenu(fileName = "New Barracks", menuName = "Entity/Barracks")]
	public class EntityBarracks : ScriptableObject
	{
		public string entityName;
		public GameObject entityPrefab;
		public float entityCostOre;
		public float entityCostWood;
		public float entityHealth;
		public float entityArmor;
		public float entityCreationTime;
		public EntityUnit[] buildableUnits;
	}
}