using UnityEngine;
using UnityEngine.AI;

namespace Game.Characters
{
    public class Movement : MonoBehaviour
    {
        [Space] public NavMeshAgent agent;
        
        #region Public API

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
        
        #endregion
        
        private void Awake()
        {
            if (agent == null) agent = GetComponent<NavMeshAgent>();
        }
    }
}