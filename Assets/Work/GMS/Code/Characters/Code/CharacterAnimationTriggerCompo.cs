using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Entities.Code;

namespace Work.Characters.Code
{
    public class CharacterAnimationTriggerCompo : EntityAnimationTriggerCompo
    {
        public event Action OnAttackTriggerEvent;

        public void HandleAttackTrigger() => OnAttackTriggerEvent?.Invoke();
    }
}
