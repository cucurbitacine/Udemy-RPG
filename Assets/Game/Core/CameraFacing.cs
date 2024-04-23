using UnityEngine;

namespace Game.Core
{
    public class CameraFacing : MonoBehaviour
    {
        public static Camera cameraMain => Camera.main;

        private void LateUpdate()
        {
            if (cameraMain)
            {
                //transform.LookAt(cameraMain.transform);
                transform.forward = cameraMain.transform.forward;
            }
        }
    }
}