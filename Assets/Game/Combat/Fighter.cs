using Game.Core;
using Game.Movement;
using UnityEngine;

namespace Game.Combat
{
    public class Fighter : MonoBehaviour, IActor
    {
        public float cooldown = 1f;
        public float attackRange = 1f;
        public float damage = 5f;
        
        [Space]
        public CombatTarget target;
        
        public MovementController movement { get; private set; }
        public Animator animator { get; private set; }

        private float _lastAttack = float.MinValue;
        
        private static readonly int TriggerAttack = Animator.StringToHash("Attack");
        private static readonly int TriggerStopAttack = Animator.StringToHash("StopAttack");

        public bool CanAttack(CombatTarget combatTarget)
        {
            if (combatTarget == null) return false;
            return combatTarget.health.points > 0;
        }
        
        public void Attack()
        {
            if (Time.time - _lastAttack < cooldown) return;

            if (target)
            {
                if (target.health.points == 0f)
                {
                    ResetTarget();
                    return;
                }
            }
            
            _lastAttack = Time.time;

            if (target)
            {
                movement.LookAt(target.transform);
            }

            if (animator)
            {
                animator.ResetTrigger(TriggerStopAttack);
                animator.SetTrigger(TriggerAttack);
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget;
        }

        public void ResetTarget()
        {
            target = null;
            
            if (animator)
            {
                animator.SetTrigger(TriggerStopAttack);
            }
        }

        public void Cancel()
        {
            ResetTarget();
        }

        // Animation event
        private void Hit()
        {
            if (target)
            {
                target.health.Damage(damage);    
            }
        }
        
        private void Awake()
        {
            movement = GetComponent<MovementController>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (target && movement)
            {
                if (Vector3.Distance(movement.position, target.transform.position) < attackRange)
                {
                    movement.Stop();

                    Attack();
                }
                else
                {
                    if (target.health.points == 0f)
                    {
                        ResetTarget();
                    }
                    else
                    {
                        movement.MoveAt(target.transform.position);
                    }
                }
            }
        }
    }
}
