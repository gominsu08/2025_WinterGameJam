using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work.Utils.EventBus
{
    public static class Bus<T> where T : IEvent
    {
        public static Action<T> Events;

        public static void Raise(T evt) => Events?.Invoke(evt);
    }
}
