using Game.Saving;
using UnityEngine;

namespace Game.Combat
{
    public class WeaponPickup : MonoBehaviour, ISaveable
    {
        public Weapon weapon;
        public float respawnTime = 5f;
        
        private void Respawn()
        {
            gameObject.SetActive(true);
        }
        
        private void Hide(float duration)
        {
            gameObject.SetActive(false);
            
            Invoke(nameof(Respawn), duration);
        }

        private void Hide()
        {
            Hide(respawnTime);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (weapon && other.CompareTag("Player"))
            {
                var fighter = other.GetComponent<Fighter>();

                if (fighter)
                {
                    fighter.EquipWeapon(weapon);
                    
                    Hide();
                }
            }
        }

        public object CaptureState()
        {
            return gameObject.activeSelf;
        }

        public void RestoreState(object state)
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
    }
}
