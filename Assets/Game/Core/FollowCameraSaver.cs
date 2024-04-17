using Game.Saving;
using UnityEngine;

namespace Game.Core
{
    [RequireComponent(typeof(FollowCamera))]
    public class FollowCameraSaver : MonoBehaviour, ISaveable
    {
        public FollowCamera follow => _follow ? _follow : (_follow = GetComponent<FollowCamera>());
        
        private FollowCamera _follow;
        
        public object CaptureState()
        {
            return new float[] { follow.height, follow.radius, follow.yaw };
        }

        public void RestoreState(object state)
        {
            if (state is float[] array)
            {
                if (array.Length == 3)
                {
                    follow.height = array[0];
                    follow.radius = array[1];
                    follow.yaw = array[2];
                }
            }
        }
    }
}