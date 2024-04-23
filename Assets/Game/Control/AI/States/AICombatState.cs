using Game.Combat;
using UnityEngine;

namespace Game.Control.AI.States
{
    public class AICombatState : AIState
    {
        public float chaseSpeed = 4;
        public float chaseDistance = 5f;

        [Space]
        public bool provocated;
        public float provocationTimer = 0f;
        public float provocationDuration = 5f;
        
        [Space]
        public CombatTarget player;
        
        public void Provocation()
        {
            if (provocated) return;
            
            provocated = true;

            provocationTimer = 0f;
        }

        public override bool Process(AIController ai)
        {
            ai.movement.agent.speed = chaseSpeed;

            if (player && (provocated || IsValidDistance(ai.fighter.transform, player.transform)))
            {
                if (ai.fighter.CanAttack(player))
                {
                    ai.fighter.Attack(player);

                    var clds = Physics.OverlapSphere(ai.movement.position, 10);

                    foreach (var cld in clds)
                    {
                        var combat = cld.GetComponentInChildren<AICombatState>();
                        if (combat)
                        {
                            combat.Provocation();
                        }
                    }
                    
                    return true;
                }
            }

            ai.fighter.ResetTarget();

            return false;
        }

        private bool IsValidDistance(Transform origin, Transform target)
        {
            return Vector3.Distance(origin.position, target.position) < chaseDistance;
        }

        private void Start()
        {
            player = GameObject.FindWithTag("Player").GetComponent<CombatTarget>();
        }

        private void Update()
        {
            if (provocated)
            {
                if (provocationTimer < provocationDuration)
                {
                    provocationTimer += Time.deltaTime;
                }
                else
                {
                    provocated = false;
                    provocationTimer = 0f;
                }
            }
        }
    }
}