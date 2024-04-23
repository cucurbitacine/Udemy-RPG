using Game.Attributes;
using UnityEngine;

namespace Game.UI
{
    public class DamageTextSpawner : MonoBehaviour
    {
        public Health health;
        
        [Space]
        public DamageText textPrefab;

        public void Spawn(float damage)
        {
            if (textPrefab)
            {
                var text = Instantiate(textPrefab, transform);

                text.Play(damage);
            }
        }

        private void Awake()
        {
            if (health == null)
            {
                health = GetComponentInParent<Health>();
            }
        }

        private void OnEnable()
        {
            if (health)
            {
                health.changed.AddListener(Spawn);
            }
        }
        
        private void OnDisable()
        {
            if (health)
            {
                health.changed.RemoveListener(Spawn);
            }
        }
    }
}