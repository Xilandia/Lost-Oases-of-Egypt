using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTrainer : MonoBehaviour, Damagable
{

    public string trainerName;
    public float trainerCost, trainerHealth, trainerArmor, trainerBuildTime;
    public float trainerCurrentHealth;
    
    public GameObject trainerPrefab;
    public Transform trainerTransform;
    public PlacableObject trainerPlacable;
    
    public bool isPrototype, isPlaced, isComplete;
    public Entity[] buildableUnits;
    public ITrainer interactable;

    private bool constructionStarted = false;
    private float currProgress, initYScale;
    
    [SerializeField] private Transform spawnPoint;
    private Queue<Entity> unitQueue = new Queue<Entity>();
    private float currentUnitTrainTime;
    private float elapsedTrainingTime;
    private bool isTraining;

    private Vector3 originalScale;

    public void UpdatePrototypePosition()
    {
        if (isPrototype)
        {
            transform.position = BuildingHandler.SnapToGrid(BuildingHandler.GetMouseWorldPosition());
        }
    }

    public void StartConstruction()
    {
        constructionStarted = true;
        isPlaced = true;
        currProgress = 0f;
        originalScale = transform.localScale;
        transform.localScale = new Vector3(originalScale.x, originalScale.y / 100, originalScale.z);
        interactable.OnInteractExit();
    }

    void Update()
    {
        if (constructionStarted)
        {
            currProgress += Time.deltaTime;
            transform.localScale = new Vector3(originalScale.x, initYScale * (currProgress / trainerBuildTime), originalScale.z);
            if (currProgress >= trainerBuildTime)
            {
                CompleteConstruction();
            }
        }

        if (isTraining)
        {
            elapsedTrainingTime += Time.deltaTime;
            if (elapsedTrainingTime >= currentUnitTrainTime)
            {
                elapsedTrainingTime = 0;
                FinishTrainingUnit();
            }
        }
    }
    
    private void CompleteConstruction()
    {
        isPlaced = false;
        isComplete = true;
        transform.localScale = originalScale;
    }
    
   public void AddToQueue(string unitName)
   {
       Entity playerUnit = null;
       foreach (Entity entity in buildableUnits)
       {
           if (entity.entityName == unitName)
           {
               playerUnit = entity;
               break;
           }
       }
       
       if (playerUnit.Equals(null))
       {
           Debug.LogError("PlayerTrainer.cs: AddToQueue(): playerUnit is null");
           return;
       }
       
       if (unitQueue.Count == 0)
       {
           unitQueue.Enqueue(playerUnit);
           currentUnitTrainTime = playerUnit.entityCreationTime;
           isTraining = true;
       }
       else
       {
           unitQueue.Enqueue(playerUnit);
       }
   }

   public void FinishTrainingUnit()
   {
       Entity pU = unitQueue.Dequeue();
       
       GameObject unit = Instantiate(pU.entityPrefab, spawnPoint.position, Quaternion.identity);
       unit.transform.SetParent(PlayerManager.instance.playerUnits);
       
       EntityHandler.instance.SetPlayerUnitStats(unit.gameObject.GetComponent<PlayerUnit>(), pU.entityName);
       if (unitQueue.Count > 0) 
       { 
           currentUnitTrainTime = unitQueue.Peek().entityCreationTime;
       }
       else 
       { 
           isTraining = false;
       }
   }
   
   public void TakeDamage(float damage)
   {
       float totalDamage = damage - trainerArmor;
       trainerCurrentHealth -= Math.Max(totalDamage, 1);

       if (trainerCurrentHealth <= 0)
       {
           // make sound, do something?
           Destroy(gameObject);
       }
   }
}
