using UnityEngine;
using Work.Entities;

namespace Work.Characters.Code
{
    public class Character : Entity
    {
        [field: SerializeField] public bool IsPushMode { get; private set; }
        public CharacterDataSO CharacterData => EntityDataSO as CharacterDataSO;
    }
}