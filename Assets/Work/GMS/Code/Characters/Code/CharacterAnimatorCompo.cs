using UnityEngine;
using Work.Entities;

namespace Work.Characters.Code
{
    public class CharacterAnimatorCompo : EntityAnimatorCompo
    {
        private Character _character;

        public override void InitCompo(Entity entity)
        {
            base.InitCompo(entity);
            _character = entity as Character;
        }
    }
}