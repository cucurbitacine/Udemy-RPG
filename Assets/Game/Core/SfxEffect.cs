using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Core
{
    [RequireComponent(typeof(AudioSource))]
    public class SfxEffect : MonoBehaviour
    {
        public List<AudioClip> clips = new List<AudioClip>();

        public AudioSource audioSource { get; private set; }
        
        public void Play()
        {
            if (audioSource && clips != null && clips.Count > 0)
            {
                var clip = clips[Random.Range(0, clips.Count)];
                if (clip)
                {
                    audioSource.PlayOneShot(clip);
                }
            }
        }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
}