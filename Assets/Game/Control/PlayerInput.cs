using UnityEngine;

namespace Game.Control
{
    public abstract class PlayerInput : MonoBehaviour
    {
        public Camera cameraMain => Camera.main;
        
        public abstract bool Process(PlayerController player);
    }
}