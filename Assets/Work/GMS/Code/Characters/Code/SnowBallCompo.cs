using DG.Tweening;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Work.Characters.Code;
using Work.Characters.Events;
using Work.Entities;
using Work.GMS.Code.Snowballs;
using Work.Utils.EventBus;

namespace Work.GMS.Code.Characters.Code
{
    public class SnowBallCompo : MonoBehaviour, IEntityComponent
    {
        public Entity Owner { get; private set; }
        public float CurrentSnowRadius { get; private set; }

        [SerializeField] private Transform snowballTransform;
        [SerializeField] private Snowball snowball;
        [SerializeField] private float snowRadius = 0.3f;


        private CharacterMovementCompo mover;
        private const float SNOW_GROWTH_RATE = 0.05f; // 눈덩이 반지름 증가율
        private float _multiplier = 1f;



        public void InitCompo(Entity entity)
        {
            Owner = entity;

            CurrentSnowRadius = snowRadius * _multiplier;

            snowball.SetSnow(CurrentSnowRadius);

            mover = Owner.GetCompo<CharacterMovementCompo>();
        }

        private void Update()
        {
            if(mover.IsMoveing)
            {
                CurrentSnowRadius += SNOW_GROWTH_RATE * _multiplier * Time.deltaTime;
                snowball.transform.localPosition  = new Vector3(0, (CurrentSnowRadius / 2) - 0.776f, (CurrentSnowRadius / 2));
                snowball.transform.Rotate((10 * mover.CurrentSpeed + (CurrentSnowRadius * 4)) * Time.deltaTime, 0, 0f);
                //float angle = Mathf.Atan2();
                snowball.SetSnow(CurrentSnowRadius);
            }
        }

        public void SetMultiplier(float multiplier)
        {
            _multiplier = multiplier;
        }

    }
}