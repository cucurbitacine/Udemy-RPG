using System.Linq;
using Game.Saving;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Stats
{
    public class BaseStats : MonoBehaviour, ISaveable
    {
        public int currentLevel;
        
        [Space]
        [Min(1)]
        [SerializeField] private int _startingLevel = 1;
        public ClassType classType = ClassType.Unknown;
        public bool useModifier = false;
        
        [Space]
        public Progression progression;
        public GameObject levelUpEffectPrefab;
        
        private XP _xp;
        
        public UnityEvent<int> onLevelChanged { get; } = new UnityEvent<int>();
        
        public float GetStat(StatsType statType, int level)
        {
            if (progression)
            {
                return (GetBaseStat(statType, level) + GetModifier(statType)) * (1 + GetPercentage(statType) * 0.01f);
            }
            
            return Progression.DefaultValue;
        }

        public float GetStat(StatsType statType)
        {
            return GetStat(statType, _startingLevel);
        }
        
        public int GetLength(StatsType statType)
        {
            return progression ? progression.GetLength(this, statType) : 0;
        }
        
        public object CaptureState()
        {
            return currentLevel;
        }

        public void RestoreState(object state)
        {
            if (state is int intValue)
            {
                currentLevel = intValue;
            }
        }
        
        public int GetLevel()
        {
            return currentLevel;
        }

        private float GetBaseStat(StatsType statType, int level)
        {
            return progression.Get(classType, statType, level);
        }
        
        private float GetModifier(StatsType statsType)
        {
            return useModifier ? GetComponentsInChildren<IModifier>().Sum(m => m.GetModifier(statsType).Sum()) : 0f;
        }
        
        private float GetPercentage(StatsType statType)
        {
            return useModifier ? GetComponentsInChildren<IModifier>().Sum(m => m.GetPercentage(statType).Sum()) : 0f;
        }
        
        private int EvaluateLevel(float xp)
        {
            var maxLevel = GetLength(StatsType.LevelXP);
            for (var level = 1; level <= maxLevel; level++)
            {
                var requiredXP = GetStat(StatsType.LevelXP, level);
                if (xp < requiredXP)
                {
                    return Mathf.Max(_startingLevel, level);
                }
            }

            return Mathf.Max(_startingLevel, maxLevel);
        }
        
        private void UpdateLevel(float xp)
        {
            var level = EvaluateLevel(xp);
            
            if (currentLevel != level)
            {
                currentLevel = level;
                
                onLevelChanged.Invoke(currentLevel);

                LevelUpEffect();
            }
        }

        private void LevelUpEffect()
        {
            if (levelUpEffectPrefab)
            {
                var levelUp = Instantiate(levelUpEffectPrefab, transform);
                
                if (levelUp.TryGetComponent<ParticleSystem>(out var effect))
                {
                    if (!effect.main.playOnAwake)
                    {
                        effect.Play();
                    }
                }
            }
        }
        
        private void OnEnable()
        {
            if (TryGetComponent(out _xp))
            {
                _xp.onChanged.AddListener(UpdateLevel);
                
                UpdateLevel(_xp.points);
            }
        }

        private void OnDisable()
        {
            if (_xp)
            {
                _xp.onChanged.RemoveListener(UpdateLevel);
            }
        }
    }
}