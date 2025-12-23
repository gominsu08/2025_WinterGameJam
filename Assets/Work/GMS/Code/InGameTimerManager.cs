using DG.Tweening;
using TMPro;
using UnityEngine;
using Work.Characters;
using Work.GMS.Code.Data;
using Work.GMS.Code.UIs;
using Work.KJY.Code.Manager;
using Work.Utils.EventBus;

namespace Work.GMS.Code
{
    public struct GameEndEvent : IEvent
    {
    }


    public class InGameTimerManager : MonoBehaviour
    {
        [SerializeField] private float playTime;
        [SerializeField] private CharacterDataContainer characterDataContainer;
        [SerializeField] private CountDownUI countDownUI;
        [SerializeField] private TextMeshProUGUI text;

        private float _timer = 0;
        private bool _isGameStarted = false;
        private bool _isGameFinished = false;



        private float _firstBallsize = 0;
        private float _secondBallsize = 0;

        private bool _isFirstSet = true;

        private void Awake()
        {
            CountDown();

            
        }

        public void CountDown()
        {
            countDownUI.CountDown(() => StartGame());
        }

        public void StartGame()
        {
            Bus<GameStartEvent>.Raise(new GameStartEvent());
            text.gameObject.SetActive(true);
            _isGameStarted = true;
        }

        public void Update()
        {
            if (!_isGameStarted || _isGameFinished)
                return;
            _timer += Time.deltaTime;
            text.SetText($"{(int)(playTime - _timer)}");

            if (_timer >= playTime)
            {
                GameReset();
                text.gameObject.SetActive(false);
                _timer = 0;

            }
        }

        [ContextMenu("Reset")]
        public void GameReset()
        {
            _isGameStarted = false;


            if (_isFirstSet)
            {
                Fade();
            }
            else
            {
                _secondBallsize = characterDataContainer.ResetCharacter();
                _isGameFinished = true;
                InGameEnd();
            }

            _isFirstSet = false;
        }

        public void Fade()
        {
            IrisFadeManager.Instance.FadeIn(0.5f);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                _firstBallsize = characterDataContainer.ResetCharacter();
                IrisFadeManager.Instance.FadeOut(0.5f);
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    CountDown();
                });
            });
        }

        public void InGameEnd()
        {
            DataContainer.Instance.SetRadius(_firstBallsize, _secondBallsize);

            IrisFadeManager.Instance.FadeIn(1f, "SnowmanDeco");

        }
    }
}