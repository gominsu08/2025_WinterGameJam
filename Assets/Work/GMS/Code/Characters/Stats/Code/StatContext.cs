using UnityEngine;

namespace Work.Characters.Stats.Code
{
    public struct StatContext
    {
        public string StatName { get; private set; }
        public string StatDescription { get; private set; }
        public Sprite StatIcon { get; private set; }
        public float BaseValue { get; private set; }
        public float MaxValue { get; private set; }
        public float MinValue { get; private set; }

        public StatContext(string statName,string statDescription,Sprite statIcon,float baseValue,float maxValue,float minValue)
        {
            StatName = statName;
            StatDescription = statDescription;
            StatIcon = statIcon;
            BaseValue = baseValue;
            MaxValue = maxValue;
            MinValue = minValue;
        }
    }
}
