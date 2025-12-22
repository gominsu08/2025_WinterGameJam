using System;
using System.Collections.Generic;
using UnityEngine;
using Work.Characters.Code;
using Work.Entities.Code;

namespace Work.Characters.Stats.Code
{
    public class StatContainer
    {
        #region Member

        private Dictionary<string, Stat> stats;

        #endregion

        #region Initailizer

        public void InitailizeStatContainer(AbstractEntityDataSO abstarectEntityData)
        {
            if (abstarectEntityData == null) return;
            SetStatDict(abstarectEntityData);
        }

        #endregion

        #region Method

        private void SetStatDict(AbstractEntityDataSO abstarectEntityData)
        {
        }

        public Stat GetStat(string statName)
        {
            if (stats.TryGetValue(statName, out Stat stat))
            {
                return stat;
            }
            return default;
        }

        public float GetStatValue(string statName)
        {
            Stat stat = GetStat(statName);
            if (stat == null) return 0f;
            return stat.Value;
        }

        public void AddListenerValueChangedEvent(Action<float, float> action, string statName)
        {
            Stat stat = GetStat(statName);
            if (stat == null) return;
            stat.AddListenerValueChanged(action);
        }

        public void RemoveListenerValueChangedEvent(Action<float, float> action, string statName)
        {
            Stat stat = GetStat(statName);
            if (stat == null) return;
            stat.RemoveListenerValueChanged(action);
        }

        public void RemoveAllValueChangedEvent(string statName)
        {
            Stat stat = GetStat(statName);
            if (stat == null) return;
            stat.RemoveAllListenerValueChanged();
        }

        public void AddModifier(string statName, IStatUpgrader statUpgrader, float value)
        {
            Stat stat = GetStat(statName);
            if (stat == null || statUpgrader == null) return;
            stat.AddModifier(statUpgrader, value);
        }

        public void RemoveModifier(string statName, IStatUpgrader statUpgrader)
        {
            Stat stat = GetStat(statName);
            if (stat == null || statUpgrader == null) return;
            stat.RemoveModifier(statUpgrader);
        }

        public Stat GetAttackStatForAttackType(AttackTypeEnum type)
        {
            string attackType = type switch
            {
                AttackTypeEnum.Magic => "INT",
                AttackTypeEnum.Melee => "STR",
                AttackTypeEnum.None => throw new NotImplementedException(),
            };

            Stat stat = GetStat(attackType);
            Debug.Assert(stat != null, $"Not find {attackType}");
            return stat;
        }

        public Stat GetDefenceStatForAttackType(AttackTypeEnum type)
        {
            string attackType = type switch
            {
                AttackTypeEnum.Magic => "SAN",
                AttackTypeEnum.Melee => "DEF",
                AttackTypeEnum.None => throw new NotImplementedException(),
            };

            Stat stat = GetStat(attackType);
            Debug.Assert(stat != null, $"Not find {attackType}");
            return stat;
        }

        #endregion
    }
}