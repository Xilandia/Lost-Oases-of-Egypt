using System;
using _Scripts.Interaction.Management;
using _Scripts.Player.Structure;
using _Scripts.Player.Unit;
using _Scripts.Utility.Entity;
using UnityEngine;

namespace _Scripts.Interaction.Action
{
    public class ActionHandler : MonoBehaviour
    {
        public static ActionHandler instance;
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
            int place = -1;

            for (int i = 0; i < worker.buildableStructureInfo.Length; i++)
            {
                if (worker.buildableStructureInfo[i].name == buttonName)
                {
                    place = i;
                    break;
                }
            }

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