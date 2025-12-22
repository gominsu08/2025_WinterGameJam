using UnityEngine;
using Work.Characters.Code;
using Work.Entities;
using Work.GMS.Code.Snowballs;

namespace Work.GMS.Code.Characters.Code
{
    public class SnowBallCompo : MonoBehaviour, IEntityComponent
    {
        public Entity Owner { get; private set; }
        public float CurrentSnowRadius => snowball.currentRadius;

        [SerializeField] private Transform snowballTransform;
        [SerializeField] private Snowball snowball;
        [SerializeField] private float snowRadius = 0.3f;


        private CharacterMovementCompo mover;
        private const float SNOW_GROWTH_RATE = 0.05f; // 눈덩이 반지름 증가율
        private float _multiplier = 1f;



        public void InitCompo(Entity entity)
        {
            Owner = entity;

            snowball.Init(entity as Character);
            snowball.SetSnow(CurrentSnowRadius);

            mover = Owner.GetCompo<CharacterMovementCompo>();

        }

        private void Update()
        {
            if (mover.IsMoveing)
            {
                if (snowball.IsOnSnow())
                    snowball.SetSnow(CurrentSnowRadius + SNOW_GROWTH_RATE * Time.deltaTime);
                //float angle = Mathf.Atan2();
            }
        }

    }
}