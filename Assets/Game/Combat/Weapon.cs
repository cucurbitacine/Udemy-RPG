using UnityEngine;

namespace Game.Combat
{
    [CreateAssetMenu(menuName = "Weapons/Create Weapon", fileName = "Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        public float cooldown = 1f;
        public float attackRange = 1f;
        public float damage = 5f;
        public bool isRightHand = true;
        
        [Space]
        public GameObject equipPrefab = null;
        public GameObject pickupPrefab = null;
        public GameObject projectilePrefab = null;
        
        [Space]
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
                projectile.Shoot(fighter.target, damage);
            }
        }
    }
}
