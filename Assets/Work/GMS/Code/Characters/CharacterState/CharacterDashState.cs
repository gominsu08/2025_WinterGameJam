using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Characters.FSM.Code;
using Work.Entities;

namespace Work.GMS.Code.Characters.CharacterState
{
    public class CharacterDashState : State
    {
        

        public CharacterDashState(Entity entity, int animHash) : base(entity, animHash)
        {

        }
    }
}
