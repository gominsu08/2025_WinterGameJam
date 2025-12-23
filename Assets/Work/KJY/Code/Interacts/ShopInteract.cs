using System;
using UnityEngine;
using Work.Characters.Events;
using Work.Utils.EventBus;

namespace Work.KJY.Code.Interacts
{
    public class ShopInteract : MonoBehaviour
    {
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private GameObject shopUI;

        private bool _isInPlayer;

        private void Start()
        {
            Bus<CharacterInteractionEvent>.Events += OnInteractShop;
        }

        private void OnDestroy()
        {
            Bus<CharacterInteractionEvent>.Events -= OnInteractShop;
        }

        private void OnInteractShop(CharacterInteractionEvent evt)
        {
            if (_isInPlayer)
            {
                // 여기에 Shop UI 띄우면 될듯
                shopUI.SetActive(true);
                Debug.Log("상점 인터랙트 실행");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((playerMask & (1 << other.gameObject.layer)) != 0) 
                _isInPlayer = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if ((playerMask & (1 << other.gameObject.layer)) != 0)
                _isInPlayer = false;
        }
    }
}