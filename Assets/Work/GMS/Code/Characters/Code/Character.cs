using System.Collections;
using UnityEngine;
using Work.Entities;
using Work.Entities.Code;

namespace Work.Characters.Code
{
    public class Character : Entity
    {
        //public bool Is
        public CharacterDataSO CharacterData => EntityDataSO as CharacterDataSO;
    }
}