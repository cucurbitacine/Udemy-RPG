using System.Collections.Generic;
using UnityEngine;

namespace Game.Control.Player.Inputs
{
    public class PlayerInputGroup : PlayerInput
    {
        public bool paused = false;
        
        [Space]
        public List<PlayerInput> inputs = new List<PlayerInput>();
        
        public override bool Process(PlayerController player)
        {
            if (paused) return false;
            
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