using UnityEngine;

namespace Game.Control.AI.States
{
    public class AIPatrolState : AIState
    {
        public int index = 0;
        public float threshold = 0.1f;
        public float waitingTime = 3f;
        
        [Space]
        public PatrolPath path;

        private float _timeStop = float.MinValue;
        
        public override bool Process(AIController ai)
        {
            if (path && path.points.Count > 0)
            {
                var point = path.points[index];
                if (point)
                {
                    var origin = Vector3.ProjectOnPlane(ai.movement.position, Vector3.up);
                    var target = Vector3.ProjectOnPlane(point.position, Vector3.up);

                    if (Vector3.Distance(origin, target) > threshold)
                    {
                        _timeStop = Time.time;
                        return ai.schedule.Run(ai.movement, m => m.MoveAt(point.position));
                    }

                    var waiting = Time.time - _timeStop;
                    
                    if (waiting < waitingTime)
                    {
                        return true;
                    }
                    
                    index = (index + 1) % path.points.Count;
                }
            }
            else
            {
                return ai.schedule.Run(ai.movement, m => m.MoveAt(ai.initialPosition));
            }

            return false;
        }
    }
}