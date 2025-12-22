using UnityEngine;
using Work.Characters.Code;
using Work.Inputs;

namespace Work.Characters
{
    public class CharacterDataContainer : MonoBehaviour //다른 곳에서 플레이어의 데이터가 필요할때 플레이어를 직접적으로 참조하는것이 아니라 이곳에서 데이터만 가지고 간다.
    {
        [field: SerializeField] public Character CurrentCharacter { get; private set; }
        public CharacterDataSO CurrentCharacterData { get; private set; }
        public Vector3 MoveDirection { get; private set; }

        public bool IsPushMode => CurrentCharacter.IsPushMode;

        private InputContainer _inputContainer;

        private void Awake()
        {
            _inputContainer = new InputContainer();
            _inputContainer.Init();
        }
    }
}