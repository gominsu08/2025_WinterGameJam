using System;
using System.Collections;
using UnityEngine;

namespace Work.Entities.Code
{
    public class EntityAnimationTriggerCompo : MonoBehaviour, IEntityComponent
    {
        public Entity Owner { get; private set; }
        public Action OnAnimationEndEvent;
        public Action OnSnowSoundEvent;

        public void InitCompo(Entity entity)
        {
            Owner = entity;
        }

        public void AnimationEnd() => OnAnimationEndEvent?.Invoke();

        /// <summary>
        /// 공격 애니메이션에서 호출될 트리거입니다.
        /// </summary>
        public void HandleAttackTrigger()
        {
            // 현재 발소리 이벤트가 잘못 연결되어 있어 해당 로직을 HandleFootstepTrigger로 이전합니다.
            // 향후 공격 관련 이벤트가 필요하면 여기에 추가합니다.
        }

        /// <summary>
        /// 발소리 타이밍에 맞춰 이동 애니메이션에서 호출될 트리거입니다.
        /// </summary>
        public void HandleFootstepTrigger() => OnSnowSoundEvent?.Invoke();
    }
}