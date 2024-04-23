using UnityEngine;

namespace Game.Core
{
    public interface IRaycastable
    {
        public CursorType GetCursor();
        public bool HandleRaycast(GameObject context);
    }
}
