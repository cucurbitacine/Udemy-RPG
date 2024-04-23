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

        [Space] public UnityEvent onDied = new UnityEvent();
        
        private static readonly int TriggerDeath = Animator.StringToHash("Death");

        public Animator animator { get; private set; }
        public ActionSchedule schedule { get; private set; }
        public BaseStats stats { get; private set; }
        
        public void Damage(GameObject source, float amount)
        {
            if (amount > 0)
            {
                points = Mathf.Max(0, points - amount);
            }

            if (points == 0f)
            {
                Die(source);
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

        public void Die(GameObject source)
        {
            if (isDied) return;

            Die();

            if (source && source.TryGetComponent<XP>(out var xp))
            {
                if (gameObject.TryGetComponent<BaseStats>(out var stats))
                {
                    var reward = stats.GetStat(StatsType.RewardXP);
                    xp.Gain(reward);
                }
            }
        }

        public void RestoreHealth()
        {
            points = total;
        }
        
        public object CaptureState()
        {
            return new float[] { points, total };
        }

        public void RestoreState(object state)
        {
            if (state is float[] array && array.Length == 2)
            {
                points = array[0];
                total = array[1];
                
                if (points == 0f)
                {
                    Die();
                }
            }
        }

        private void LevelChanged(int level)
        {
            UpdateTotalHealth(level);

            if (points > 0)
            {
                points = total;
            }
        }

        private void UpdateHealth()
        {
            points = Mathf.Clamp(points, 0, total);

            if (points == 0)
            {
                Die();
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

            UpdateHealth();
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