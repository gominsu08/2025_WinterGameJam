using UnityEngine;
using Work.Entities;

namespace Work.Characters.Code
{
    public class CharacterMovementCompo : MonoBehaviour, IEntityComponent
    {
        #region Member

        private Character _character;
        private DetectSensorCompo _sensor;
        private CharacterAnimatorCompo _animatorCompo;
        private Rigidbody _rbCompo;
        private Vector3 _direction = Vector3.zero; //움직일 방향
        private float _snowRadiuse = 8f;
        [SerializeField] private float _defaultSpeed = 3; //속도 -> 나중에 캐릭터 데이터에서 받아오도록 변경 필요


        public Entity Owner { get; private set; }
        public Transform TargetTransform => _sensor == null ? null : _sensor.CurrentTarget.Transform;
        public bool IsCanMove { get; set; } = true;
        public bool IsPushMode => _character.IsPushMode;
        public bool IsMoveing => _direction != Vector3.zero && IsCanMove;
        public float CurrentSpeed { get; private set; }
        public float CurrentSpeedMultiplier { get; private set; } = 1f;

        #endregion

        //현재 움직일 수 있는 상태인지
        //다른 무언가에 의해서 움직이고 있는지
        #region Init

        public void InitCompo(Entity entity)
        {
            Owner = entity;
            _character = Owner as Character;
            _sensor = _character.GetCompo<DetectSensorCompo>();
            _rbCompo = _character.GetComponent<Rigidbody>();
            _animatorCompo = _character.GetCompo<CharacterAnimatorCompo>();
            CurrentSpeed = _defaultSpeed;
        }



        #endregion

        #region Unity Built-In Method

        private void Update()
        {
            Move();
            Rotate();
        }

        #endregion

        #region Method

        public void SetCanMove(bool isCan) => IsCanMove = isCan;
        public void SetDirection(Vector3 dir) => _direction = dir;

        private void Rotate()
        {
            if (IsPushMode) //타겟이 존재할때
            {
                if (_direction == Vector3.zero) return;
                //_direction.z = 0;

                Quaternion lookRotation = Quaternion.LookRotation(_direction);
                float mul = 1 / _snowRadiuse;

                _character.transform.rotation = Quaternion.Slerp(_character.transform.rotation, lookRotation, Time.deltaTime * 3 * mul);
            }
            else //타겟이 없을때
            {
                if (_direction == Vector3.zero) return;
                Quaternion lookRotation = Quaternion.LookRotation(_direction);
                _character.transform.rotation = Quaternion.Slerp(_character.transform.rotation, lookRotation, Time.deltaTime * 10f);
            }
        }

        public void Knockback()
        {
            _rbCompo.linearVelocity = Vector3.zero;
            _rbCompo.angularVelocity = Vector3.zero;
            Vector3 knockbackDirection = -_character.transform.forward;
            float knockbackForce = 50f;
            _rbCompo.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);
        }

        private void Move()
        {
            Vector3 moveVector = _direction * CurrentSpeed * CurrentSpeedMultiplier;

            if (IsPushMode)
            {
                moveVector = _character.transform.forward * CurrentSpeed * CurrentSpeedMultiplier;
            }


            if (!IsCanMove)
            {
                moveVector = Vector3.zero;
                //_rbCompo.freezeRotation = false;
                _rbCompo.angularVelocity = Vector3.zero;
            }
            else
            {
                //_rbCompo.angularVelocity = Vector3.zero;
                //_rbCompo.freezeRotation = true;
            }

            //moveVector.y += -9.8f;

            _rbCompo.linearVelocity = moveVector;
        }

        public void SetMultiplier(float value = 1f)
        {
            CurrentSpeedMultiplier = value;
            _animatorCompo.SetParam(Animator.StringToHash("MOVE_SPEED"),CurrentSpeedMultiplier);
        }

        #endregion 
    }
}
