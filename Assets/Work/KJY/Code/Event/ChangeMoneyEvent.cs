using UnityEngine;
using Work.Utils.EventBus;

namespace Work.KJY.Code.Event
{
    public struct ChangeMoneyEvent :  IEvent
    {
        public int CurMoney;

        public ChangeMoneyEvent(int curMoney)
        {
            CurMoney = curMoney;
        }
    }
}