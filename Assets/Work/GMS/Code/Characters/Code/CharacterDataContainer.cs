using UnityEngine;
using Work.Characters.Code;
using Work.Characters.FSM.Code;
using Work.GMS.Code.Characters.Code;
using Work.Inputs;
using Work.Utils.EventBus;

namespace Work.Characters
{
    public class CharacterDataContainer : MonoBehaviour //다른 곳에서 플레이어의 데이터가 필요할때 플레이어를 직접적으로 참조하는것이 아니라 이곳에서 데이터만 가지고 간다.
    {
        [field: SerializeField] public Character CurrentCharacter { get; private set; }
        public CharacterDataSO CurrentCharacterData { get; private set; }
        public Vector3 MoveDirection { get; private set; }

        public bool IsPushMode => CurrentCharacter.IsPushMode;

        private InputContainer _inputContainer;
        private StateCompo _stateCompo;
        private CharacterMovementCompo _mover;
        private Vector3 _startPosition;

        private void Awake()
        {
            _inputContainer = new InputContainer();
            _inputContainer.Init();
            _startPosition = CurrentCharacter.transform.position;
            Bus<GameStartEvent>.Events += HandleStartGame;
        }

        private void OnDestroy()
        {
            Bus<GameStartEvent>.Events -= HandleStartGame;
            _inputContainer.Destroy();
        }

        private void Start()
        {
            _stateCompo = CurrentCharacter.GetCompo<StateCompo>();
            _mover = CurrentCharacter.GetCompo<CharacterMovementCompo>();

            _mover.SetCanMove(false);
            _stateCompo.SetCanStateChange(false);
        }

        private void HandleStartGame(GameStartEvent evt)
        {
            SetCharacterStateChange();
        }

        public void SetCharacterStateChange(bool value = true)
        {
            _stateCompo.ChangeState("IDLE");
            _stateCompo.SetCanStateChange(value);
        }

        public void SetCharacterMove(bool value = true)
        {
            _mover.SetCanMove(value);
        }

        public float ResetCharacter()
        {
            CurrentCharacter.transform.position = _startPosition;
            CurrentCharacter.transform.rotation = Quaternion.identity;
            _mover.SetCanMove(false);
            SetCharacterStateChange(false);
            float ballsize = CurrentCharacter.GetCompo<SnowBallCompo>().ResetCompo();

            return ballsize;
        }
    }
}