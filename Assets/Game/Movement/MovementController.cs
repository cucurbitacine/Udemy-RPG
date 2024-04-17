using Game.Core;
using Game.Saving;
using Game.Saving.Dto;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Health))]
    public class MovementController : MonoBehaviour, IActor, ISaveable
    {
        public NavMeshAgent agent { get; private set; }
        public Health health { get; private set; }
        
        #region Public API

        public Vector3 position => transform.position;
        
        public void Move(Vector3 offset)
        {
            if (agent)
            {
                agent.ResetPath();

                offset = Vector3.ClampMagnitude(offset, 1f);
                
                agent.velocity = offset * agent.speed;
            }
        }
        
        public void MoveAt(Vector3 point)
        {
            if (agent)
            {
                agent.SetDestination(point);
            }
        }

        public void Stop()
        {
            if (agent)
            {
                if (agent.hasPath)
                {
                    agent.ResetPath();
                }
            }
        }

        public void LookAt(Transform target)
        {
            if (agent)
            {
                agent.transform.LookAt(target, Vector3.up);
            }
        }
        
        public void Cancel()
        {
            Stop();
        }
        
        #endregion
        
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
            health.onDied.AddListener(() => agent.enabled = false);
        }
        
        private void OnDrawGizmosSelected()
        {
            if (agent && agent.hasPath)
            {
                Gizmos.DrawLineStrip(agent.path.corners, false);
            }
        }

        public object CaptureState()
        {
            return new SerializableVector3(agent.transform.position);
        }

        public void RestoreState(object state)
        {
            if (state is SerializableVector3 serializableVector3)
            {
                agent.Warp(serializableVector3.vector3);
            }
        }
    }
}