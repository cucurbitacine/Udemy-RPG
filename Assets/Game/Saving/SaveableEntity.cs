using System.Collections.Generic;
using UnityEngine;

namespace Game.Saving
{
    [RequireComponent(typeof(UniqueEntity))]
    public class SaveableEntity : MonoBehaviour
    {
        public UniqueEntity entity => _entity ? _entity : (_entity = GetComponent<UniqueEntity>());
        
        private UniqueEntity _entity = null; 

        public object CaptureState()
        {
            var state = new Dictionary<string, object>();
            
            foreach (var saveable in GetComponentsInChildren<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            
            return state;
        }

        public void RestoreState(object state)
        {
            if (state is Dictionary<string, object> stateDict)
            {
                foreach (var saveable in GetComponentsInChildren<ISaveable>())
                {
                    if (stateDict.TryGetValue(saveable.GetType().ToString(), out var entityState))
                    {
                        saveable.RestoreState(entityState);
                    }
                }
            }
        }
    }
}