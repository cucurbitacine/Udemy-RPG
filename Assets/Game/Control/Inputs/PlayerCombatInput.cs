using Game.Combat;
using UnityEngine;

namespace Game.Control.Inputs
{
    public class PlayerCombatInput : PlayerInput
    {
        public float distance = 100f;
        public LayerMask targetLayer = 1;
        
        [Space]
        public KeyCode attackKey = KeyCode.F;

        public override bool Process(PlayerController player)
        {
            if (Input.GetKeyDown(attackKey))
            {
                player.schedule.StartAction(player.fighter, f => f.Attack());
                
                return true;
            }

            if (Input.GetAxisRaw("Fire1") > 0f)
            {
                var ray = cameraMain.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit, distance, targetLayer))
                {
                    if (hit.transform.TryGetComponent<CombatTarget>(out var target))
                    {
                        if (player.fighter.CanAttack(target) && target != player.target)
                        {
                            player.schedule.StartAction(player.fighter, f => f.Attack(target));
                            
                            return true;
                        }
                    }
                }
                
                player.fighter.ResetTarget();
            }

            return false;
        }
    }
}