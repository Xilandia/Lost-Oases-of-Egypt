using System;
using _Scripts.Interaction.Management;
using _Scripts.Player.Structure;
using _Scripts.Player.Unit;
using UnityEngine;

namespace _Scripts.Interaction.Action
{
    public class ActionManager : MonoBehaviour
    {
        public static ActionManager instance;
        public bool isTower;

        void Awake()
        {
            instance = this;
        }

        public void TriggerAction(string buttonName)
        {
            if (InputHandler.instance.HaveSelectedWorkers())
            {
                BuildingHandler.instance.InitializeWithObject(SelectStructureFromList(buttonName), isTower);
            }
            else
            {
                InputHandler.instance.selectedBarracks.AddToQueue(buttonName);
            }
        }
        
        private GameObject SelectStructureFromList(string buttonName)
        {
            PlayerWorker worker = InputHandler.instance.selectedWorkers[0];
            int place = Array.IndexOf(worker.buildableStructureNames, buttonName);

            if (place == -1)
            {
                // shouldn't happen, because name comes from the list
                throw new Exception("Name not found in list");
            }

            if (place > worker.buildableBarracks.Length - 1)
            {
                isTower = true;
                return worker.buildableTowers[place - worker.buildableBarracks.Length].towerPrefab;
            }
            else
            {
                isTower = false;
                return worker.buildableBarracks[place].barracksPrefab;
            }
        }
    }
}