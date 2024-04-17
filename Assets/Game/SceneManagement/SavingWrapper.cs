using System.Collections;
using Game.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.SceneManagement
{
    [RequireComponent(typeof(SavingSystem))]
    public class SavingWrapper : MonoBehaviour
    {
        public static SavingWrapper Singleton { get; private set; }
        
        public string saveKey = "save";

        public SavingSystem saving { get; private set; }

        public void Save()
        {
            saving.Save(GetSaveFile());
        }

        public void Load()
        {
            saving.Load(GetSaveFile());
        }

        private string GetSaveFile()
        {
#if UNITY_EDITOR
            return $"EDITOR_{saveKey}";
#endif
            
            return saveKey;
        }
        
        private void Awake()
        {
            saving = GetComponent<SavingSystem>();

            if (Singleton)
            {
                Destroy(gameObject);
            }
            else
            {
                Singleton = this;
            }
        }

        private IEnumerator Start()
        {
            if (Fader.Singleton)
            {
                Fader.Singleton.FadeIn(0f);
            }
            
            yield return saving.LoadScene(GetSaveFile());
            
            if (Fader.Singleton)
            {
                yield return Fader.Singleton.FadeOut();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Save();
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Application.isPlaying)
                {
                    Save();
                    
                    if (!Application.isEditor)
                    {
                        Application.Quit();
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                saving.Delete(GetSaveFile());

                SceneManager.LoadScene(0);
            }
        }
    }
}