using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class FPS : MonoBehaviour
    {
        public float fps;
        public float fpsSmooth;
        
        private int _index = 0; 
        private readonly float[] _history = new float[HistorySize];

        private const int HistorySize = 1024;
        
        private void Update()
        {
            fps = 1f / Time.deltaTime;

            _history[_index % _history.Length] = fps;
            _index++;

            fpsSmooth = _history.Sum() / HistorySize;
        }

        private void OnGUI()
        {
            GUILayout.Box($"{fps:F0} / FPS");
            GUILayout.Box($"{fpsSmooth:F1} / FPS Smooth");
        }
    }
}
