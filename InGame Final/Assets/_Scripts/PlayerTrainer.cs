using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTrainer : MonoBehaviour
{

    public string trainerName;
    public float trainerCost, trainerHealth, trainerArmor, trainerBuildTime;
    public float trainerCurrentHealth;
    public bool isBuildable, isPrototype, isPlaced, isComplete;
    public Entity[] buildableUnits;
    public ITrainer interactable;

    private bool constructionStarted = false;
    private float currProgress, initYScale;
    
    public Transform myLocation;
    [SerializeField] private Transform spawnPoint;
    private Queue<Entity> unitQueue = new Queue<Entity>();
    private float currentUnitTrainTime;
    private float elapsedTrainingTime;
    private bool isTraining;

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
        Vector3 scale = transform.localScale;
        initYScale = scale.y;
        transform.localScale = new Vector3(scale.x, initYScale / 100, scale.z);
        interactable.OnInteractExit();
    }

    void Update()
    {
        if (constructionStarted)
        {
            currProgress += Time.deltaTime;
            transform.localScale = new Vector3(transform.localScale.x, initYScale * (currProgress / trainerBuildTime), transform.localScale.z);
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
    
    public void CompleteConstruction()
    {
        isPlaced = false;
        isComplete = true;
        transform.localScale = new Vector3(transform.localScale.x, initYScale, transform.localScale.z);
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
       
       if (playerUnit == null)
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

       Debug.Log("Added " + playerUnit.entityName + " to queue, now sized " + unitQueue.Count + " and will take " + currentUnitTrainTime + " seconds to train");
   }

   public void FinishTrainingUnit()
   {
       Entity pU = unitQueue.Dequeue();
       
       //GameObject unit = Instantiate(pU.entityPrefab, spawnPoint.position, Quaternion.identity);
       Debug.Log("Will spawn " + pU.entityName);
       
       //EntityHandler.instance.SetPlayerUnitStats(unit.gameObject.GetComponent<PlayerUnit>(), pU.entityName);
       if (unitQueue.Count > 0) 
       { 
           currentUnitTrainTime = unitQueue.Peek().entityCreationTime;
       }
       else 
       { 
           isTraining = false;
       }
   }
}
