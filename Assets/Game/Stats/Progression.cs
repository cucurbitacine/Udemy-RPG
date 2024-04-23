using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Stats
{
    [CreateAssetMenu(menuName = "RPG/Create Progression", fileName = "Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private List<ClassProgression> progressions = new List<ClassProgression>();

        public const float DefaultValue = 0f;

        private Dictionary<ClassType, ClassProgression> dict = null;

        public float Get(ClassType classType, StatsType statsType, int level)
        {
            BuildDict();

            if (dict != null && dict.TryGetValue(classType, out var classProgression))
            {
                return classProgression.Get(statsType, level);
            }

            return DefaultValue;
        }

        public int GetLength(ClassType classType, StatsType statType)
        {
            BuildDict();

            if (dict != null && dict.TryGetValue(classType, out var progression))
            {
                return progression.GetLength(statType);
            }

            return 0;
        }

        public float Get(BaseStats stats, StatsType statsType)
        {
            return Get(stats.classType, statsType, stats.GetLevel());
        }
        
        public int GetLength(BaseStats stats, StatsType statsType)
        {
            return GetLength(stats.classType, statsType);
        }
        
        private void BuildDict()
        {
            if (dict == null)
            {
                dict = new Dictionary<ClassType, ClassProgression>();

                foreach (var progression in progressions)
                {
                    dict[progression.classType] = progression;
                }
            }
        }

        private void OnValidate()
        {
            if (progressions != null)
            {
                foreach (var progression in progressions)
                {
                    progression.UpdateDisplayName();
                }
            }
        }

        [Serializable]
        private class ClassProgression
        {
            [HideInInspector] [SerializeField] private string _displayName;

            public ClassType classType = ClassType.Unknown;
            public StatProgression[] stats = null;

            private Dictionary<StatsType, StatProgression> dict = null;

            public float Get(StatsType statType, int level)
            {
                BuildDict();

                if (dict != null && dict.TryGetValue(statType, out var stat))
                {
                    return stat.Get(level);
                }

                return DefaultValue;
            }

            public int GetLength(StatsType statType)
            {
                BuildDict();
                
                if (dict != null && dict.TryGetValue(statType, out var stat))
                {
                    return stat.GetLength();
                }

                return 0;
            }
            
            public void UpdateDisplayName()
            {
                _displayName = $"{classType}";
                if (stats != null && stats.Length > 0)
                {
                    var statsDisplay = string.Join(", ", stats.Select(s => $"{s.statType}"));
                    _displayName = $"{_displayName} / {statsDisplay}";

                    foreach (var stat in stats)
                    {
                        stat.UpdateDisplayName();
                    }
                }
            }

            private void BuildDict()
            {
                if (dict == null)
                {
                    dict = new Dictionary<StatsType, StatProgression>();

                    if (stats != null)
                    {
                        foreach (var statProgression in stats)
                        {
                            dict[statProgression.statType] = statProgression;
                        }
                    }
                }
            }
        }

        [Serializable]
        private class StatProgression
        {
            [HideInInspector] [SerializeField] private string _displayName;

            public StatsType statType = StatsType.Unknown;
            public float[] levels = null;

            public float Get(int level)
            {
                if (levels == null) return DefaultValue;
                if (levels.Length == 0) return DefaultValue;

                return levels[Mathf.Clamp(level - 1, 0, levels.Length - 1)];
            }

            public int GetLength()
            {
                return levels?.Length ?? 0;
            }
            
            public void UpdateDisplayName()
            {
                _displayName = $"{statType}";

                if (levels != null)
                {
                    _displayName = $"{_displayName} ({levels.Length})";
                }
            }
        }
    }
}