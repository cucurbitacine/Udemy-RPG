using System;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class DamageText : MonoBehaviour
    {
        public Animation animation;
        public TextMeshProUGUI text;

        public void Play(float damage)
        {
            if (text)
            {
                text.text = $"{damage:F0}";
            }

            if (animation)
            {
                animation.Play();
            }
        }

        private void Update()
        {
            if (!animation || !animation.isPlaying)
            {
                Destroy(gameObject);
            }
        }
    }
}