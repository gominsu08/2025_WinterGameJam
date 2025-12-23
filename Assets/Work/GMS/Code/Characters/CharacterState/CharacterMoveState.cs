using UnityEngine;
using Work.Characters.Events;
using Work.Entities;
using Work.KJY.Code.Manager;

namespace Work.GMS.Code.Characters.CharacterState
{
    public class CharacterMoveState : CharacterCanMoveState
    {
        public CharacterMoveState(Entity entity, int animHash) : base(entity, animHash)
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
            SoundManager.Instance.Play2DSFX("STONE1"); // + Random.Range(1, 2).ToString());
        }

        protected override void MoveHandler(CharacterMoveEvent evt)
        {
            if (evt.MoveDirection == Vector3.zero)
            {
                //SoundManager.Instance.
                _stateCompo.ChangeState("IDLE");
            }
        }
    }
}