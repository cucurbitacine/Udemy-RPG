using Game.Saving;
using UnityEngine;

namespace Game.Core
{
    public abstract class PickupItem : MonoBehaviour, ISaveable, IRaycastable
    {
        public float radiusPickup = 1f;
        public float respawnTime = 5f;

        #region Public API

        public void Respawn()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide(float duration)
        {
            gameObject.SetActive(false);
            
            Invoke(nameof(Respawn), duration);
        }

        public void Hide()
        {
            Hide(respawnTime);
        }

        public bool CanPickup(GameObject context)
        {
            var distance = Vector3.Distance(context.transform.position, transform.position);
            
            return context.CompareTag("Player") && 0 < radiusPickup && distance < radiusPickup;
        }

        public bool Pickup(GameObject context)
        {
            if (CanPickup(context) && TryPickup(context))
            {
                Hide();

                return true;
            }

            return false;
        }

        #endregion
        
        #region Virtual

        public virtual CursorType GetCursor()
        {
            return CursorType.Pickup;
        }
        
        public virtual bool HandleRaycast(GameObject context)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Pickup(context);
            }
            
            return CanPickup(context);
        }
        
        public virtual object CaptureState()
        {
            return gameObject.activeSelf;
        }

        public virtual void RestoreState(object state)
        {
            if (state is bool activeSelf)
            {
                if (activeSelf)
                {
                    Respawn();
                }
                else
                {
                    Hide();
                }
            }
        }
        
        #endregion

        #region Abstract

        protected abstract bool TryPickup(GameObject context);

        #endregion
        
        private void OnTriggerEnter(Collider other)
        {
            var context = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
            
            Pickup(context);
        }
    }

    public abstract class PickupItem<T> : PickupItem where T : MonoBehaviour
    {
        protected abstract bool PickupTyped(T t);
        
        protected override bool TryPickup(GameObject context)
        {
            return context.TryGetComponent<T>(out var t) && PickupTyped(t);
        }
    }
}