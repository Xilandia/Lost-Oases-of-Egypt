using System.Collections;
using System.Collections.Generic;
using _Scripts.Player.Management;
using UnityEngine;

namespace _Scripts.GameFlow.Sound
{
    public class SoundHandler : MonoBehaviour
    {
        public static SoundHandler instance;
        
        [SerializeField] private AudioClip openingTrack;
        [SerializeField] private AudioClip transitionTrack;
        [SerializeField] private AudioClip[] stageTracks;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource soundEffectSource;

        void Awake()
        {
            instance = this;
        }

        public void PlayOpeningTrack()
        {
            musicSource.clip = openingTrack;
            musicSource.Play();
        }

        public void PlayTransitionTrack()
        {
            musicSource.clip = transitionTrack;
            musicSource.Play();
        }
        
        public void PlayStageTrack(int stage)
        {
            musicSource.clip = stageTracks[stage];
            musicSource.Play();
        }

        public void PlaySoundEffect(AudioClip clip)
        {
            soundEffectSource.PlayOneShot(clip, 0.2f);
        }

        public void MuteForExplosion()
        {
            musicSource.mute = true;
        }
        
        public void UnmuteAfterExplosion()
        {
            musicSource.mute = false;
        }

        public void PlayExplosion(AudioClip clip)
        {
            soundEffectSource.PlayOneShot(clip);
        }
    }
}