using System;
using UnityEngine;
using UnityEngine.UI;
using Work.KJY.Code.Core;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Work.KJY.Code.Manager
{
    public class FadeManager : MonoSingleton<FadeManager>
    {
        [SerializeField] private Image fadeObject;

        private Tween fadeTween;
        private bool isFaded;

        private void Start()
        {
            FadeOut();
        }
        
        private void Update()
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
                FadeIn(2, "Boost Item Test");
            else if (Keyboard.current.eKey.wasPressedThisFrame)
                FadeOut();
        }

        public void FadeIn(float duration = 2f, string sceneName = "")
        {
            Fade(1f, duration, true, sceneName);
        }

        public void FadeOut(float duration = 2f)
        {
            Fade(0f, duration, false);
        }

        private void Fade(float targetAlpha, float duration, bool targetFaded, string sceneName = "")
        {
            if (fadeObject == null) return;

            if (Mathf.Approximately(fadeObject.color.a, targetAlpha))
                return;
            
            if (fadeTween != null && fadeTween.IsActive())
            {
                return;
            }

            fadeTween?.Kill();

            if (targetAlpha > 0)
                fadeObject.gameObject.SetActive(true);

            fadeTween = fadeObject.DOFade(targetAlpha, duration)
                .SetEase(Ease.InOutQuad)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    isFaded = targetFaded;

                    if (!isFaded)
                        fadeObject.gameObject.SetActive(false);

                    if (sceneName != "")
                        SceneManager.LoadScene(sceneName);
                });
        }


    }
}