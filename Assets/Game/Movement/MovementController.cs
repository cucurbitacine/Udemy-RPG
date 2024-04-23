using System.Collections.Generic;
using Game.Attributes;
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

                var velocity = agent.velocity + offset * (agent.acceleration * Time.deltaTime);
                velocity = Vector3.ClampMagnitude(velocity, agent.speed);
                
                agent.velocity = velocity;
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
            var data = new Dictionary<string, object>();
            data.Add(positionName, new SerializableVector3(agent.transform.position));
            data.Add(eulerAnglesName, new SerializableVector3(agent.transform.eulerAngles));
            
            return data;
        }

        public void RestoreState(object state)
        {
            if (state is Dictionary<string, object> data)
            {
                if (data.TryGetValue(positionName, out var serializablePosition) &&
                    serializablePosition is SerializableVector3 serializableVector3)
                {
                    agent.Warp(serializableVector3.vector3);
                }
                
                if (data.TryGetValue(eulerAnglesName, out var serializableRotation) &&
                    serializableRotation is SerializableVector3 serializableAngles)
                {
                    agent.transform.eulerAngles = serializableAngles.vector3;
                }
            }
        }

        private string positionName => nameof(transform.position);
        private string eulerAnglesName => nameof(transform.eulerAngles);
    }
}