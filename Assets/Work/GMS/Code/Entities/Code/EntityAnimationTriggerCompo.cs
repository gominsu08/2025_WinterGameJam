using System;
using System.Collections;
using UnityEngine;

namespace Work.Entities.Code
{
    public class EntityAnimationTriggerCompo : MonoBehaviour, IEntityComponent
    {
        public Entity Owner { get; private set; }
        public Action OnAnimationEndEvent;

        public void InitCompo(Entity entity)
        {
            Owner = entity;
        }

        public void AnimationEnd() => OnAnimationEndEvent?.Invoke();
    }
}