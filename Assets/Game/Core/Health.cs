using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(ActionSchedule))]
    public class Health : MonoBehaviour
    {
        public float points = 100f;
        
        public bool isDied = false;

        [Space] public UnityEvent onDied = new UnityEvent();
        
        private static readonly int TriggerDeath = Animator.StringToHash("Death");

        public Animator animator { get; private set; }
        public ActionSchedule schedule { get; private set; }
        
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
            if (isDied) return;
            
            isDied = true;
            animator.SetTrigger(TriggerDeath);
            schedule.CancelCurrentActor();
            
            onDied.Invoke();
        }
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
            schedule = GetComponent<ActionSchedule>();
        }
    }
}