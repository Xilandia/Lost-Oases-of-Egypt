using System.Collections;
using System.Collections.Generic;
using _Scripts.Player.Management;
using UnityEngine;

namespace _Scripts.GameFlow.Sound
{
    public class SoundHandler : MonoBehaviour
    {
        public AudioClip[] music;
        public AudioClip[] soundEffects;
        public AudioSource musicSource;
        public AudioSource soundEffectSource;

        private float lastChange = 0;
        private int currTrack = 0;

        void Update()
        {
            if (PlayerManager.instance.roundTimer[2] - lastChange > 200)
            {
                lastChange = 0;
                musicSource.PlayOneShot(music[currTrack]);
                currTrack++;
            }
        }
    }
}