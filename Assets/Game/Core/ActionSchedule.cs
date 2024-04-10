using System;
using UnityEngine;

namespace Game.Core
{
    public class ActionSchedule : MonoBehaviour
    {
        public IActor currentActor { get; private set; }

        public void StartAction<T>(T actor, Action<T> action) where T : IActor
        {
            StartAction(actor, () => action.Invoke(actor));
        }
        
        public void StartAction(IActor actor, Action action)
        {
            if (currentActor != null)
            {
                if (currentActor != actor)
                {
                    currentActor.Cancel();
                }
            }
            
            currentActor = actor;
            action.Invoke();
        }
    }
    
    public interface IActor
    {
        public void Cancel();
    }
}