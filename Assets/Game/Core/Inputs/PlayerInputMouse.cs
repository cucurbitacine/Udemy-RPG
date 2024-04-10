using Game.Characters;
using UnityEngine;

namespace Game.Core.Inputs
{
    public class PlayerInputMouse : PlayerInput
    {
        public float maxDistance = 100f;
        public LayerMask layerMask = 1;

        public Camera cameraMain => Camera.main;
        
        public override void ProcessMovement(Movement movement)
        {
            if (Input.GetAxisRaw("Fire1") > 0f)
            {
                var ray = cameraMain.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit, maxDistance, layerMask))
                {
                    movement.MoveAt(hit.point);
                }
            }

            if (Input.GetAxisRaw("Fire3") > 0f)
            {
                movement.Stop();
            }
        }
    }
}