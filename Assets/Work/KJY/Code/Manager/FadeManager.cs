using UnityEngine;
using UnityEngine.UI;
using Work.KJY.Code.Core;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Work.KJY.Code.Manager
{
    public class FadeManager : MonoSingleton<FadeManager>
    {
        [SerializeField] private Image fadeObject;

        private Tween fadeTween;
        private bool isFaded;

        private RectTransform fadeRect;

        private void Awake()
        {
            fadeRect = fadeObject.rectTransform;
        }

        private void Start()
        {
            //FadeOut();
        }

        public void FadeIn(float duration = 1f, string sceneName = "")
        {
            Fade(Vector3.zero, duration, true, sceneName);
        }

        public void FadeOut(float duration = 1f)
        {
            Fade(Vector3.one, duration, false);
        }

        private void Fade(Vector3 targetScale, float duration, bool targetFaded, string sceneName = "")
        {
            if (fadeObject == null)
                return;

            if (fadeTween != null && fadeTween.IsActive())
                return;

            fadeTween?.Kill();

            fadeObject.gameObject.SetActive(true);

            fadeTween = fadeRect
                .DOScale(targetScale, duration)
                .SetEase(Ease.InOutQuad)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    isFaded = targetFaded;

                    if (!isFaded)
                        fadeObject.gameObject.SetActive(false);

                    if (!string.IsNullOrEmpty(sceneName))
                        SceneManager.LoadScene(sceneName);
                });
        }
    }
}