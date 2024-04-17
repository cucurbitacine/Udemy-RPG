using System.Collections;
using UnityEngine;

namespace Game.SceneManagement
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Fader : MonoBehaviour
    {
        public static Fader Singleton { get; private set; }
        
        public bool fadeOutOnStart = true;
        public float fadeDuration = 1f;

        public CanvasGroup canvasGroup => _canvasGroup ? _canvasGroup : (_canvasGroup = GetComponent<CanvasGroup>());
        
        private CanvasGroup _canvasGroup;
        private Coroutine _fading;

        public Coroutine Fade(float begin, float end, float duration)
        {
            if (_fading != null) StopCoroutine(_fading);
            _fading = StartCoroutine(Fading(begin, end, duration));
            return _fading;
        }

        public Coroutine FadeIn(float duration)
        {
            return Fade(0, 1, duration);
        }
        
        public Coroutine FadeOut(float duration)
        {
            return Fade(1, 0, duration);
        }

        public Coroutine FadeIn() => FadeIn(fadeDuration);
        public Coroutine FadeOut() => FadeOut(fadeDuration);
        
        private IEnumerator Fading(float begin, float end, float duration)
        {
            begin = Mathf.Clamp01(begin);
            end = Mathf.Clamp01(end);

            if (duration > 0.001f)
            {
                var time = 0f;
                while (time < duration)
                {
                    var t = time / duration;

                    canvasGroup.alpha = Mathf.Lerp(begin, end, t);

                    time += Time.deltaTime;
                    yield return null;
                }
            }

            canvasGroup.alpha = end;
        }

        private void Awake()
        {
            if (Singleton)
            {
                Destroy(gameObject);
            }
            else
            {
                Singleton = this;
            }
        }

        private void Start()
        {
            if (fadeOutOnStart) FadeOut();
        }
    }
}