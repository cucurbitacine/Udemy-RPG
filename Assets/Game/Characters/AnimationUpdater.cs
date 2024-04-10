using UnityEngine;

namespace Game.Characters
{
    public class AnimationUpdater : MonoBehaviour
    {
        public Movement movement;
        public Animator animator;
        
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void UpdateAnimator()
        {
            if (animator && movement && movement.agent)
            {
                var velocity = movement.agent.velocity;
                var localVelocity = movement.agent.transform.InverseTransformDirection(velocity);
                var speed = localVelocity.z;
                
                animator.SetFloat(Speed, speed) ;
            }
        }

        private void Update()
        {
            UpdateAnimator();
        }
    }
}
