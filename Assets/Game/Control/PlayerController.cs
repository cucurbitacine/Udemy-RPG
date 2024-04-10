using Game.Combat;
using Game.Core;
using Game.Movement;
using UnityEngine;

namespace Game.Control
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerInput input;

        [Space]
        public GameObject playerObject;
        
        public MovementController movement { get; private set; }
        public Fighter fighter { get; private set; }
        public CombatTarget target { get; private set; }
        public ActionSchedule schedule { get; private set; }

        private void Awake()
        {
            if (playerObject)
            {
                movement = playerObject.GetComponent<MovementController>();
                fighter = playerObject.GetComponent<Fighter>();
                target = playerObject.GetComponent<CombatTarget>();
                schedule = playerObject.GetComponent<ActionSchedule>();
            }
        }

        private void Update()
        {
            if (input)
            {
                input.Process(this);
            }
        }
    }
}