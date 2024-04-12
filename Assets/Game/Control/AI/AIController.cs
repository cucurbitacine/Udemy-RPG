using Game.Combat;
using Game.Core;
using Game.Movement;
using UnityEngine;

namespace Game.Control.AI
{
    public class AIController : MonoBehaviour
    {
        public AIState aiState;
        
        [Space]
        public GameObject aiObject;
        
        public MovementController movement { get; private set; }
        public Fighter fighter { get; private set; }
        public CombatTarget target { get; private set; }
        public ActionSchedule schedule { get; private set; }

        public Vector3 initialPosition { get; private set; }
        
        private void Awake()
        {
            if (aiObject)
            {
                movement = aiObject.GetComponent<MovementController>();
                fighter = aiObject.GetComponent<Fighter>();
                target = aiObject.GetComponent<CombatTarget>();
                schedule = aiObject.GetComponent<ActionSchedule>();

                initialPosition = movement.position;
            }
        }

        private void Update()
        {
            if (aiState && target && target.health && !target.health.isDied)
            {
                aiState.Process(this);
            }
        }
    }
}
