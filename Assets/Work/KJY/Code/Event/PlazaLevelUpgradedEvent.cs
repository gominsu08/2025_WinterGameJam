using Work.Utils.EventBus;

namespace Work.KJY.Code.Event
{
    public struct PlazaLevelUpgradedEvent : IEvent
    {
        public readonly int NewLevel;

        public PlazaLevelUpgradedEvent(int newLevel)
        {
            NewLevel = newLevel;
        }
    }
}
