using UnityEngine;

namespace Game.Combat
{
    [CreateAssetMenu(menuName = "RPG/Create Weapon Model", fileName = "Weapon Model", order = 0)]
    public class WeaponModel : ScriptableObject
    {
        [Min(0f)] public float damage = 5f;
        [Min(0f)] public float cooldown = 1f;
        [Min(0f)] public float attackRange = 1f;
        
        [Space]
        [SerializeField] private float _damagePerSec;
        [Range(0, 1)]
        [SerializeField] private float _volumeUsage = 1f;
        [SerializeField] private float _weaponPower;
        
        [Space]
        [Min(0f)] public float percentageModifier = 0f;
        
        [Space]
        public WeaponEquipped equipPrefab = null;
        public WeaponPickup pickupPrefab = null;
        public Projectile projectilePrefab = null;
        
        [Space]
        public bool isRightHand = true;
        public AnimatorOverrideController animatorOverride = null;
        
        public void Equip(Fighter fighter)
        {
            if (fighter == null) return;

            var hand = isRightHand ? fighter.handRight : fighter.handLeft;
            
            if (hand)
            {
                if (fighter.weaponEquipped)
                {
                    Destroy(fighter.weaponEquipped.gameObject);
                }
                
                if (equipPrefab)
                {
                    fighter.weaponEquipped = InstantiateWeapon(equipPrefab, hand);
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
                
                if (fighter.weaponEquipped)
                {
                    fighter.weaponEquipped.Sfx();
                }
            }
        }

        private WeaponEquipped InstantiateWeapon(WeaponEquipped prefab, Transform parent)
        {
            var weapon = Instantiate(prefab, parent);
            weapon.SetModel(this);
            return weapon;
        }
        
        private void OnValidate()
        {
            if (cooldown > 0f)
            {
                _damagePerSec = damage * percentageModifier / cooldown;
                _weaponPower = _volumeUsage * _damagePerSec;
            }
        }
    }
}
