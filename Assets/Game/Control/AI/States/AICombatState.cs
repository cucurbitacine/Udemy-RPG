using System;
using Game.Combat;
using UnityEngine;

namespace Game.Control.AI.States
{
    public class AICombatState : AIState
    {
        public float chaseDistance = 5f;

        [Space]
        public CombatTarget player;
        
        public override bool Process(AIController ai)
        {
            if (player && IsValidDistance(ai.fighter.transform, player.transform))
            {
                if (ai.fighter.CanAttack(player))
                {
                    return ai.schedule.Run(ai.fighter, f => f.Attack(player));
                }
            }

            ai.fighter.ResetTarget();
            return false;
        }

        private bool IsValidDistance(Transform origin, Transform target)
        {
            return Vector3.Distance(origin.position, target.position) < chaseDistance;
        }

        private void Start()
        {
            player = GameObject.FindWithTag("Player").GetComponent<CombatTarget>();
        }
    }
}