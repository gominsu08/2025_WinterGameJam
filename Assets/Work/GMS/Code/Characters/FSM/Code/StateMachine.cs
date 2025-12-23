using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Work.Entities;

namespace Work.Characters.FSM.Code
{
    public class StateMachine
    {
        #region Member

        private Dictionary<string,State> states = new Dictionary<string, State>();

        public State CurrentState { get; private set; }

        #endregion

        #region Initialize

        public void Initialized(List<StateSO> stateList, Entity entity)
        {
            foreach(StateSO item in stateList)
            {
                Type type = Type.GetType(item.statePath);
                Debug.Assert(type != null, $"Type '{item.statePath}' not found.");
                State stateInstance = (State)Activator.CreateInstance(type, entity,item.animationHash);
                states.Add(item.stateName, stateInstance);
            }

            ChangeState("IDLE");
        }

        #endregion

        #region Method

        public void ChangeState(string stateName, bool isForcing = false)
        {
            if (CurrentState != null && !isForcing && CurrentState == states[stateName])
                return;

            if (states.ContainsKey(stateName))
            {
                CurrentState?.Exit();    
                CurrentState = states[stateName];
                CurrentState?.Enter();
            }
            else
            {
                throw new Exception($"State '{stateName}' not found in the state machine.");
            }
        }

        public void Update()
        {
            CurrentState?.Update();
        }

        #endregion

        #region Debug Method

        public void DebugAllStateCheck()
        {
            foreach(var state in states)
            {
                Debug.Log($"State Name: {state.Key}, State Type: {state.Value.GetType().Name}");
            }
        }

        internal void OnDestroy()
        {
            CurrentState?.Exit();
        }

        #endregion
    }
}
