using UnityEngine;
using UnityEngine.InputSystem;
using Work.Characters.Events;
using Work.Utils.EventBus;

namespace Work.Inputs
{
    public class InputContainer : Console.IPlayerActions
    {
        private Console _console;

        public void Init()
        {
            if (_console == null)
            {
                _console = new Console();
                _console.Player.SetCallbacks(this);
            }
            _console.Player.Enable();
        }

        ~InputContainer()
        {
            _console.Player.Disable();
            _console = null;
        }


        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
                Debug.Log("상호작용 입력 감지");
                Bus<CharacterInteractionEvent>.Raise(new CharacterInteractionEvent());
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 vector = context.ReadValue<Vector2>();

            Bus<CharacterMoveEvent>.Raise(new CharacterMoveEvent(new Vector3(vector.x, 0, vector.y)));
        }


    }
}