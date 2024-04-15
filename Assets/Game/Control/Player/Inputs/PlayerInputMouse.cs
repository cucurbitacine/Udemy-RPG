using UnityEngine;

namespace Game.Control.Player.Inputs
{
    public class PlayerInputMouse : PlayerInput
    {
        public float maxDistance = 100f;
        public LayerMask layerMask = 1;

        public override bool Process(PlayerController player)
        {
            if (Cursor.visible && Input.GetAxisRaw("Fire1") > 0f)
            {
                var ray = cameraMain.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit, maxDistance, layerMask))
                {
                    player.schedule.Run(player.movement, movement => movement.MoveAt(hit.point));
                    
                    return true;
                }
            }

            if (Input.GetAxisRaw("Fire3") > 0f)
            {
                player.schedule.Run(player.movement, movement => movement.Stop());
            }
            
            return false;
        }
    }
}