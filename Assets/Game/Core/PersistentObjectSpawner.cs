using UnityEngine;

namespace Game.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        public static bool spawned { get; private set; }

        public bool spawnOnAwake = true;
        
        [Space]
        public GameObject prefabPersistentObject;

        public void Spawn()
        {
            if (spawned) return;
            
            if (prefabPersistentObject)
            {
                var persistentObject = Instantiate(prefabPersistentObject);
                DontDestroyOnLoad(persistentObject);
                spawned = true;
            }
        }
        
        private void Awake()
        {
            if (spawnOnAwake) Spawn();
        }
    }
}