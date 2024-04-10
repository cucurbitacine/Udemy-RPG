using Game.Movement;
using UnityEngine;

namespace Game.Control.Inputs
{
    public class PlayerInputKeyboard : PlayerInput
    {
        public Vector3 move = Vector3.zero;
        
        public override bool Process(PlayerController player)
        {
            move.x = Input.GetAxisRaw("Horizontal");
            move.y = 0;
            move.z = Input.GetAxisRaw("Vertical");

            if (cameraMain && move != Vector3.zero)
            {
                move.x = Input.GetAxis("Horizontal");
                move.y = 0;
                move.z = Input.GetAxis("Vertical");
                
                var forward = Vector3.ProjectOnPlane(cameraMain.transform.forward, Vector3.up).normalized;
                var right = Vector3.ProjectOnPlane(cameraMain.transform.right, Vector3.up).normalized;
                
                move = right * move.x + forward * move.z;

                player.schedule.StartAction(player.movement, t => t.Move(move));
                return true;
            }

            return false;
        }
    }
}