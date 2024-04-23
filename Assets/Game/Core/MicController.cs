using System;
using UnityEngine;

namespace Game.Core
{
    [RequireComponent(typeof(AudioListener))]
    public class MicController : MonoBehaviour
    {
        public bool paused = false;
        public FollowCamera followCamera;
        
        public AudioListener listener { get; private set; }

        private void Awake()
        {
            listener = GetComponent<AudioListener>();
        }

        private void OnEnable()
        {
            if (followCamera == null)
            {
                followCamera = FindObjectOfType<FollowCamera>();
            }
        }

        private void OnDisable()
        {
            if (followCamera) followCamera = null;
        }

        private void Update()
        {
            if (paused)
            {
                listener.transform.localPosition = Vector3.zero;
            }
            else if (followCamera)
            {
                listener.transform.localPosition = Vector3.forward * followCamera.distance;
            }
        }
    }
}