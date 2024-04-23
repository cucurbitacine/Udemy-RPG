using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public string folder = "Saves";
        public string extension = ".sav";
        
        private const string SceneIndexKey = nameof(SceneIndexKey);

        public IEnumerator LoadScene(string saveFile)
        {
            var state = LoadFile(saveFile);

            yield return RestoreScene(state);
            
            RestoreState(state);
            
            Debug.Log($"Load File <- {GetPathToSaveFile(saveFile)}");
        }
        
        public void Save(string saveFile)
        {
            var state = LoadFile(saveFile);
            
            CaptureState(state);
            
            SaveFile(saveFile, state);
            
            Debug.Log($"Save File -> {GetPathToSaveFile(saveFile)}");
        }
        
        public void Load(string saveFile)
        {
            var state = LoadFile(saveFile);
            
            RestoreState(state);
            
            Debug.Log($"Load File <- {GetPathToSaveFile(saveFile)}");
        }

        public void Delete(string saveFile)
        {
            var path = GetPathToSaveFile(saveFile);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        
        private void SaveFile(string saveFile, Dictionary<string, object> state)
        {
            var pathToFolder = GetPathToSaveFolder();

            if (!Directory.Exists(pathToFolder))
            {
                Directory.CreateDirectory(pathToFolder);
            }
            
            var path = GetPathToSaveFile(saveFile);

            using var stream = File.Open(path, FileMode.Create);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
        
        private Dictionary<string, object> LoadFile(string saveFile)
        {
            var path = GetPathToSaveFile(saveFile);

            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            
            using var stream = File.Open(path, FileMode.Open);
            var formatter = new BinaryFormatter();

            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
        
        private string GetPathToSaveFolder()
        {
            return Path.Combine(Application.persistentDataPath, folder);
        }
        
        private string GetPathToSaveFile(string saveFile)
        {
            return Path.Combine(GetPathToSaveFolder(), $"{saveFile}{extension}");
        }
        
        private static void CaptureState(IDictionary<string, object> state)
        {
            var entities = FindObjectsByType<SaveableEntity>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

            foreach (var saveable in entities)
            {
                state[saveable.entity.GetGuid()] = saveable.CaptureState();
            }

            CaptureScene(state);
        }

        private static void RestoreState(IReadOnlyDictionary<string, object> state)
        {
            var entities = FindObjectsByType<SaveableEntity>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

            foreach (var saveable in entities)
            {
                if (state.TryGetValue(saveable.entity.GetGuid(), out var entityState))
                {
                    saveable.RestoreState(entityState);
                }
            }
        }
        
        private static void CaptureScene(IDictionary<string, object> state)
        {
            state[SceneIndexKey] = SceneManager.GetActiveScene().buildIndex;
        }

        private static IEnumerator RestoreScene(IReadOnlyDictionary<string, object> state)
        {
            if (state.TryGetValue(SceneIndexKey, out var sceneState) && sceneState is int buildIndex)
            {
                if (SceneManager.GetActiveScene().buildIndex != buildIndex)
                {
                    Debug.Log($"Loading...");
                    yield return SceneManager.LoadSceneAsync(buildIndex);
                }
            }
        }
    }
}