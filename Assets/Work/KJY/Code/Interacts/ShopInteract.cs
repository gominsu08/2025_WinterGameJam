using System;
using UnityEngine;
using Work.Characters.Events;
using Work.Utils.EventBus;

namespace Work.KJY.Code.Interacts
{
    public class ShopInteract : MonoBehaviour
    {
        [SerializeField] private LayerMask playerMask;

        private bool _isInPlayer;

        private void Start()
        {
            Bus<CharacterInteractionEvent>.Events += OnInteractShop;
        }

        private void OnInteractShop(CharacterInteractionEvent evt)
        {
            if (_isInPlayer)
            {
                // 여기에 Shop UI 띄우면 될듯
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if ((playerMask & (1 << other.gameObject.layer)) != 0)
                _isInPlayer = true;
            else
                _isInPlayer = false;
        }
    }
}