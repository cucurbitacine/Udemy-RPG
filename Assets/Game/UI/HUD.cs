using Game.Control.Player;
using Game.Stats;
using UnityEngine;

namespace Game.UI
{
    public class HUD : MonoBehaviour
    {
        public PlayerController player;

        [Space]
        public LevelDisplay playerLevel;
        public XPDisplay playerXP;
        public HealthDisplay playerHealth;
        public HealthDisplay enemyHealth;
        
        private XP _xp;
        private BaseStats _stats;
        
        private void UpdateHUD()
        {
            if (playerLevel)
            {
                playerLevel.stats = _stats;
            }
            
            if (playerHealth)
            {
                playerHealth.health = player ? player.health : null;
            }
            
            if (enemyHealth)
            {
                enemyHealth.health = player && player.fighter.target ? player.fighter.target.health : null;
            }

            if (playerXP)
            {
                playerXP.xp = _xp;
            }
        }
        
        private void Awake()
        {
            if (player == null)
            {
                var playerObject = GameObject.FindWithTag("Player");
                if (playerObject)
                {
                    player = playerObject.GetComponentInChildren<PlayerController>();
                }

                playerObject.TryGetComponent(out _xp);
                playerObject.TryGetComponent(out _stats);
            }
        }

        private void Update()
        {
            UpdateHUD();
        }
    }
}