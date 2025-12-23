using DG.Tweening;
using System;
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
        public bool IsShield = false;
        public bool IsSnowBoost = false;
        public bool IsSprintBoost = false;
        //public bool Is
        [SerializeField] private float boostDuration = 3f;

        private CharacterMovementCompo _mover;


        public Action OnChangeValueEvent;

        public void InitCompo(Entity entity)
        {
            Owner = entity;
            _mover = Owner.GetCompo<CharacterMovementCompo>();

            Bus<GetBoostItemEvent>.Events += HandleBoostItemEevent;
        }

        public void OnDestroy()
        {
            Bus<GetBoostItemEvent>.Events -= HandleBoostItemEevent;
            OnChangeValueEvent = null;
        }

        private void HandleBoostItemEevent(GetBoostItemEvent evt)
        {
            Debug.Log("부스터");
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

            DOVirtual.DelayedCall(boostDuration, () =>
            {
                IsSnowBoost = false;
                OnChangeValueEvent?.Invoke();
            });
        }

        public void SnowShield()
        {
            IsShield = true;
        }

        public void SpeedBoost()
        {
            _mover.SetMultiplier(2f);
            IsSprintBoost = true;
            DOVirtual.DelayedCall(boostDuration, () =>
            {
                _mover.SetMultiplier(1f);
                IsSprintBoost = false;
                OnChangeValueEvent?.Invoke();
            });
        }

        public void SetSieldFalse()
        {
            IsShield = false;
            OnChangeValueEvent?.Invoke();
        }
    }
}