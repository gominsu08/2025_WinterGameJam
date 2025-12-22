using UnityEngine;

namespace Work.Characters.FSM.Code
{
    [CreateAssetMenu(fileName = "StateData", menuName = "SO/Characters/FSM/StateData", order = -1)]
    public class StateSO : ScriptableObject
    {
        public string stateName;
        public string statePath;
        public int animationHash { get; private set; }

        private void OnValidate()
        {
            if (!string.IsNullOrEmpty(stateName))
                animationHash = Animator.StringToHash(stateName);
        }
    }
}