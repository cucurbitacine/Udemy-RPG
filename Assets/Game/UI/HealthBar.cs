using System;
using Game.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class HealthBar : MonoBehaviour
    {
        public Health health;
        
        [Space]
        public Slider slider;

        private void UpdateHealth()
        {
            if (slider && health)
            {
                slider.value = health.points / health.total;
            }
        }

        private void UpdateHealth(float delta)
        {
            UpdateHealth();
        }

        private void Hide()
        {
            slider.gameObject.SetActive(false);
        }
        
        private void Awake()
        {
            if (health == null) health = GetComponentInParent<Health>();
            if (slider == null) slider = GetComponentInChildren<Slider>();
        }

        private void OnEnable()
        {
            if (health)
            {
                health.changed.AddListener(UpdateHealth);
                health.onDied.AddListener(Hide);
            }
        }

        private void OnDisable()
        {
            if (health)
            {
                health.changed.RemoveListener(UpdateHealth);
                health.onDied.RemoveListener(Hide);
            }
        }

        private void Start()
        {
            UpdateHealth();
        }
    }
}