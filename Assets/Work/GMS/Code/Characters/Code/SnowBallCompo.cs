using UnityEngine;
using Work.Characters.Code;
using Work.Entities;
using Work.GMS.Code.Characters.Code.Test;
using Work.GMS.Code.Snowballs;

namespace Work.GMS.Code.Characters.Code
{
    public class SnowBallCompo : MonoBehaviour, IEntityComponent
    {
        public Entity Owner { get; private set; }
        public float CurrentSnowRadius { get; private set; } = 0.3f;


        [SerializeField] private Transform particleTrm;
        [SerializeField] private Transform snowballTransform;
        [SerializeField] private Snowball snowball;
        [SerializeField] private float snowRadius = 0.3f;


        private CharacterMovementCompo mover;
        private const float SNOW_GROWTH_RATE = 0.05f; // 눈덩이 반지름 증가율
        private float _multiplier = 1f;
        private CharacterBoostCompo _boostCompo;

        public void InitCompo(Entity entity)
        {
            Owner = entity;

            snowball.Init(entity as Character);
            snowball.SetSnow(CurrentSnowRadius);

            mover = Owner.GetCompo<CharacterMovementCompo>();
            _boostCompo = Owner.GetCompo<CharacterBoostCompo>();
        }

        private void Update()
        {

            Debug.Log(snowball.IsOnSnow() ? "눈위에 있음" : "눈위에 없음");

            if (mover.IsMoveing && snowball.IsOnSnow())
            {

                if (_boostCompo.IsSnowBoost)
                    _multiplier = 1.5f;
                else
                    _multiplier = 1f;

                CurrentSnowRadius += SNOW_GROWTH_RATE * _multiplier * Time.deltaTime;

                snowball.transform.localPosition = new Vector3(0, (CurrentSnowRadius / 2) - 0.776f, (CurrentSnowRadius / 2));
                particleTrm.position = snowball.transform.position;
                snowball.transform.Rotate((10 * mover.CurrentSpeed + (CurrentSnowRadius * 4)) * Time.deltaTime, 0, 0f);

                snowball.SetSnow(CurrentSnowRadius);

                //float angle = Mathf.Atan2();
            }
        }

        internal void Set()
        {
            CurrentSnowRadius = CurrentSnowRadius - (CurrentSnowRadius / 10);
            snowball.transform.localScale = Vector3.one * CurrentSnowRadius;
            snowball.transform.localPosition = new Vector3(0, (CurrentSnowRadius / 2) - 0.776f, (CurrentSnowRadius / 2));
            particleTrm.position = snowball.transform.position;
            snowball.transform.Rotate((10 * mover.CurrentSpeed + (CurrentSnowRadius * 4)) * Time.deltaTime, 0, 0f);
            snowball.SetSnow(CurrentSnowRadius);
        }

        public float ResetCompo()
        {
            float prev = CurrentSnowRadius;
            CurrentSnowRadius = snowRadius;
            snowball.transform.localScale = Vector3.one * CurrentSnowRadius;
            snowball.transform.localPosition = new Vector3(0, (CurrentSnowRadius / 2) - 0.776f, (CurrentSnowRadius / 2));
            snowball.SetSnow(CurrentSnowRadius);
            return prev;
        }
    }
}