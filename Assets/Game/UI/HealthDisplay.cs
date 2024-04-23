using Game.Attributes;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        public Health health;

        [Space]
        public string title = "Health";
        
        [Space]
        public TextMeshProUGUI label;
        public TextMeshProUGUI value;

        private string GetLabelText()
        {
            return $"{title}:";
        }
        
        private string GetValueText()
        {
            return health ? $"{health.points:F0} / {health.total:F0}" : "N/A";
            return health ? $"{health.points:F0}" : "N/A";
        }
        
        private void Display()
        {
            if (label)
            {
                label.text = GetLabelText();
            }
            
            if (value)
            {
                value.text = GetValueText();
            }
        }
        
        private void Update()
        {
            Display();
        }

        private void OnValidate()
        {
            Display();
        }
    }
}
