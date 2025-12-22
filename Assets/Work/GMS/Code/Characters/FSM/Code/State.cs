using UnityEngine;
using Work.Entities;
using Work.Entities.Code;

namespace Work.Characters.FSM.Code
{
    public abstract class State
    {
        protected Entity _owner;
        protected EntityAnimatorCompo _animator;
        protected EntityAnimationTriggerCompo _trigger;
        protected int _animHash;
        public bool IsAnimationEndTriggered { get; private set; }

        public State(Entity entity, int animHash)
        {
            _owner = entity;
            _animator = _owner.GetCompo<EntityAnimatorCompo>(true);
            _trigger = _owner.GetCompo<EntityAnimationTriggerCompo>(true);
            _animHash = animHash;
        }

        

        public virtual void Enter()
        {
            _animator.SetParam(_animHash,true);
            _trigger.OnAnimationEndEvent += HandleAnimationEndEvent;
        }

        public virtual void Exit()
        {
            _animator.SetParam(_animHash,false);
            IsAnimationEndTriggered = false;
            _trigger.OnAnimationEndEvent -= HandleAnimationEndEvent;
        }

        public virtual void Update()
        {

        }

        public void HandleAnimationEndEvent() => IsAnimationEndTriggered = true;
    }
}