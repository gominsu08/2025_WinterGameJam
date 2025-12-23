using UnityEngine;
using Work.Characters.Code;
using Work.Entities;

namespace Work.Characters.FSM.Code
{
    public class StateCompo : MonoBehaviour, IEntityComponent
    {
        #region Member
        public Entity Owner { get; protected set; }

        private Character _character;
        private StateMachine _stateMachine;
        private bool _isCanChangeState = true;

        #endregion

        #region Initialize

        public void InitCompo(Entity entity)
        {
            Owner = entity;
            _character = entity as Character;
            _stateMachine = new StateMachine();
            _stateMachine.Initialized(_character.CharacterData.stateSOs, entity);
        }

        private void OnDestroy()
        {
            _stateMachine.OnDestroy();
        }

        #endregion

        #region Method

        public void ChangeState(string newState, bool isForcing = false)
        {
            if (!_isCanChangeState) return;
            _stateMachine.ChangeState(newState, isForcing);
        }

        #endregion

        #region Debug Method

        [ContextMenu("Show All State")]
        public void DebugShowAllState()
        {
            _stateMachine.DebugAllStateCheck();
        }

        public void SetCanStateChange(bool isCanChangeState = true)
        {
            _isCanChangeState = isCanChangeState;
        }


        #endregion

        #region Unity Built-In Func

        private void Update()
        {
            _stateMachine.Update();
        }

        #endregion
    }
}
