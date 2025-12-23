using UnityEngine;
using Work.Characters.Events;
using Work.Entities;
using Work.KJY.Code.Manager;

namespace Work.GMS.Code.Characters.CharacterState
{
    public class CharacterPushState : CharacterCanMoveState
    {

        public CharacterPushState(Entity entity, int animHash) : base(entity, animHash)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            _trigger.OnSnowSoundEvent += SnowSound;
        }

        public override void Exit()
        {
            base.Exit();
            _trigger.OnSnowSoundEvent -= SnowSound;
        }

        private void SnowSound()
        {
            Debug.Log("SNOW");
            SoundManager.Instance.Play2DSFX("SNOW1");
        }


        protected override void MoveHandler(CharacterMoveEvent evt)
        {
            if (evt.MoveDirection == Vector3.zero)
            {
                _trigger.OnSnowSoundEvent  -= SnowSound;
                _stateCompo.ChangeState("IDLE");
            }
        }
    }
}
