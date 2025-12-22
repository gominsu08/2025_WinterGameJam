using System;
using UnityEngine;
using Work.KJY.Code.Event;
using Work.Utils.EventBus;

namespace Work.KJY.Code.Item
{
    public class BoostChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private BoostType boostType;

        private void OnTriggerEnter(Collider other)
        {
            if ((playerLayer & (1 << other.gameObject.layer)) != 0)
            {
                Bus<GetBoostItemEvent>.Raise(new GetBoostItemEvent(boostType));
            }
        }
    }
}