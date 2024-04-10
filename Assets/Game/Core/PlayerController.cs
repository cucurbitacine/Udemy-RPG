using Game.Characters;
using Game.Core.Inputs;
using UnityEngine;

namespace Game.Core
{
    public class PlayerController : MonoBehaviour
    {
        public Movement movement;

        [Space]
        public PlayerInput input;
        
        private void Awake()
        {
            if (movement == null) movement = GetComponent<Movement>();
        }

        private void Update()
        {
            if (input && movement)
            {
                input.ProcessMovement(movement);
            }
        }
    }
}