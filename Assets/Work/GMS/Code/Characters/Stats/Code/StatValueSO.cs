using UnityEngine;

namespace Work.Characters.Stats.Code
{
    [CreateAssetMenu(fileName = " Stat value data", menuName = "SO/Characters/Stats/StatValueData", order = 0)]
    public class StatValueSO : ScriptableObject
    {
        public string statName;
        public string statDescription;
        public Sprite statIcon;
        public float baseValue;
        public float maxValue;
        public float minValue;

        public StatContext GenerateStatContext(float overrideValue = -1)
        {
            StatContext context;
            if (overrideValue >= 0)
            {
                context = new StatContext(statName, statDescription, statIcon, overrideValue, maxValue, minValue);
            }
            else
                context = new StatContext(statName, statDescription, statIcon, baseValue, maxValue, minValue);

            return context;
        }
    }
}
