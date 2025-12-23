using UnityEngine;
using Work.Characters.Events;
using Work.Utils.EventBus;

namespace Work.KJY.Code.Interacts
{
    public class PlayInteract : MonoBehaviour
    {
        [SerializeField] private LayerMask playerMask;
        
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
                Debug.Log("플레이 씬 변경");
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