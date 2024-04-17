using UnityEngine;

namespace Game.Combat
{
    public class DestroyEffect : MonoBehaviour
    {
        public ParticleSystem effect { get; private set; }
        
        private void Awake()
        {
            effect = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (effect && !effect.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}