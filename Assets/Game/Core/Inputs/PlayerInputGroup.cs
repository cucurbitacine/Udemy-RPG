using System.Collections.Generic;
using Game.Characters;

namespace Game.Core.Inputs
{
    public class PlayerInputGroup : PlayerInput
    {
        public List<PlayerInput> inputs = new List<PlayerInput>();
        
        public override void ProcessMovement(Movement movement)
        {
            foreach (var input in inputs)
            {
                input.ProcessMovement(movement);
            }
        }
    }
}