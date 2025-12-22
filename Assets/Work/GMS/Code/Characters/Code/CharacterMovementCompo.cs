using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Work.Entities;

namespace Work.Characters.Code
{
    public class CharacterMovementCompo : MonoBehaviour, IEntityComponent
    {
        #region Member

        private Character _character;
        private DetectSensorCompo _sensor;
        private Rigidbody _rbCompo;
        private Vector3 _direction; //움직일 방향
        private float _c = 1f;
        [SerializeField] private float _defaultSpeed = 5; //속도 -> 나중에 캐릭터 데이터에서 받아오도록 변경 필요
        

        public Entity Owner { get; private set; }
        public Transform TargetTransform => _sensor == null ? null : _sensor.CurrentTarget.Transform;
        public bool IsCanMove { get; set; } = true;
        public bool IsExistTarget => _sensor == null ? false : _sensor.IsExistTarget;
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
            CurrentSpeed = _defaultSpeed * _c;
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
            if(IsExistTarget) //타겟이 존재할때
            {                
                Vector3 dir = (TargetTransform.position - _character.transform.position).normalized;
                dir.y = 0f;
                if (dir == Vector3.zero) return;
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                _character.transform.rotation = Quaternion.Slerp(_character.transform.rotation, lookRotation, Time.deltaTime * 10f);
            }
            else //타겟이 없을때
            {
                if (_direction == Vector3.zero) return;
                Quaternion lookRotation = Quaternion.LookRotation(_direction);
                _character.transform.rotation = Quaternion.Slerp(_character.transform.rotation, lookRotation, Time.deltaTime * 10f);
            }
        }

        private void Move()
        {
            Vector3 moveVector = _direction * CurrentSpeed * CurrentSpeedMultiplier;
            if (!IsCanMove)
            {
                moveVector = Vector3.zero;
                _rbCompo.freezeRotation = false;
            }
            else
            {
                _rbCompo.freezeRotation = true;
            }

            //moveVector.y += -9.8f;

            _rbCompo.linearVelocity = moveVector;
        }

        public void SetMultiplier(float value = 1f) => CurrentSpeedMultiplier = value;

        #endregion 
    }
}
