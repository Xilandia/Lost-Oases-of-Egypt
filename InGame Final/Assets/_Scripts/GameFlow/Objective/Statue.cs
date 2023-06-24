using System;
using System.Collections.Generic;
using _Scripts.GameFlow.Sound;
using _Scripts.Player.Unit;
using _Scripts.Utility.Interface;
using _Scripts.Utility.Popup;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.GameFlow.Objective
{
    public class Statue : MonoBehaviour, IDamageable
    {
        public GameObject statue;
        public CapsuleCollider statueCollider;
        public GameObject outerScaffolding;
        public GameObject innerScaffolding;
        public bool outerScaffoldingUp;
        public bool innerScaffoldingUp;

        public float statueHealth;
        public float statueArmor;
        public float statueCurrentHealth;
        public float statueBuildTime;
        public int statueOffset;
        public AudioClip statueDamagedSound;
        public AudioClip statueBuildSound;
        public Transform statuePopupSpawnPosition;
        public List<PlayerWorker> workersInvolvedInConstruction = new List<PlayerWorker>();

        private float currProgress;
        private float prevSoundEffect;

        public void TickConstruction()
        {
            currProgress += Time.deltaTime;

            if (currProgress >= prevSoundEffect + 1f)
            {
                prevSoundEffect = currProgress;
                SoundHandler.instance.PlaySoundEffect(statueBuildSound);
            }

            if (!outerScaffoldingUp && currProgress >= 0.3f * statueBuildTime)
            {
                outerScaffoldingUp = true;
                outerScaffolding.SetActive(true);
            }

            if (!innerScaffoldingUp && currProgress >= 0.6f * statueBuildTime)
            {
                innerScaffoldingUp = true;
                innerScaffolding.SetActive(true);
            }

            if (currProgress >= statueBuildTime)
            {
                Debug.Log("Statue complete");
                StatueHandler.instance.StatueIsFinished();
            }
        }

        public int GetOffset()
        {
            return statueOffset;
        }

        public void TakeDamage(float damage)
        {
            float totalDamage = Math.Max(damage - statueArmor, 1);
            statueCurrentHealth -= totalDamage;
            PopupHandler.instance.CreatePopup("-" + totalDamage + " Health!", new Color(139, 0, 0),
                statuePopupSpawnPosition.position);
            SoundHandler.instance.PlaySoundEffect(statueDamagedSound);

            CheckIfDead();
        }

        private void CheckIfDead()
        {
            if (statueCurrentHealth <= 0)
            {
                ScoreHandler.instance.GameEndScoreCalculation(false);
                SceneManager.LoadScene("Lose Screen");
            }
        }
    }
}