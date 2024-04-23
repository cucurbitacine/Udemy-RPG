using Game.Stats;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class XPDisplay : MonoBehaviour
    {
        public XP xp;

        [Space]
        public string title = "XP";
        
        [Space]
        public TextMeshProUGUI label;
        public TextMeshProUGUI value;

        private string GetLabelText()
        {
            return $"{title}:";
        }
        
        private string GetValueText()
        {
            return xp ? $"{xp.points:F0}" : "N/A";
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