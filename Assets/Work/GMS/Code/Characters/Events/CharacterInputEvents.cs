using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Work.Utils.EventBus;

namespace Work.Characters.Events
{
    public class CharacterInputEvents { }

    public struct CharacterMoveEvent : IEvent
    {
        public Vector3 MoveDirection { get; private set; }

        public CharacterMoveEvent(Vector3 dir)
        {
            MoveDirection = dir;
        }
    }

    public struct CharacterSkillEvent : IEvent
    {
        public int skillNumber {  get; private set; }

        public CharacterSkillEvent(int number)
        {
            skillNumber = number;
        }
    }

    public struct CharacterDashEvent : IEvent
    {

    }

    public struct CharacterUltimateSkillEvent : IEvent
    {

    }

    public struct CharacterInteractionEvent : IEvent
    {

    }
}
