using UnityEngine;
using Work.Characters.Code;
using Work.Characters.Events;
using Work.Characters.FSM.Code;
using Work.Entities;
using Work.Utils.EventBus;

namespace Work.GMS.Code.Characters.CharacterState
{
    public class CharacterIdleState : State
    {
        private Character _character;
        private StateCompo _stateCompo;
        private CharacterMovementCompo _movementCompo;

        public CharacterIdleState(Entity entity, int animHash) : base(entity, animHash)
        {
            _character = entity as Character;
            _stateCompo = _character.GetCompo<StateCompo>();
            _movementCompo = _character.GetCompo<CharacterMovementCompo>();
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Idle 진입");
            _movementCompo.SetCanMove(false);
            Bus<CharacterMoveEvent>.Events += HandleMoveDirectionChanged;
        }

        public override void Exit()
        {
            base.Exit();
            Bus<CharacterMoveEvent>.Events -= HandleMoveDirectionChanged;
        }

        private void HandleMoveDirectionChanged(CharacterMoveEvent evt)
        {
            if (evt.MoveDirection != Vector3.zero)
            {

                string stateName = _character.IsPushMode ? "PUSH" : "MOVE";
                _stateCompo.ChangeState(stateName);
            }
        }
    }
}