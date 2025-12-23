using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
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
        private bool isInitialized;
        private void Awake()
        {
            radiusId = Shader.PropertyToID(radiusProperty);

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;

            TryBindUI();
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            KillTween();
            DestroyRuntimeMaterial();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            TryBindUI();

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
                return;

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
            if (irisImage == null)
                return false;

            if (!irisImage)
                return false;

            if (runtimeMat == null)
                return false;

            return true;
        }

        private void TryBindUI()
        {
            if (irisImage == null || !irisImage)
            {
                var found = GameObject.Find("Iris Fade Image");
                if (found != null)
                    irisImage = found.GetComponent<RawImage>();
            }

            if (irisImage == null || !irisImage)
            {
                isInitialized = false;
                DestroyRuntimeMaterial();
                return;
            }

            if (irisImage.material == null)
            {
                isInitialized = false;
                DestroyRuntimeMaterial();
                return;
            }

            DestroyRuntimeMaterial();

            runtimeMat = new Material(irisImage.material);
            irisImage.material = runtimeMat;

            irisImage.gameObject.SetActive(true);
            runtimeMat.SetFloat(radiusId, 1f);

            isInitialized = true;
        }

        private bool EnsureValidTargetOrRebind()
        {
            if (EnsureValidTarget())
                return true;

            TryBindUI();
            return EnsureValidTarget();
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
