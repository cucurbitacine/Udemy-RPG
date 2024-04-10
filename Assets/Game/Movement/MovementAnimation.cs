using UnityEngine;

namespace Game.Movement
{
    public class MovementAnimation : MonoBehaviour
    {
        public Animator animator { get; private set; }
        public MovementController MovementController { get; private set; }
        
        private static readonly int FloatSpeed = Animator.StringToHash("Speed");

        private void UpdateAnimator()
        {
            if (animator && MovementController && MovementController.agent)
            {
                var velocity = MovementController.agent.velocity;
                var localVelocity = MovementController.agent.transform.InverseTransformDirection(velocity);
                var speed = localVelocity.z;
                
                animator.SetFloat(FloatSpeed, speed) ;
            }
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            MovementController = GetComponent<MovementController>();
        }

        private void Update()
        {
            UpdateAnimator();
        }
    }
}
