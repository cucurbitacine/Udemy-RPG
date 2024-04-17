using System;
using UnityEngine;

namespace Game.Saving.Dto
{
    [Serializable]
    public struct SerializableVector3
    {
        public float x, y, z;

        public Vector3 vector3 => new Vector3(x, y, z);
        
        public SerializableVector3(Vector3 vector3)
        {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
        }
    }
}