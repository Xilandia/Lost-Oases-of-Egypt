using UnityEngine;

[CreateAssetMenu(fileName = "New Entity", menuName = "New Entity")]
public class Entity : ScriptableObject
{
    public enum entityList
    {
        Worker,
        Robot,
        UnitTrainer,
        Turret,
        BaseStructure,
        Wall,
        Enemy,
        Resource // not sure if relevant
    }    

    [Header("Entity Settings")]
	public string entityName;

    public entityList entityType;
    
    public GameObject entityPrefab;
	public bool belongsToPlayer;
    
	[Space(20)]
	[Header("Entity Stats")]
    public float entityCost;
    public float entityHealth;
	public float entityArmor;
    public float entityAttack;
	public float entityTimeBetweenAttacks;
    public float entityAttackRange;
    public float entityAggroRange;
    public float entityMoveSpeed;
    public float entityCreationTime;
    
    [Space(20)]
    [Header("Building Atributes")]
    public bool isBuilding;
    public bool isBuildable;
    public bool isPrototype;
    public bool isPlaced;
    public bool isComplete;
    public Entity[] buildableUnits;
}
