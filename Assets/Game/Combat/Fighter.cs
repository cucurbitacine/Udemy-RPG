using System.Collections.Generic;
using Game.Core;
using Game.Movement;
using Game.Saving;
using Game.Stats;
using UnityEngine;

namespace Game.Combat
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(CombatTarget))]
    [RequireComponent(typeof(ActionSchedule))]
    public class Fighter : MonoBehaviour, IActor, ISaveable, IModifier
    {
        [Header("Weapons")]
        public WeaponEquipped weaponEquipped;
        
        [Space]
        public WeaponModel defaultWeaponModel;
        
        [Header("Target")]
        public CombatTarget target;
        
        [Space]
        public float lastTargetTimeSeen = 0f;
        public Vector3 lastTargetPosition = Vector3.zero;
        
        [Header("Joints")]
        public Transform handRight = null;
        public Transform handLeft = null;
        
        public MovementController movement { get; private set; }
        public CombatTarget selfTarget { get; private set; }
        public Animator animator { get; private set; }
        public ActionSchedule schedule { get; private set; }

        public WeaponModel currentWeaponModel => weaponEquipped ? weaponEquipped.model : null;
        
        private float _lastAttack = float.MinValue;
        
        private static readonly int TriggerAttack = Animator.StringToHash("Attack");
        private static readonly int TriggerStopAttack = Animator.StringToHash("StopAttack");

        public bool CanAttack(CombatTarget combatTarget)
        {
            if (combatTarget)
            {
                if (combatTarget == selfTarget) return false;
                if (combatTarget.health.isDied) return false;
                
                return combatTarget.health.points > 0;
            }

            return false;
        }
        
        public void Attack()
        {
            if (!currentWeaponModel) return;
            
            if (Time.time - _lastAttack < currentWeaponModel.cooldown) return;

            if (target)
            {
                movement.LookAt(target.transform);
                
                if (target.health.points == 0f)
                {
                    ResetTarget();
                    return;
                }
            }

            schedule.Run(this);
            
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

        public void EquipWeapon(WeaponModel weaponModel)
        {
            if (weaponModel)
            {
                weaponModel.Equip(this);
            }
        }
        
        public object CaptureState()
        {
            if (currentWeaponModel)
            {
                return currentWeaponModel.name;
            }

            return null;
        }

        public void RestoreState(object state)
        {
            if (state is string weaponName)
            {
                var weapon = Resources.Load<WeaponModel>(weaponName);

                if (weapon)
                {
                    EquipWeapon(weapon);
                }
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

        public float GetDamage()
        {
            if (TryGetComponent<BaseStats>(out var stats))
            {
                return stats.GetStat(StatsType.Damage);
            }

            return GetWeaponDamage();
        }

        private float GetWeaponDamage()
        {
            return currentWeaponModel ? currentWeaponModel.damage : 0f;
        }
        
        private void DamageTarget(CombatTarget combatTarget)
        {
            if (currentWeaponModel && combatTarget)
            {
                combatTarget.health.Damage(gameObject, GetDamage());
                
                if (weaponEquipped)
                {
                    weaponEquipped.Sfx();
                }
            }
        }

        // Animation event
        private void Hit()
        {
            if (target)
            {
                DamageTarget(target);
            }
        }

        // Animation event
        private void Shoot()
        {
            if (currentWeaponModel)
            {
                currentWeaponModel.Shoot(this);
            }
        }
        
        public IEnumerable<float> GetModifier(StatsType statsType)
        {
            if (statsType == StatsType.Damage)
            {
                yield return GetWeaponDamage();
            }
        }

        public IEnumerable<float> GetPercentage(StatsType statsType)
        {
            if (currentWeaponModel)
            {
                yield return currentWeaponModel.percentageModifier;
            }
        }
        
        private void Awake()
        {
            movement = GetComponent<MovementController>();
            animator = GetComponent<Animator>();
            selfTarget = GetComponent<CombatTarget>();
            schedule = GetComponent<ActionSchedule>();
        }

        private void OnEnable()
        {
            EquipWeapon(currentWeaponModel ? currentWeaponModel : defaultWeaponModel);
        }

        private void Update()
        {
            if (!selfTarget.health.isDied && target && movement && currentWeaponModel)
            {
                lastTargetPosition = target.transform.position;
                
                if (Vector3.Distance(movement.position, target.transform.position) < currentWeaponModel.attackRange)
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

            var weapon = currentWeaponModel;
            if (weapon == null) weapon = defaultWeaponModel;
            
            if (weapon)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, weapon.attackRange);
            }
        }
    }
}
