using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Game.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        public string portalName = string.Empty;
        public Transform spawnPoint;
        
        [Space]
        public string sceneNameTarget = string.Empty;
        public string portalNameTarget = string.Empty;

        public Fader fader => Fader.Singleton;
        public SavingWrapper saving => SavingWrapper.Singleton;
        
        private Coroutine _loading = null;
        
        private IEnumerator LoadingScene()
        {
            if (fader)
            {
                yield return fader.FadeIn();
            }

            if (saving)
            {
                saving.Save();
            }
            
            var loading = SceneManager.LoadSceneAsync(sceneNameTarget, LoadSceneMode.Single);

            if (loading != null)
            {
                loading.completed += SceneLoaded;
            }
        }
        
        private void LoadScene()
        {
            if (_loading != null)
            {
                Debug.LogError("Have already been loading");
                return;
            }
            
            if (string.IsNullOrWhiteSpace(sceneNameTarget))
            {
                Debug.LogError("Have no target scene");
                return;
            }

            _loading = StartCoroutine(LoadingScene());
        }
        
        private void SceneLoaded(AsyncOperation ao)
        {
            if (string.IsNullOrWhiteSpace(portalNameTarget))
            {
                Debug.LogWarning("Have no target portal");
                return;
            }

            var portals = FindObjectsByType<Portal>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            var portal = portals.FirstOrDefault(p => p != this && p.portalName == portalNameTarget);
            if (portal == null)
            {
                Debug.LogWarning("Have not found any valid portal");
                return;
            }
            
            if (portal.fader)
            {
                portal.fader.FadeOut();
            }

            if (portal.saving)
            {
                portal.saving.Load();
            }
           
            var player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("Have not found any player");
                return;
            }
            
            var agent = player.GetComponentInChildren<NavMeshAgent>();
            
            if (agent && portal.spawnPoint)
            {
                var position = portal.spawnPoint.position;
                var rotation = portal.spawnPoint.rotation;

                agent.Warp(position);
                agent.transform.rotation = rotation;
            }

            if (portal.saving)
            {
                portal.saving.Save();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                LoadScene();
            }
        }

        private void OnValidate()
        {
            name = $"Portal \"{portalName}\" -> {sceneNameTarget} / {portalNameTarget}";
        }
    }
}