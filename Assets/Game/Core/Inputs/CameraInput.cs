using UnityEngine;

namespace Game.Core.Inputs
{
    public class CameraInput : MonoBehaviour
    {
        [Min(0f)]
        public float zoomSpeed = 0.5f;
        public float rotateSpeed = 4f;
        
        [Space]
        public FollowCamera follow;

        private Vector2 _rotate;
        private float _lastRotate;
        
        private void ProcessZoom()
        {
            follow.distance += -Input.mouseScrollDelta.y * zoomSpeed;
        }
        
        private void ProcessRotate()
        {
            var rotate = Input.GetAxisRaw("Fire2");
            
            if (_lastRotate < rotate)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if (0f < rotate)
            {
                _rotate.x = Input.GetAxis("Mouse X");
                _rotate.y = Input.GetAxis("Mouse Y");
                
                follow.yaw += _rotate.x  * rotateSpeed;
                follow.pitch += _rotate.y * rotateSpeed;
            }
            else if (rotate < _lastRotate)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            _lastRotate = rotate;
        }
        
        private void ProcessInput()
        {
            ProcessZoom();

            ProcessRotate();
        }

        private void Awake()
        {
            if (follow == null) follow = GetComponent<FollowCamera>();
        }

        private void Update()
        {
            if (follow) ProcessInput();
        }
    }
}