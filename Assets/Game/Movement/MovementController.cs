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
    [RequireComponent(typeof(ActionSchedule))]
    public class MovementController : MonoBehaviour, IActor, ISaveable
    {
        public NavMeshAgent agent { get; private set; }
        public Health health { get; private set; }
        public ActionSchedule schedule { get; private set; }
        
        #region Public API

        public Vector3 position => transform.position;

        public void ScheduledMove(Vector3 offset)
        {
            schedule.Run(this);
            
            Move(offset);
        }
        
        public void ScheduledMoveAt(Vector3 point)
        {
            schedule.Run(this);
            
            MoveAt(point);
        }
        
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
                var path = new NavMeshPath();

                if (agent.CalculatePath(point, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    agent.SetDestination(point);
                }
            }
        }
        
        public void Stop()
        {
            if (agent)
            {
                agent.ResetPath();
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
        
        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
            schedule = GetComponent<ActionSchedule>();
            
            health.onDied.AddListener(() => agent.enabled = false);
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