using System;
using UnityEngine;

namespace Game.Combat
{
    [CreateAssetMenu(menuName = "RPG/Create Weapon", fileName = "Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [Min(0f)] public float damage = 5f;
        [Min(0f)] public float cooldown = 1f;
        [SerializeField] private float _damagePerSec;
        
        [Space]
        [Min(0f)] public float attackRange = 1f;
        [Min(0f)] public float percentageModifier = 0f;
        
        [Space]
        public GameObject equipPrefab = null;
        public GameObject pickupPrefab = null;
        public GameObject projectilePrefab = null;
        
        [Space]
        public bool isRightHand = true;
        public AnimatorOverrideController animatorOverride = null;
        
        public void Equip(Fighter fighter)
        {
            if (fighter == null) return;

            var hand = isRightHand ? fighter.handRight : fighter.handLeft;
            
            if (hand)
            {
                if (fighter.weaponObject)
                {
                    Destroy(fighter.weaponObject);
                }
                
                fighter.currentWeapon = Instantiate(this);
                fighter.currentWeapon.name = name; // IMPORTANT
                
                if (equipPrefab)
                {
                    fighter.weaponObject = Instantiate(equipPrefab, hand);
                }
                    
                if (animatorOverride)
                {
                    fighter.animator.runtimeAnimatorController = animatorOverride;
                }
                else
                {
                    fighter.ResetAnimator();
                }
            }
        }

        public void Shoot(Fighter fighter)
        {
            if (fighter == null) return;
            if (fighter.target == null) return;
            var hand = isRightHand ? fighter.handRight : fighter.handLeft;
            if (hand == null) return;

            if (projectilePrefab == null) return;

            var projectileObject = Instantiate(projectilePrefab, hand.position, Quaternion.identity);

            if (projectileObject.TryGetComponent<Projectile>(out var projectile))
            {
                projectile.Shoot(fighter.gameObject, fighter.target, fighter.GetDamage());
            }
        }

        private void OnValidate()
        {
            if (cooldown > 0f)
            {
                _damagePerSec = damage / cooldown;
            }
        }
    }
}
