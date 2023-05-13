using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStructure : MonoBehaviour, Damagable
{
    public static BaseStructure instance;
    public float baseStructureHealth;
    public float baseStructureCurrentHealth;
    public float baseStructureArmor;

    void Awake()
    {
        instance = this;
        baseStructureCurrentHealth = baseStructureHealth;
    }
    
    public void TakeDamage(float damage)
    {
        float totalDamage = damage - baseStructureArmor;
        baseStructureCurrentHealth -= totalDamage;
        
        if (baseStructureCurrentHealth <= 0)
        {
            BaseStructureDeath();
        }
    }
    
    private void BaseStructureDeath()
    {
        Debug.Log("Base Structure Destroyed!");
        // handle game over
    }

    public void IncreaseProgression()
    {
        EnemySpawnManager.instance.progressionMultiplier *= 1.5f;
    }
}
