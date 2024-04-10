using UnityEngine;

namespace Game.Control.Inputs
{
    public class PlayerInputMouse : PlayerInput
    {
        public float maxDistance = 100f;
        public LayerMask layerMask = 1;

        public override bool Process(PlayerController player)
        {
            if (Input.GetAxisRaw("Fire1") > 0f)
            {
                var ray = cameraMain.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit, maxDistance, layerMask))
                {
                    player.schedule.StartAction(player.movement, movement => movement.MoveAt(hit.point));
                    
                    return true;
                }
            }

            if (Input.GetAxisRaw("Fire3") > 0f)
            {
                player.schedule.StartAction(player.movement, movement => movement.Stop());
            }
            
            return false;
        }
    }
}