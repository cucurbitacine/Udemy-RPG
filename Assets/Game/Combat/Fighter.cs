using System.Linq;
using Game.Core;
using Game.Movement;
using Game.Saving;
using UnityEngine;

namespace Game.Combat
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(CombatTarget))]
    public class Fighter : MonoBehaviour, IActor, ISaveable
    {
        [Header("Weapons")]
        public GameObject weaponObject;
        
        [Space]
        public Weapon defaultWeapon;
        public Weapon currentWeapon;
        
        [Header("Target")]
        public CombatTarget target;
        
        [Space]
        public float lastTargetTimeSeen = 0f;
        public Vector3 lastTargetPosition = Vector3.zero;
        
        [Header("Attack")]
        public Vector3 centerAttack = Vector3.forward;
        public Vector3 sizeAttack = Vector3.one;
        public LayerMask layerAttack = 1;
        
        [Header("Joints")]
        public Transform handRight = null;
        public Transform handLeft = null;
        
        public MovementController movement { get; private set; }
        public CombatTarget selfTarget { get; private set; }
        public Animator animator { get; private set; }

        private float _lastAttack = float.MinValue;
        private RuntimeAnimatorController _defaultAnimatorController;
        
        private readonly Collider[] _overlap = new Collider[32];
        
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
            if (!currentWeapon) return;
            
            if (Time.time - _lastAttack < currentWeapon.cooldown) return;

            if (target)
            {
                movement.LookAt(target.transform);
                
                if (target.health.points == 0f)
                {
                    ResetTarget();
                    return;
                }
            }
            
            _lastAttack = Time.time;

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

        public void EquipWeapon(Weapon weapon)
        {
            if (weapon)
            {
                weapon.Equip(this);
            }
        }
        
        public object CaptureState()
        {
            if (currentWeapon)
            {
                return currentWeapon.name;
            }

            return null;
        }

        public void RestoreState(object state)
        {
            if (state is string weaponName)
            {
                var weapon = Resources.Load<Weapon>(weaponName);
                
                EquipWeapon(weapon);
            }
        }

        public void ResetAnimator()
        {
            var overideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (overideController)
            {
                animator.runtimeAnimatorController = overideController.runtimeAnimatorController;
            }
        }
        
        private int Overlap(Collider[] overlap)
        {
            var center = transform.TransformPoint(centerAttack);
            var size = sizeAttack * 0.5f;
            return Physics.OverlapBoxNonAlloc(center, size, overlap, transform.rotation, layerAttack, QueryTriggerInteraction.Collide);
        }
        
        private void DamageTarget(CombatTarget combatTarget)
        {
            if (currentWeapon && combatTarget)
            {
                combatTarget.health.Damage(currentWeapon.damage);    
            }
        }
        
        private void DamageTarget()
        {
            if (target)
            {
                DamageTarget(target);
            }
            else
            {
                var count = Overlap(_overlap);
                foreach (var cld in _overlap.Take(count))
                {
                    if (cld.TryGetComponent<CombatTarget>(out var cldTarget))
                    {
                        if (CanAttack(cldTarget))
                        {
                            DamageTarget(cldTarget);
                        }
                    }
                }
            }
        }
        
        // Animation event
        private void Hit()
        {
            DamageTarget();
        }

        // Animation event
        private void Shoot()
        {
            if (currentWeapon)
            {
                currentWeapon.Shoot(this);
            }
        }
        
        private void Awake()
        {
            movement = GetComponent<MovementController>();
            animator = GetComponent<Animator>();
            selfTarget = GetComponent<CombatTarget>();

            _defaultAnimatorController = animator.runtimeAnimatorController;
        }

        private void Start()
        {
            EquipWeapon(currentWeapon ? currentWeapon : defaultWeapon);
        }

        private void Update()
        {
            if (target && movement && currentWeapon)
            {
                lastTargetPosition = target.transform.position;
                
                if (Vector3.Distance(movement.position, target.transform.position) < currentWeapon.attackRange)
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

            var weapon = currentWeapon;
            if (weapon == null) weapon = defaultWeapon;
            
            if (weapon)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, weapon.attackRange);
            }
            
            var center = transform.TransformPoint(centerAttack);
            var size = sizeAttack * 0.5f;
            Gizmos.DrawWireCube(center, size);
        }
    }
}
