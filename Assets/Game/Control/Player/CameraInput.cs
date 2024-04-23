using Game.Core;
using UnityEngine;

namespace Game.Control.Player
{
    public class CameraInput : MonoBehaviour
    {
        [Min(0f)]
        public float zoomSpeed = 0.5f;
        public float rotateSpeed = 4f;

        [Space]
        public bool hideCursor = true;
        public CursorLockMode cursorLockBusy = CursorLockMode.Confined;
        public CursorLockMode cursorLockFree = CursorLockMode.None;
        
        private Vector2 _rotate;
        private float _lastRotate;
        
        public FollowCamera follow { get; private set; }
        
        private void ProcessZoom()
        {
            follow.distance += -Input.mouseScrollDelta.y * zoomSpeed;
        }
        
        private void ProcessRotate()
        {
            var rotate = Input.GetAxisRaw("Fire2");
            
            if (_lastRotate < rotate)
            {
                if (hideCursor)
                {
                    Cursor.visible = false;
                }
                Cursor.lockState = cursorLockBusy;
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
                if (hideCursor)
                {
                    Cursor.visible = true;
                }
                Cursor.lockState = cursorLockFree;
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
            follow = GetComponent<FollowCamera>();
        }

        private void OnDisable()
        {
            Cursor.visible = true;
        }

        private void Update()
        {
            if (follow) ProcessInput();
        }
    }
}