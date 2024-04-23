using Game.Attributes;
using Game.Combat;
using Game.Core;
using Game.Movement;
using UnityEngine;

namespace Game.Control.Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerInput input;

        [Space]
        public GameObject playerObject;
        public FollowCamera follow;
        
        public MovementController movement { get; private set; }
        public Fighter fighter { get; private set; }
        public CombatTarget target { get; private set; }
        public ActionSchedule schedule { get; private set; }

        public Health health => target.health;
        
        private void Awake()
        {
            if (playerObject)
            {
                movement = playerObject.GetComponent<MovementController>();
                fighter = playerObject.GetComponent<Fighter>();
                target = playerObject.GetComponent<CombatTarget>();
                schedule = playerObject.GetComponent<ActionSchedule>();

                if (follow)
                {
                    follow.followAt = playerObject.transform;
                }
            }
        }

        private void Update()
        {
            if (input && target && target.health && !target.health.isDied)
            {
                input.Process(this);
            }
        }
    }
}