using UnityEngine;
using Work.Characters.Events;
using Work.Entities;

namespace Work.GMS.Code.Characters.CharacterState
{
    public class CharacterMoveState : CharacterCanMoveState
    {

        public CharacterMoveState(Entity entity, int animHash) : base(entity, animHash)
        {
        }

        protected override void MoveHandler(CharacterMoveEvent evt)
        {
            if (evt.MoveDirection == Vector3.zero)
            {
                _stateCompo.ChangeState("IDLE");
            }
        }
    }
}