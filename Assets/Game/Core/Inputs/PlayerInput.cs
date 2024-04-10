using Game.Characters;
using UnityEngine;

namespace Game.Core.Inputs
{
    public abstract class PlayerInput : MonoBehaviour
    {
        public abstract void ProcessMovement(Movement movement);
    }
}