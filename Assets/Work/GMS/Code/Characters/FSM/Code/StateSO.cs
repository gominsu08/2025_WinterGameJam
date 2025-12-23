using UnityEngine;

namespace Work.Characters.FSM.Code
{
    [CreateAssetMenu(fileName = "StateData", menuName = "SO/Characters/FSM/StateData", order = -1)]
    public class StateSO : ScriptableObject
    {
        public string stateName;
        public string statePath;
        
        [SerializeField, HideInInspector] private int _animationHash;
        
        public int animationHash
        {
            get
            {
                if (_animationHash == 0 && !string.IsNullOrEmpty(stateName))
                {
                    _animationHash = Animator.StringToHash(stateName);
                }
                return _animationHash;
            }
        }

        private void OnValidate()
        {
            if (!string.IsNullOrEmpty(stateName))
                _animationHash = Animator.StringToHash(stateName);
        }
    }
}