using _Scripts.GameFlow.Transition;
using _Scripts.Player.Management;
using _Scripts.Player.Unit;
using UnityEngine;

namespace _Scripts.GameFlow.Objective
{
    public class StatueHandler : MonoBehaviour
    {
        public static StatueHandler instance;

        public Statue statue;

        void Awake()
        {
            instance = this;
        }

        public void StartStatueStage()
        {
            statue.statueCollider.enabled = true;

            foreach (PlayerWorker worker in PlayerManager.instance.workers)
            {
                worker.MoveWorker(statue.transform.position);
            }
        }

        public void StatueIsFinished()
        {
            // play sound + animation
            statue.gameObject.SetActive(false);
            StageTransitionHandler.instance.readyToLoadStage = true;
            StageTransitionHandler.instance.LoadStage();
        }
    }
}
