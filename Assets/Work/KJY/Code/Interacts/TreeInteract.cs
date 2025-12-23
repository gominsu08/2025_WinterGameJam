using UnityEngine;
using Work.Characters.Events;
using Work.KJY.Code.Event;
using Work.Utils.EventBus;

namespace Work.KJY.Code.Interacts
{
    public class TreeInteract : MonoBehaviour
    {
        [SerializeField] private LayerMask playerMask;
        
        private bool _isInPlayer;

        private void Start()
        {
            Bus<CharacterInteractionEvent>.Events += OnInteractTree;
        }

        private void OnDestroy()
        {
            Bus<CharacterInteractionEvent>.Events -= OnInteractTree;
        }

        private void OnInteractTree(CharacterInteractionEvent evt)
        {
            if (_isInPlayer)
            {
                
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if ((playerMask & (1 << other.gameObject.layer)) != 0)
            {
                _isInPlayer = true;
                Bus<TreeInteractEvent>.Raise(new TreeInteractEvent(true));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if ((playerMask & (1 << other.gameObject.layer)) != 0)
            {
                _isInPlayer = false;
                Bus<TreeInteractEvent>.Raise(new TreeInteractEvent(false));
            }
        }
    }
}