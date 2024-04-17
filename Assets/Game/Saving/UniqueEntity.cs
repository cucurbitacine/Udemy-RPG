using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Saving
{
    [DisallowMultipleComponent]
    public sealed class UniqueEntity : MonoBehaviour
    {
        [SerializeField] private string _guid;

        private static readonly Dictionary<string, UniqueEntity> Guids = new Dictionary<string, UniqueEntity>();
        
        private static bool IsUnique(string key, UniqueEntity entity)
        {
            Guids.TryAdd(key, entity);
            
            if (Guids[key] == null)
            {
                Guids[key] = entity;
            }
            
            return Guids[key] == entity;
        }
        
        public string GetGuid()
        {
            UpdateGuid();

            return _guid;
        }

        public void SetGuid(string guid)
        {
            _guid = guid;
        }
        
        private void UpdateGuid()
        {
#if UNITY_EDITOR
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrWhiteSpace(gameObject.scene.path)) return;

            var serializableObject = new UnityEditor.SerializedObject(this);
            var serializableProperty = serializableObject.FindProperty(nameof(_guid));
            var guid = serializableProperty.stringValue;

            if (string.IsNullOrWhiteSpace(guid) || !IsUnique(guid, this))
            {
                serializableProperty.stringValue = Guid.NewGuid().ToString();
                serializableObject.ApplyModifiedProperties();
            }
#endif
        }
        
        private void OnValidate()
        {
            UpdateGuid();
        }
    }
}