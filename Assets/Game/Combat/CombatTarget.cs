using Game.Attributes;
using Game.Core;
using UnityEngine;

namespace Game.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public Health health { get; private set; }

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        public CursorType GetCursor()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(GameObject context)
        {
            if (health.isDied) return false;
            
            if (context.TryGetComponent<Fighter>(out var fighter))
            {
                var canAttack = fighter.CanAttack(this);
                
                if (canAttack && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    fighter.Attack(this);
                }

                return canAttack;
            }
            
            return false;
        }
    }
}