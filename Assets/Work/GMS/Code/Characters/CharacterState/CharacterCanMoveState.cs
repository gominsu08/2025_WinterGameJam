using Work.Characters.Code;
using Work.Characters.Events;
using Work.Characters.FSM.Code;
using Work.Entities;
using Work.Utils.EventBus;

namespace Work.GMS.Code.Characters.CharacterState
{
    public abstract class CharacterCanMoveState : State
    {
        protected Character _character;
        protected StateCompo _stateCompo;
        protected CharacterMovementCompo _movementCompo;

        protected CharacterCanMoveState(Entity entity, int animHash) : base(entity, animHash)
        {
            _character = entity as Character;
            _stateCompo = _character.GetCompo<StateCompo>();
            _movementCompo = _character.GetCompo<CharacterMovementCompo>();
        }

        ~CharacterCanMoveState()
        {
            Bus<CharacterMoveEvent>.Events -= HandleMoveDirectionChanged;
        }


        public override void Enter()
        {
            base.Enter();
            _movementCompo.SetCanMove(true);
            Bus<CharacterMoveEvent>.Events += HandleMoveDirectionChanged;
        }

        public override void Exit()
        {
            base.Exit();
            Bus<CharacterMoveEvent>.Events -= HandleMoveDirectionChanged;
            _movementCompo.SetCanMove(false);
        }

        protected void HandleMoveDirectionChanged(CharacterMoveEvent evt)
        {
            _movementCompo.SetDirection(evt.MoveDirection);
            MoveHandler(evt);
        }

        protected virtual void MoveHandler(CharacterMoveEvent evt)
        {

        }
    }
}