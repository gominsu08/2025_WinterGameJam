using System;
using System.Collections.Generic;
using UnityEngine;

namespace Work.Characters.Stats.Code
{
    public class Stat
    {
        #region Member

        public StatContext StatContext {  get; private set; }
        private Dictionary<IStatUpgrader, float> _statModifierDict;

        public delegate void StatValueChangedEvent(float prevValue, float changeValue);
        private event StatValueChangedEvent OnStatValueChangedEvent;
        private Dictionary<Action<float, float>, StatValueChangedEvent> _wrappers;

        private float _value;
        public float Value
        {
            get
            {
                return Mathf.Clamp(_value + _modifyValue, StatContext.MinValue, StatContext.MaxValue);
            }
            set { _value = Mathf.Clamp(value, StatContext.MinValue, StatContext.MaxValue); }
        }

        private float _modifyValue;

        #endregion

        #region Constructor and Destructor
        public Stat(StatContext statContext)
        {
            StatContext = statContext;
            _value = StatContext.BaseValue;
            _statModifierDict = new Dictionary<IStatUpgrader, float>();
            _wrappers = new Dictionary<Action<float, float>, StatValueChangedEvent>();
        }

        ~Stat()
        {
            RemoveAllListenerValueChanged();
        }
        #endregion

        #region Method

        public float GetStatValue()
        {
            return Value;
        }

        public void AddModifier(IStatUpgrader statUpgrader, float value)
        {
            if (_statModifierDict.TryAdd(statUpgrader, value))
            {
                ModifierValueChange(value);
            }
        }

        public void RemoveModifier(IStatUpgrader statUpgrader)
        {
            if (_statModifierDict.TryGetValue(statUpgrader, out float value))
            {
                _statModifierDict.Remove(statUpgrader);
                ModifierValueChange(-value);
            }
        }

        private void ModifierValueChange(float value)
        {
            float prevValue = Value;
            _modifyValue += value;
            OnStatValueChangedEvent?.Invoke(prevValue, Value);
        }

        public void AddListenerValueChanged(Action<float, float> action)
        {
            if (action == null) return;
            if(!_wrappers.TryGetValue(action,out StatValueChangedEvent _))
            {
                StatValueChangedEvent statValueChanged = ((prev, change) => action?.Invoke(prev, change));
                _wrappers[action] = statValueChanged;
                OnStatValueChangedEvent += statValueChanged;
            }
        }

        public void RemoveListenerValueChanged(Action<float, float> action)
        {
            if (action == null) return;
            if(_wrappers.TryGetValue(action, out StatValueChangedEvent targetEvent))
            {
                OnStatValueChangedEvent -= targetEvent;
                _wrappers.Remove(action);
            }
        }

        public void RemoveAllListenerValueChanged()
        {
            _wrappers.Clear();
            OnStatValueChangedEvent = null;
        }

        #endregion
    }
}
