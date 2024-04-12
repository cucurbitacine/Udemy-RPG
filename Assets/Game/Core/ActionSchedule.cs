using System;
using UnityEngine;

namespace Game.Core
{
    public class ActionSchedule : MonoBehaviour
    {
        public IActor currentActor { get; private set; }

        public bool Run<T>(T actor, Action<T> action) where T : IActor
        {
            return Run(actor, () => action.Invoke(actor));
        }
        
        public bool Run(IActor actor, Action action = null)
        {
            if (currentActor != null)
            {
                if (currentActor != actor)
                {
                    currentActor.Cancel();
                }
            }
            
            currentActor = actor;
            action?.Invoke();
            
            return true;
        }

        public void CancelCurrentActor()
        {
            Run(null);
        }
    }
    
    public interface IActor
    {
        public void Cancel();
    }
}