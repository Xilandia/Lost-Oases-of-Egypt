using System.Collections;
using _Scripts.GameFlow.Sound;
using _Scripts.GameFlow.Transition;
using _Scripts.Player.Management;
using _Scripts.Player.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.GameFlow.Objective
{
    public class StatueHandler : MonoBehaviour
    {
        public static StatueHandler instance;

        public Statue statue;
        public Image blackoutImage;
        public AudioClip statueExplosionSound;

        void Awake()
        {
            instance = this;
        }

        public void StartStatueStage()
        {
            statue.statueCollider.enabled = true;

            foreach (PlayerWorker worker in PlayerManager.instance.workers)
            {
                worker.MoveWorker(statue.statue.transform.position);
            }
        }

        public void StatueIsFinished()
        {
            // play sound + animation
            statue.gameObject.SetActive(false);
            StartCoroutine(StatueExplosion());
        }

        private void EndStageFour()
        {
            StageTransitionHandler.instance.readyToLoadStage = true;
            StageTransitionHandler.instance.LoadStage();
            ScoreHandler.instance.StatueBuilt();
            
            foreach (PlayerWorker worker in statue.workersInvolvedInConstruction)
            {
                worker.isAttemptingToBuild = false;
                worker.isConstructing = false;
                worker.isAttemptingToGather = false;
                worker.isBuildingTower = false;
                worker.isBuildingStatue = false;
                worker.structureTarget = null;
                worker.constructionStatue = null;

                worker.CheckForResourceTargets();
            }
        }

        private IEnumerator StatueExplosion()
        {
            Color objectColor = blackoutImage.color;
            
            while (objectColor.a < 1f)
            {
                float fadeAmount = objectColor.a + Time.deltaTime;
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackoutImage.color = objectColor;
                yield return null;
            }
            
            SoundHandler.instance.MuteForExplosion();
            SoundHandler.instance.PlayExplosion(statueExplosionSound);
            yield return new WaitForSeconds(2f);
            
            while (objectColor.a > 0f)
            {
                float fadeAmount = objectColor.a - Time.deltaTime;
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackoutImage.color = objectColor;
                yield return null;
            }
            
            SoundHandler.instance.UnmuteAfterExplosion();
            EndStageFour();
        }
    }
}
