using Game.Characters;
using UnityEngine;

namespace Game.Core.Inputs
{
    public class PlayerInputKeyboard : PlayerInput
    {
        public Vector3 move = Vector3.zero;
        
        public Camera cameraMain => Camera.main;
        
        public override void ProcessMovement(Movement movement)
        {
            move.x = Input.GetAxis("Horizontal");
            move.y = 0;
            move.z = Input.GetAxis("Vertical");

            if (cameraMain && move != Vector3.zero)
            {
                var forward = Vector3.ProjectOnPlane(cameraMain.transform.forward, Vector3.up).normalized;
                var right = Vector3.ProjectOnPlane(cameraMain.transform.right, Vector3.up).normalized;
                
                move = right * move.x + forward * move.z;

                movement.Move(move);
            }
        }
    }
}