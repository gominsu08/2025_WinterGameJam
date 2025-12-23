using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Work.GMS.Code.UIs
{
    public class CountDownUI : MonoBehaviour
    {
        [SerializeField] private float countDownTime = 3f;
        [SerializeField] private TextMeshProUGUI countDownText;
        [SerializeField] private float moveDuration = 0.2f;
        [SerializeField] private float moveDistance = 1200;

        public bool IsCountingDown { get; private set; } = false;

        private void Awake()
        {
            countDownText.rectTransform.anchoredPosition = new Vector2(-moveDistance, 0);
        }

        public void CountDown(Action action)
        {
            countDownText.rectTransform.anchoredPosition = new Vector2(-moveDistance, 0);
            IsCountingDown = true;
            countDownText.SetText("3");


            countDownText.rectTransform.DOAnchorPos(new Vector2(0, 0), moveDuration).OnComplete(() =>
            {
                DOVirtual.DelayedCall(0.6f, () =>
                {
                    countDownText.rectTransform.DOAnchorPos(new Vector2(moveDistance, 0), moveDuration).OnComplete(()=>
                    {
                        countDownText.SetText("2");
                        countDownText.rectTransform.anchoredPosition = new Vector2(-moveDistance, 0);
                        countDownText.rectTransform.DOAnchorPos(new Vector2(0, 0), moveDuration).OnComplete(() =>
                        {
                            DOVirtual.DelayedCall(0.6f, () =>
                            {
                                countDownText.rectTransform.DOAnchorPos(new Vector2(moveDistance, 0), moveDuration).OnComplete(() =>
                                {
                                    countDownText.SetText("1");
                                    countDownText.rectTransform.anchoredPosition = new Vector2(-moveDistance, 0);
                                    countDownText.rectTransform.DOAnchorPos(new Vector2(0, 0), moveDuration).OnComplete(() =>
                                    {
                                        DOVirtual.DelayedCall(0.6f, () =>
                                        {
                                            countDownText.rectTransform.DOAnchorPos(new Vector2(moveDistance, 0), moveDuration);
                                            action?.Invoke();
                                            IsCountingDown = false;
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
            
        }
    }
}