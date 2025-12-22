using DG.Tweening;
using UnityEngine;
using Work.Characters.Code;
using Work.Entities;
using Work.KJY.Code.Event;
using Work.Utils.EventBus;

namespace Work.GMS.Code.Characters.Code.Test
{
    public class CharacterBoostCompo : MonoBehaviour, IEntityComponent
    {
        public Entity Owner { get; private set; }
        public bool IsShield { get; private set; } = false;
        public bool IsSnowBoost { get; private set; } = false;
        //public bool Is
        [SerializeField] private float boostDuration = 1.2f;

        private CharacterMovementCompo _mover;

        public void InitCompo(Entity entity)
        {
            Owner = entity;
            _mover = Owner.GetCompo<CharacterMovementCompo>();

            Bus<GetBoostItemEvent>.Events += HandleBoostItemEevent;
        }

        private void HandleBoostItemEevent(GetBoostItemEvent evt)
        {
            switch (evt.Type)
            {
                case BoostType.SnowBoost:
                    SnowBoost();
                    break;
                case BoostType.SnowShield:
                    SnowShield();
                    break;
                case BoostType.SpeedBoost:
                    SpeedBoost();
                    break;
                default:
                    break;
            }
        }

        public void SnowBoost()
        {
            IsSnowBoost = true;
        }

        public void SnowShield()
        {
            IsShield = true;
        }

        public void SpeedBoost()
        {
            _mover.SetMultiplier(1.25f);
            DOVirtual.DelayedCall(boostDuration, () =>
            {
                _mover.SetMultiplier(1f);
            });
        }
    }
}