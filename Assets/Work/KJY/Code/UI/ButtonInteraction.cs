using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Work.KJY.Code.UI
{
    public class ButtonInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float duration = 0.2f;
        [SerializeField] private float scaleFactor = 1.05f;

        private Vector3 _defaultScale;
        private Tween _currentTween;

        private void Awake()
        {
            _defaultScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _currentTween?.Kill();

            _currentTween = transform.DOScale(_defaultScale * scaleFactor, duration).SetEase(Ease.OutQuad);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _currentTween?.Kill();

            _currentTween = transform.DOScale(_defaultScale, duration).SetEase(Ease.OutQuad);
        }
    }
}