using UnityEngine;
using Work.Utils.EventBus;

public enum BoostType
{
    SpeedBoost,
    SnowBoost,
    SnowShield,
    Other
}

namespace Work.KJY.Code.Event
{
    public struct GetBoostItemEvent : IEvent
    {
        public BoostType Type;

        public GetBoostItemEvent(BoostType type)
        {
            Type = type;
        }
    }
}