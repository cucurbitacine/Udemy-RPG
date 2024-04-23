using Game.Core;
using Game.Saving;
using Game.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Attributes
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(ActionSchedule))]
    [RequireComponent(typeof(BaseStats))]
    public class Health : MonoBehaviour, ISaveable
    {
        public float points = 100f;
        public float total = 100f;
        
        [Min(0f)]
        public float restoringSpeed = 0f;
        
        [Space]
        public bool isDied = false;

        public UnityEvent<float> changed = new UnityEvent<float>();
        public UnityEvent damaged = new UnityEvent();
        public UnityEvent onDied  = new UnityEvent();
        
        private static readonly int TriggerDeath = Animator.StringToHash("Death");

        public Animator animator { get; private set; }
        public ActionSchedule schedule { get; private set; }
        public BaseStats stats { get; private set; }
        
        public void Damage(GameObject source, float amount)
        {
            if (!isDied && amount > 0)
            {
                var value = Mathf.Max(0, points - amount);
                
                SetValues(value, total);
                
                if (points == 0f)
                {
                    if (source && source.TryGetComponent<XP>(out var xp))
                    {
                        var reward = stats.GetStat(StatsType.RewardXP);
                        
                        xp.Gain(reward);
                    }
                }
                
                damaged.Invoke();
            }
        }

        public void Heal(float amount)
        {
            if (!isDied && amount > 0)
            {
                var value = Mathf.Min(points + amount, total);
                
                SetValues(value, total);
            }
        }
        
        public void Die()
        {
            if (isDied) return;
            
            isDied = true;
            animator.SetTrigger(TriggerDeath);
            schedule.CancelCurrentActor();
            
            onDied.Invoke();
        }
        
        public void RestoreHealth()
        {
            SetValues(total, total);
        }
        
        public object CaptureState()
        {
            return new float[] { points, total };
        }

        public void SetValues(float value, float max)
        {
            var delta = value - points;
            
            points = value;
            total = max;
            
            if (points == 0f)
            {
                Die();
            }

            if (Mathf.Abs(delta) > 0f)
            {
                changed.Invoke(delta);
            }
        }
        
        public void RestoreState(object state)
        {
            if (state is float[] array && array.Length == 2)
            {
                SetValues(array[0], array[1]);
            }
        }

        private void LevelChanged(int level)
        {
            UpdateTotalHealth(level);

            if (!isDied)
            {
                RestoreHealth();
            }
        }
        
        private void UpdateTotalHealth(int level)
        {
            total = stats.GetStat(StatsType.Health, level);
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            schedule = GetComponent<ActionSchedule>();
            stats = GetComponent<BaseStats>();
        }

        private void OnEnable()
        {
            stats.onLevelChanged.AddListener(LevelChanged);
            
            UpdateTotalHealth(stats.GetLevel());

            RestoreHealth();
        }
        
        private void OnDisable()
        {
            stats.onLevelChanged.RemoveListener(LevelChanged);
        }
        
        private void Update()
        {
            if (0 < points && points < total)
            {
                var deltaHealth = restoringSpeed * Time.deltaTime;
                if (deltaHealth > 0f)
                {
                    points = Mathf.Clamp(points + deltaHealth, 0, total);
                }
            }
        }
    }
}