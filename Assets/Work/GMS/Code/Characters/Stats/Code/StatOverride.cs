using System;
using UnityEngine;

namespace Work.Characters.Stats.Code
{
    [Serializable]
    public class StatOverride
    {
        public string name;
        [SerializeField] private StatValueSO stat;
        [SerializeField] private bool isOverrid;
        [SerializeField] private float overridValue;

        public Stat CreateStat()
        {
            StatContext context;
            if (isOverrid)
                context = this.stat.GenerateStatContext(overridValue);
            else
                context = this.stat.GenerateStatContext();
            Stat stat = new Stat(context);

            return stat;
        }

        
    }
}
