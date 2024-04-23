using Game.Core;
using UnityEngine;

namespace Game.Combat
{
    public class WeaponPickup : PickupItem<Fighter>
    {
        [Header("Weapon")]
        public WeaponModel weaponModel;

        protected override bool PickupTyped(Fighter t)
        {
            if (weaponModel)
            {
                t.EquipWeapon(weaponModel);
                
                return true;
            }

            return false;
        }
    }
}
