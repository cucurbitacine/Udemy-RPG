using Game.Attributes;
using Game.Core;
using UnityEngine;

namespace Game.Combat
{
    public class HealthPickup : PickupItem<Health>
    {
        [Header("Healing")]
        public float healing = 50;
        
        protected override bool PickupTyped(Health t)
        {
            if (healing > 0f)
            {
                t.Heal(healing);

                return true;
            }

            return false;
        }
    }
}