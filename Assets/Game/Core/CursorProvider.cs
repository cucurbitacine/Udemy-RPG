using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(menuName = "RPG/Create CursorProvider", fileName = "CursorProvider", order = 0)]
    public class CursorProvider : ScriptableObject
    {
        public List<CursorMapping> cursors = new List<CursorMapping>();

        public CursorType currentCursor { get; private set; }
        
        public void SetCursor(CursorMapping cursor)
        {
            Cursor.SetCursor(cursor.texture, cursor.hotspot, CursorMode.Auto);
        }
        
        public void SetCursor(CursorType cursorType)
        {
            if (currentCursor != cursorType)
            {
                var cursor = GetCursor(cursorType);
                
                SetCursor(cursor);

                currentCursor = cursorType;
            }
        }
        
        public CursorMapping GetCursor(CursorType cursorType)
        {
            foreach (var cursor in cursors)
            {
                if (cursor.type == cursorType)
                {
                    return cursor;
                }
            }
            
            return cursors[0];
        }
    }
    
    [Serializable]
    public struct CursorMapping
    {
        public CursorType type;
        [Space]
        public Texture2D texture;
        public Vector2 hotspot;
    }
        
    public enum CursorType
    {
        Unknown,
        
        None,
        Movement,
        MovementPressed,
        
        Combat,
        Pickup,
        
        UI,
    }
}