using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entity", menuName = "New Entity")]
public class Entity : ScriptableObject
{
    public enum entityList
    {
        Worker,
        Robot,
        Building,
        Turret,
        BaseStructure,
        Wall
    };    

    [Header("Entity Settings")]
	public string entityName;

    public entityList entityType;
    
    public GameObject entityPrefab;
	public bool belongsToPlayer;
    
	[Space(20)]
	[Header("Entity Stats")]
    public int entityCost;
    public int entityHealth;
	public int entityArmor;
    public int entityAttack;
	public int entityAttackSpeed;
    public int entityAttackRange;
    public int entityMoveSpeed;
}
