using UnityEngine;

namespace Game.Control.AI
{
    public abstract class AIState : MonoBehaviour
    {
        public abstract bool Process(AIController ai);
    }
}