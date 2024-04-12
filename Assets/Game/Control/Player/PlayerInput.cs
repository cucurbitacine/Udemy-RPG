using UnityEngine;

namespace Game.Control.Player
{
    public abstract class PlayerInput : MonoBehaviour
    {
        public Camera cameraMain => Camera.main;
        
        public abstract bool Process(PlayerController player);
    }
}