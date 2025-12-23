using UnityEngine;
using UnityEngine.SceneManagement;
using Work.Characters;
using Work.GMS.Code.Data;
using Work.GMS.Code.UIs;
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
            _isGameStarted = true;
        }

        public void Update()
        {
            if (!_isGameStarted || _isGameFinished)
                return;
            _timer += Time.deltaTime;

            if (_timer >= playTime)
            {
                GameReset();
                _timer = 0;
                
            }
        }

        [ContextMenu("Reset")]
        public void GameReset()
        {
            _isGameStarted = false;


            if (_isFirstSet)
            {
                _firstBallsize = characterDataContainer.ResetCharacter();
                CountDown();
            }
            else
            {
                _secondBallsize = characterDataContainer.ResetCharacter();
                _isGameFinished = true;
                InGameEnd();
            }

            _isFirstSet = false;
        }

        public void InGameEnd()
        {
            DataContainer.Instance.SetRadius(_firstBallsize, _secondBallsize);

            SceneManager.LoadScene("SnowmanDeco");
        }
    }
}