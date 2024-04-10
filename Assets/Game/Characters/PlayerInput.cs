using UnityEngine;

namespace Game.Characters
{
    public class PlayerInput : MonoBehaviour
    {
        public float maxDistance = 100f;
        public LayerMask layerMask = 1;

        [Space] public Movement movement;
        
        public Camera cam => Camera.main;
        
        private void ProcessInput()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit, maxDistance, layerMask))
                {
                    movement.MoveAt(hit.point);
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                movement.Stop();
            }
        }

        private void Awake()
        {
            if (movement == null) movement = GetComponent<Movement>();
        }

        private void LateUpdate()
        {
            if (movement)
            {
                ProcessInput();
            }
        }
    }
}