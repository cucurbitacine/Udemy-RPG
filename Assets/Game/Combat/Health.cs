using UnityEngine;

namespace Game.Combat
{
    public class Health : MonoBehaviour
    {
        public float points = 100f;
        
        public bool isDied = false;
        
        private static readonly int TriggerDeath = Animator.StringToHash("Death");

        public Animator animator { get; private set; }
        
        public void Damage(float amount)
        {
            if (amount > 0)
            {
                points = Mathf.Max(0, points - amount);
            }

            if (points == 0f)
            {
                Die();
            }
        }

        public void Die()
        {
            if(isDied) return;
            isDied = true;
            animator.SetTrigger(TriggerDeath);
        }
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
    }
}