using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work.Entities
{
    public interface IEntityComponent
    {
        Entity Owner { get; }

        void InitCompo(Entity entity);
    }
}
