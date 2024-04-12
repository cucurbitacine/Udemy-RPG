using System.Collections.Generic;

namespace Game.Control.Player.Inputs
{
    public class PlayerInputGroup : PlayerInput
    {
        public List<PlayerInput> inputs = new List<PlayerInput>();
        
        public override bool Process(PlayerController player)
        {
            foreach (var input in inputs)
            {
                if (input.Process(player))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}