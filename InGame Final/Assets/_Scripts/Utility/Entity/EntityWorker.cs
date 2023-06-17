using _Scripts.Player.Structure;
using UnityEngine;

namespace _Scripts.Utility.Entity
{
	[CreateAssetMenu(fileName = "New Worker", menuName = "Entity/Worker")]
	public class EntityWorker : ScriptableObject
	{
		public string entityName;
		public GameObject entityPrefab;
		public float entityHealth;
		public float entityArmor;
		public float entityTimeBetweenGathering;
		public float entityOperationRange;
		public float entityMoveSpeed;
		public PlayerBarracks[] buildableBarracks;
		public PlayerTower[] buildableTowers;
		public string[] buildableStructureNames;
		public AudioClip entityGatherSound;
		public AudioClip entityDeathSound;
	}
}