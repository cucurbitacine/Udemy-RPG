using Game.Core;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Movement
{
    public class MovementController : MonoBehaviour, IActor
    {
        public NavMeshAgent agent { get; private set; }
        
        #region Public API

        public Vector3 position => transform.position;
        
        public void Move(Vector3 offset)
        {
            if (agent)
            {
                if (agent.hasPath)
                {
                    agent.ResetPath();
                }
                
                agent.Move(offset * Time.deltaTime);
                
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
        }

        private void OnDrawGizmosSelected()
        {
            if (agent && agent.hasPath)
            {
                Gizmos.DrawLineStrip(agent.path.corners, false);
            }
        }
    }
}