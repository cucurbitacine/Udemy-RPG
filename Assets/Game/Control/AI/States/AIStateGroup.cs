using System.Collections.Generic;
using UnityEngine;

namespace Game.Control.AI.States
{
    public class AIStateGroup : AIState
    {
        public AIState activeState = null;
        public AIState lastState = null;
        
        [Space]
        public List<AIState> states = new List<AIState>();
        
        public override bool Process(AIController ai)
        {
            foreach (var state in states)
            {
                if (state.Process(ai))
                {
                    activeState = state;
                    lastState = state;
                    return true;
                }
            }
            
            activeState = null;
            return false;
        }
    }
}