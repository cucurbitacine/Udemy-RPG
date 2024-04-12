using Game.Combat;
using UnityEngine;

namespace Game.Control.Player.Inputs
{
    public class PlayerCombatInput : PlayerInput
    {
        public float distance = 100f;
        public LayerMask targetLayer = 1;
        
        [Space]
        public KeyCode attackKey = KeyCode.F;

        private float _lastFire;
        
        public override bool Process(PlayerController player)
        {
            if (Input.GetKeyDown(attackKey))
            {
                player.schedule.Run(player.fighter, f => f.Attack());
                
                return true;
            }

            var fire = Input.GetAxisRaw("Fire1");
            
            if (_lastFire < fire)
            {
                var ray = cameraMain.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit, distance, targetLayer))
                {
                    if (hit.transform.TryGetComponent<CombatTarget>(out var target))
                    {
                        if (player.fighter.CanAttack(target))
                        {
                            player.schedule.Run(player.fighter, f => f.Attack(target));
                            
                            return true;
                        }
                    }
                }
                
                player.fighter.ResetTarget();
            }

            _lastFire = fire;

            return false;
        }
    }
}