using Game.Core;
using UnityEngine;

namespace Game.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour
    {
        public Health health { get; private set; }

        private void Awake()
        {
            health = GetComponent<Health>();
        }
    }
}