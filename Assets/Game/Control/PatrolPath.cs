using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Control
{
    public class PatrolPath : MonoBehaviour
    {
        public Color beginColor = Color.white;
        public Color endColor = Color.cyan;

        [Space]
        public bool autoBuild = false;
        
        [Space]
        public List<Transform> points = new List<Transform>();

        public void Build()
        {
            points.Clear();
            points.AddRange(GetComponentsInChildren<Transform>().Where(t => t != transform));
        }

        private void Awake()
        {
            Build();
        }

        private void OnDrawGizmos()
        {
            if (autoBuild) Build();
            
            if (points.Count > 0)
            {
                for (var i = 0; i < points.Count; i++)
                {
                    var t = points.Count > 1 ? (float)i / (points.Count - 1) : 0f;
                    Gizmos.color = Color.Lerp(beginColor, endColor, t);
                    
                    var point = points[i];
                    if (point)
                    {
                        Gizmos.DrawWireSphere(point.position, 0.3f);
                        
                        var nextPoint = points[(i + 1) % points.Count];
                        if (nextPoint && point != nextPoint)
                        {
                            Gizmos.DrawLine(point.position, nextPoint.position);
                        }
                    }
                }
            }
        }
    }
}