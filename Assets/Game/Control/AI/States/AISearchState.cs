using UnityEngine;

namespace Game.Control.AI.States
{
    public class AISearchState : AIState
    {
        public float searchSpeed = 3f;
        public float durationSearch = 3f;
        
        public override bool Process(AIController ai)
        {
            ai.movement.agent.speed = searchSpeed;
            
            if (ai.fighter.lastTargetTimeSeen > 0f)
            {
                var duration = Time.time - ai.fighter.lastTargetTimeSeen;

                if (duration < durationSearch)
                {
                    return ai.schedule.Run(ai.movement, m => m.MoveAt(ai.fighter.lastTargetPosition));
                }
            }

            return false;
        }
    }
}