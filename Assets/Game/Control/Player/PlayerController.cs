using System;
using System.Linq;
using Game.Attributes;
using Game.Combat;
using Game.Core;
using Game.Movement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace Game.Control.Player
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject playerObject;
        public FollowCamera follow;

        [Header("Interaction")]
        public float interactionRaycastDistance = 100f;
        public LayerMask interactionLayers = 1;
        
        [Header("Movement")]
        public float movementRaycastDistance = 100f;
        public LayerMask movementLayers = 1;
        public float navMeshMaxProjection = 1f;
        public float navMeshMaxPath = 10f;
        
        [Header("Cursors")]
        public CursorProvider cursors;

        private NavMeshPath _path;
        
        private readonly RaycastHit[] _hitsRaycastable = new RaycastHit[32];
        
        public static Camera cameraMain => Camera.main;
        
        public MovementController movement { get; private set; }
        public Fighter fighter { get; private set; }
        public CombatTarget target { get; private set; }
        public ActionSchedule schedule { get; private set; }

        public Health health => target.health;

        private void SetCursor(CursorType cursorType)
        {
            if (cursors)
            {
                cursors.SetCursor(cursorType);
            }
        }

        private void Keyboard()
        {
            if (health.isDied) return;
            
            var move = Vector3.zero;
            move.x = Input.GetAxisRaw("Horizontal");
            move.z = Input.GetAxisRaw("Vertical");

            if (move != Vector3.zero)
            {
                var forward = Vector3.ProjectOnPlane(cameraMain.transform.forward, Vector3.up).normalized;
                var right = Vector3.ProjectOnPlane(cameraMain.transform.right, Vector3.up).normalized;

                move = right * move.x + forward * move.z;

                movement.ScheduledMove(move);
            }
        }
        
        private bool UI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }

            return false;
        }

        private bool IsDead()
        {
            if (health.isDied)
            {
                SetCursor(CursorType.None);
                
                return true;
            }

            return false;
        }
        
        private static Ray GetMouseRay()
        {
            return cameraMain.ScreenPointToRay(Input.mousePosition);
        }

        private static void SortByDistance(RaycastHit[] hits, int size)
        {
            size = Mathf.Min(hits.Length, size);
            
            var distances = new float[size];

            for (var i = 0; i < size; i++)
            {
                distances[i] = hits[i].distance;
            }
            
            Array.Sort(distances, hits, 0, size);
        }
        
        private bool Interaction()
        {
            var size = Physics.RaycastNonAlloc(GetMouseRay(), _hitsRaycastable, interactionRaycastDistance, interactionLayers, QueryTriggerInteraction.Collide);

            SortByDistance(_hitsRaycastable, size);
            
            foreach (var hit in _hitsRaycastable.Take(size))
            {
                var raycastables = hit.transform.GetComponents<IRaycastable>();
                
                foreach (var raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(playerObject))
                    {
                        SetCursor(raycastable.GetCursor());
                        return true;
                    }
                }
            }

            return false;
        }

        private static float GetPathLength(params Vector3[] points)
        {
            if (points == null) return 0f;
            if (points.Length < 2) return 0f;

            var length = 0f;
            for (var i = 0; i < points.Length - 1; i++)
            {
                length += Vector3.Distance(points[i], points[i + 1]);
            }

            return length;
        }

        private bool Raycast(out RaycastHit hit)
        {
            return Physics.Raycast(GetMouseRay(), out hit, movementRaycastDistance, movementLayers);
        }

        private bool Passing(Vector3 point, out NavMeshHit navHit)
        {
            if (NavMesh.SamplePosition(point, out navHit, navMeshMaxProjection, NavMesh.AllAreas))
            {
                var origin = playerObject.transform.position;

                if (_path == null) _path = new NavMeshPath();
                
                if (NavMesh.CalculatePath(origin, navHit.position, NavMesh.AllAreas, _path))
                {
                    if (_path.status == NavMeshPathStatus.PathComplete)
                    {
                        return GetPathLength(_path.corners) < navMeshMaxPath;
                    }
                }
            }

            return false;
        }
        
        private bool Navigation()
        {
            var wasHit = Raycast(out var hit);
            
            if (wasHit)
            {
                var isPassing = Passing(hit.point, out var navHit);
                
                if (isPassing)
                {
                    if (Input.GetMouseButton(0))
                    {
                        movement.ScheduledMoveAt(navHit.position);
                    
                        SetCursor(CursorType.MovementPressed);
                    }
                    else
                    {
                        SetCursor(CursorType.Movement);
                    }
                    
                    return true;
                }
            }

            return false;
        }

        private void Nothing()
        {
            SetCursor(CursorType.None);
        }
        
        private void Awake()
        {
            if (playerObject)
            {
                movement = playerObject.GetComponent<MovementController>();
                fighter = playerObject.GetComponent<Fighter>();
                target = playerObject.GetComponent<CombatTarget>();
                schedule = playerObject.GetComponent<ActionSchedule>();

                if (follow)
                {
                    follow.followAt = playerObject.transform;
                }
            }
        }

        private void Update()
        {
            Keyboard();

            if (!Cursor.visible) return;
            
            if (UI()) return;

            if (IsDead()) return;
            
            if (Interaction()) return;

            if (Navigation()) return;

            Nothing();
        }
    }
}