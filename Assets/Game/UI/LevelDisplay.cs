using Game.Stats;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class LevelDisplay : MonoBehaviour
    {
        public BaseStats stats;

        [Space]
        public string title = "Level";
        
        [Space]
        public TextMeshProUGUI label;
        public TextMeshProUGUI value;

        private string GetLabelText()
        {
            return $"{title}:";
        }
        
        private string GetValueText()
        {
            return stats ? $"{stats.GetLevel()}" : "N/A";
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