using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    [RequireComponent(typeof(Collider))]
    public class TriggerZone : MonoBehaviour
    {
        public bool wasTriggered = false;
        
        [Space]
        public bool invokeOnce = false;

        [Space] public UnityEvent onTriggered = new UnityEvent();
        
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (invokeOnce)
                {
                    if (!wasTriggered)
                    {
                        wasTriggered = true;
                        onTriggered.Invoke();
                    }
                }
                else
                {
                    onTriggered.Invoke();
                }
            }
        }
    }
}