using Work.Utils.EventBus;

namespace Work.KJY.Code.Event
{
    public struct TreeInteractEvent : IEvent
    {
        public bool IsInPlayer;

        public TreeInteractEvent(bool isInPlayer)
        {
            IsInPlayer = isInPlayer;
        }
    }
}