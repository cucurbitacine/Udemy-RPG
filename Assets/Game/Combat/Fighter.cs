using System;
using Game.Core;
using Game.Movement;
using UnityEngine;

namespace Game.Combat
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(CombatTarget))]
    public class Fighter : MonoBehaviour, IActor
    {
        public float cooldown = 1f;
        public float attackRange = 1f;
        public float damage = 5f;

        [Space]
        public float lastTargetTimeSeen = 0f;
        public Vector3 lastTargetPosition = Vector3.zero;
        
        [Space]
        public CombatTarget target;
        
        public MovementController movement { get; private set; }
        public CombatTarget selfTarget { get; private set; }
        public Animator animator { get; private set; }

        private float _lastAttack = float.MinValue;
        
        private static readonly int TriggerAttack = Animator.StringToHash("Attack");
        private static readonly int TriggerStopAttack = Animator.StringToHash("StopAttack");

        public bool CanAttack(CombatTarget combatTarget)
        {
            if (combatTarget)
            {
                if (combatTarget == selfTarget) return false;
                
                return combatTarget.health.points > 0;
            }

            return false;
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
            if (combatTarget)
            {
                lastTargetTimeSeen = Time.time;
                target = combatTarget;
            }
        }

        public void ResetTarget()
        {
            if (target)
            {
                target = null;
            
                if (animator)
                {
                    animator.SetTrigger(TriggerStopAttack);
                }
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
            selfTarget = GetComponent<CombatTarget>();
        }

        private void Update()
        {
            if (target && movement)
            {
                lastTargetPosition = target.transform.position;
                
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

        private void OnDrawGizmosSelected()
        {
            if (lastTargetTimeSeen > 0f)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(lastTargetPosition, 0.2f);
            }
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
