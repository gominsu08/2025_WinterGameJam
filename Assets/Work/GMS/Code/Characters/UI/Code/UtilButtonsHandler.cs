using UnityEngine;
using UnityEngine.UI;
using Work.Characters.Events;
using Work.Utils.EventBus;

namespace Work.Characters.UI.Code
{
    public class UtilButtonsHandler : MonoBehaviour
    {
        public void UseFirstSkill()
        {
            Bus<CharacterSkillEvent>.Raise(new CharacterSkillEvent(1));
        }

        public void UseSecondSkill()
        {
            Bus<CharacterSkillEvent>.Raise(new CharacterSkillEvent(2));
        }

        public void UseUltimateSkill()
        {
            Bus<CharacterUltimateSkillEvent>.Raise(new CharacterUltimateSkillEvent());
        }

        public void UseDash()
        {
            Bus<CharacterDashEvent>.Raise(new CharacterDashEvent());
        }
    }
}