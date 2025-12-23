using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Work.KJY.Code.Core;

namespace Work.KJY.Code.Manager
{
    public class IrisFadeManager : MonoSingleton<IrisFadeManager>
    {
        [SerializeField] private RawImage irisImage;
        [SerializeField] private string radiusProperty = "_Radius";
        [SerializeField] private bool openOnSceneLoaded = true;
        [SerializeField] private float defaultOpenDuration = 0.6f;

        private Tween tween;
        private Material runtimeMat;
        private int radiusId;
        private Coroutine bindRoutine;
        private bool initialized;

        private void Start()
        {
            if (initialized)
                return;

            initialized = true;

            radiusId = Shader.PropertyToID(radiusProperty);

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;

            BindUIImmediate();
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            KillTween();
            DestroyRuntimeMaterial();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (bindRoutine != null)
                StopCoroutine(bindRoutine);

            bindRoutine = StartCoroutine(BindAndOpenNextFrame());
        }

        private IEnumerator BindAndOpenNextFrame()
        {
            yield return null;

            BindUIImmediate();

            if (!EnsureValidTarget())
                yield break;

            irisImage.gameObject.SetActive(true);
            runtimeMat.SetFloat(radiusId, 0f);

            if (openOnSceneLoaded)
                FadeOut(defaultOpenDuration);
        }

        public void FadeOut(float duration = 1f)
        {
            Play(1f, duration, null);
        }

        public void FadeIn(float duration = 1f, string sceneName = "")
        {
            Play(0f, duration, string.IsNullOrEmpty(sceneName) ? null : sceneName);
        }

        private void Play(float targetRadius, float duration, string sceneName)
        {
            if (!EnsureValidTarget())
            {
                BindUIImmediate();
                if (!EnsureValidTarget())
                    return;
            }

            KillTween();
            irisImage.gameObject.SetActive(true);

            float start = runtimeMat.GetFloat(radiusId);

            tween = DOTween.To(
                () => start,
                v =>
                {
                    if (!EnsureValidTarget())
                    {
                        KillTween();
                        return;
                    }

                    start = v;
                    runtimeMat.SetFloat(radiusId, v);
                },
                targetRadius,
                duration
            )
            .SetEase(Ease.InOutQuad)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                if (!EnsureValidTarget())
                    return;

                runtimeMat.SetFloat(radiusId, targetRadius);

                if (Mathf.Approximately(targetRadius, 1f))
                    irisImage.gameObject.SetActive(false);

                if (!string.IsNullOrEmpty(sceneName))
                    SceneManager.LoadScene(sceneName);
            });
        }

        private bool EnsureValidTarget()
        {
            if (irisImage == null || !irisImage)
                return false;

            if (runtimeMat == null)
                return false;

            return true;
        }

        private void BindUIImmediate()
        {
            if (irisImage == null || !irisImage)
                irisImage = FindIrisFadeRawImage();

            DestroyRuntimeMaterial();

            if (irisImage == null || !irisImage)
                return;

            if (irisImage.material == null)
                return;

            runtimeMat = new Material(irisImage.material);
            irisImage.material = runtimeMat;

            irisImage.gameObject.SetActive(true);
            runtimeMat.SetFloat(radiusId, 1f);
        }

        private RawImage FindIrisFadeRawImage()
        {
            var all = Object.FindObjectsByType<RawImage>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            for (int i = 0; i < all.Length; i++)
            {
                var img = all[i];
                if (img != null && img.gameObject.name == "IrisFade")
                    return img;
            }
            return null;
        }

        private void KillTween()
        {
            if (tween != null && tween.IsActive())
                tween.Kill();

            tween = null;
        }

        private void DestroyRuntimeMaterial()
        {
            if (runtimeMat != null)
                Destroy(runtimeMat);

            runtimeMat = null;
        }
    }
}
