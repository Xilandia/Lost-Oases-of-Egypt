using UnityEngine;

namespace _Scripts.Utility.Entity
{
	[CreateAssetMenu(fileName = "New Barracks", menuName = "Entity/Barracks")]
	public class EntityBarracks : EntityUI
	{
		public GameObject entityPrefab;
		public float entityHealth;
		public float entityArmor;
		public float entityCreationTime;
		public EntityUnit[] buildableUnits;
		public EntityUI[] buildableUnitInfo;
		public AudioClip entityBuildSound;
		public AudioClip entityTrainSound;
		public AudioClip entityDamagedSound;
		public AudioClip entityDeathSound;
	}
}