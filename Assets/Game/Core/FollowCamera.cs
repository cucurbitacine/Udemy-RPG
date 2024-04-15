using UnityEngine;

namespace Game.Core
{
    public class FollowCamera : MonoBehaviour
    {
        #region SerializeField

        public bool paused = false;
        
        [Header("Camera")]
        public Transform satellite = null;
        
        [Space]
        public float height = 10f;
        [Min(0.001f)]
        public float radius = 5f;
        public float yaw = 0f;
        
        [Header("Dumping")]
        [Min(0)] public float followDamping = 16;
        [Min(0)] public float lookDamping = 0f;

        [Header("Physics")]
        public bool avoidObstacles = false;
        [Min(0f)] public float cameraSize = 0.4f;
        public LayerMask obstaclesLayer = 1;
        [Min(0f)] public float maxAvoidDistance = 10f;
        
        [Header("Follow")]
        public Transform followAt = null;
        public Transform lookAt = null;
        
        [Space]
        public Vector3 localOffset = Vector3.zero;
        public Vector3 axis = Vector3.up;

#if UNITY_EDITOR
        [Header("Information")]
        [SerializeField] private float _distance; 
        [SerializeField] private float _pitch; 
#endif
            
        #endregion

        #region Public API

        public Vector3 offset => followAt ? followAt.TransformVector(localOffset) : localOffset;
        public Vector3 followAtPoint => (followAt ? followAt.position : Vector3.zero) + offset;
        public Vector3 lookAtPoint => lookAt ? lookAt.position : followAtPoint;
        
        public float distance
        {
            get => Mathf.Sqrt(height * height + radius * radius);
            set
            {
                if (value > 0f)
                {
                    if (radius > 0f)
                    {
                        var k = height / radius;
                        radius = value / Mathf.Sqrt(1 + k * k);
                        height = k * radius;
                    }
                    else
                    {
                        radius = 0f;
                        height = value;
                    }
                }
            }
        }
        
        public float pitch
        {
            get => Vector3.Angle(axis, EvaluateCameraPosition() - followAtPoint);
            set
            {
                if (0 < value && value < 180)
                {
                    var k = 1f / Mathf.Tan(value * Mathf.Deg2Rad);
                    radius = distance / Mathf.Sqrt(1 + k * k);
                    height = k * radius;
                }
            }
        }

        #endregion

        #region Private API

        private void InitCamera()
        {
            if (!satellite)
            {
                satellite = Camera.main ? Camera.main.transform : transform;
            }

            if (satellite)
            {
                UpdateCamera(0f);
            }
        }
        
        private Vector3 EvaluateCameraPosition()
        {
            var direction = Quaternion.AngleAxis(yaw, axis) * Vector3.forward;
            var arm = direction * radius + axis * height;
            var position = followAtPoint + arm;

            if (avoidObstacles)
            {
                var rayDistance = arm.magnitude;
                
                if (rayDistance < maxAvoidDistance)
                {
                    var cameraRadius = cameraSize * 0.5f;
                    var rayDirection = arm.normalized;
                
                    var ray = new Ray(followAtPoint, rayDirection);
                    if (Physics.SphereCast(ray, cameraRadius, out var hit, rayDistance, obstaclesLayer))
                    {
                        position = hit.point + hit.normal * cameraRadius;
                    }
                }
            }
            
            return position;
        }
        
        private Quaternion EvaluateCameraRotation(Vector3 cameraPosition)
        {
            return Quaternion.LookRotation(lookAtPoint - cameraPosition, axis);
        }
        
        private void UpdateCamera(float deltaTime)
        {
            if (paused) return;

            var pos = EvaluateCameraPosition();

            if (satellite)
            {
                if (followDamping > 0f && deltaTime > 0f)
                {
                    pos = Vector3.Lerp(satellite.position, pos, followDamping * deltaTime);
                }

                satellite.position = pos;

                var rot = EvaluateCameraRotation(pos);
                
                if (lookDamping > 0f && deltaTime > 0f)
                {
                    rot = Quaternion.Lerp(satellite.rotation, rot, lookDamping * deltaTime);
                }
                
                satellite.rotation = rot;
            }
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            InitCamera();
        }

        private void LateUpdate()
        {
            UpdateCamera(Time.deltaTime);

#if UNITY_EDITOR
            _distance = distance;
            _pitch = pitch;
#endif
        }

        private void OnValidate()
        {
            InitCamera();
        }

        private void OnDrawGizmosSelected()
        {
            var cameraPoint = EvaluateCameraPosition();
            var pointUnderCameraOnSurface = followAtPoint + Vector3.ProjectOnPlane(cameraPoint - followAtPoint, axis);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(followAtPoint, cameraSize * 0.5f);
            Gizmos.DrawLine(followAtPoint, pointUnderCameraOnSurface);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(cameraPoint, cameraSize * 0.5f);
            Gizmos.DrawLine(pointUnderCameraOnSurface, cameraPoint);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(lookAtPoint, cameraSize * 0.5f);
            Gizmos.DrawLine(cameraPoint, lookAtPoint);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(cameraPoint, cameraPoint + EvaluateCameraRotation(cameraPoint) * Vector3.up);
            Gizmos.DrawLine(followAtPoint, followAtPoint + axis);
        }

        #endregion
    }
}
