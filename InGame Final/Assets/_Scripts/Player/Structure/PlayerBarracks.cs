using System;
using System.Collections.Generic;
using UnityEngine;
using _Scripts.Utility.Interface;
using _Scripts.Utility.Entity;
using _Scripts.Interaction.Interactable;
using _Scripts.Interaction.Management;
using _Scripts.Player.Management;
using _Scripts.Player.Unit;

namespace _Scripts.Player.Structure
{
    public class PlayerBarracks : MonoBehaviour, IDamageable
    {

        public string barracksName;
        public float barracksCost, barracksHealth, barracksArmor, barracksBuildTime;
        public float barracksCurrentHealth;

        public GameObject barracksPrefab;
        public Transform barracksTransform;
        public PlacableObject barracksPlacable;

        public bool isPrototype, isPlaced, isComplete;
        public Entity[] trainableUnits;
        public string[] trainableUnitNames;
        public InteractableBarracks interactable;

        private bool constructionStarted = false;
        private float currProgress, initYScale;
        public List<PlayerWorker> workersInvolvedInConstruction = new List<PlayerWorker>();

        [SerializeField] private Transform spawnPoint;
        private Queue<Entity> unitQueue = new Queue<Entity>();
        private float currentUnitTrainTime;
        private float elapsedTrainingTime;
        private bool isTraining;

        private Vector3 originalScale;

        void Update()
        {
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

            foreach (PlayerWorker worker in InputHandler.instance.selectedWorkers)
            {
                workersInvolvedInConstruction.Add(worker);
                worker.isAttemptingToBuild = true;
                worker.isAttemptingToGather = false;
                worker.isBuildingTower = false;
                worker.structureTarget = gameObject.transform;
                worker.constructionBarracks = this;
                worker.interactable.OnInteractExit();
            }
        }

        public void TickConstruction()
        {
            if (constructionStarted)
            {
                currProgress += Time.deltaTime;
                transform.localScale = new Vector3(originalScale.x, initYScale * (currProgress / barracksBuildTime),
                    originalScale.z);
                
                if (currProgress >= barracksBuildTime)
                {
                    CompleteConstruction();
                }
            }
        }

        private void CompleteConstruction()
        {
            isPlaced = false;
            isComplete = true;
            transform.localScale = originalScale;
            
            foreach (PlayerWorker worker in workersInvolvedInConstruction)
            {
                worker.isAttemptingToBuild = false;
                worker.isAttemptingToGather = false;
                worker.isBuildingTower = false;
                worker.structureTarget = null;
                worker.constructionBarracks = null;

                worker.CheckForResourceTargets();
            }
            
            workersInvolvedInConstruction.Clear();
        }

        public void AddToQueue(string unitName)
        {
            Entity playerUnit = null;
            foreach (Entity entity in trainableUnits)
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
            float totalDamage = damage - barracksArmor;
            barracksCurrentHealth -= Math.Max(totalDamage, 1);

            if (barracksCurrentHealth <= 0)
            {
                // make sound, do something?
                Destroy(gameObject);
            }
        }
    }
}