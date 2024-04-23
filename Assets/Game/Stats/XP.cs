using Game.Saving;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Stats
{
    public class XP : MonoBehaviour, ISaveable
    {
        [Min(0f)] public float points = 0;

        public UnityEvent<float> onChanged { get; } = new UnityEvent<float>();
        
        public void Gain(float xp)
        {
            if (xp > 0)
            {
                points += xp;
                
                onChanged.Invoke(points);
            }
        }

        public object CaptureState()
        {
            return points;
        }

        public void RestoreState(object state)
        {
            if (state is float floatValue)
            {
                points = floatValue;
            }
        }
    }
}