using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Characters
{
    public class Movement : MonoBehaviour
    {
        public Vector3 destination;

        [Space] public NavMeshAgent agent;

        public Vector3 position => agent ? agent.transform.position : transform.position;
        
        #region Public API

        public void SetDestination(Vector3 point)
        {
            destination = point;
        }

        public void Move()
        {
            if (agent)
            {
                agent.isStopped = false;
            }
        }

        public void Stop()
        {
            if (agent)
            {
                agent.isStopped = true;
            }
        }

        public void MoveAt(Vector3 point)
        {
            SetDestination(point);

            Move();
        }

        #endregion
        
        private void ProcessAgent()
        {
            if (agent && !agent.isStopped)
            {
                agent.SetDestination(destination);
            }
        }

        private void Awake()
        {
            if (agent == null) agent = GetComponent<NavMeshAgent>();

            destination = position;
        }

        private void Update()
        {
            ProcessAgent();
        }
    }
}