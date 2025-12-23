using System.Collections.Generic;
using UnityEngine;
using Work.Characters.Events;
using Work.Entities;
using Work.Entities.Code;
using Work.Utils.EventBus;

namespace Work.Characters.Code
{
    public class DetectSensorCompo : MonoBehaviour, IEntityComponent
    {

        public Entity Owner { get; private set; }
        public IDamageable CurrentTarget { get; private set; } //나중에 다른걸로 바뀔 가능성 있음 , 타겟이 존재하는지와 같은 검사는 모두 얘를 통해서 이루어짐
        public bool IsExistTarget => CurrentTarget != null && CurrentTarget.Transform != null && CurrentTarget.Transform.gameObject != null;

        public delegate void OnTargetChange(IDamageable currentTarget, IDamageable prev);
        public event OnTargetChange OnTargetChangedEvent;

        private List<IDamageable> _inRangeTarget = new List<IDamageable>();
        private SphereCollider _area;
        private float _detectionRadius = 5f;

        public void InitCompo(Entity entity)
        {
            Owner = entity;
            _detectionRadius = Owner.EntityDataSO.InteractionRange; //여기서 나중에는 실질적인 수치로 바꿔줘야함

            _area = GetComponent<SphereCollider>();
            _area.radius = _detectionRadius;


            Bus<CharacterMoveEvent>.Events += HandleMoveEvent;
        }

        private void OnDestroy()
        {
            Bus<CharacterMoveEvent>.Events -= HandleMoveEvent;
        }

        private void HandleMoveEvent(CharacterMoveEvent evt)
        {
            SetTarget();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                if (!_inRangeTarget.Contains(damageable))
                {
                    _inRangeTarget.Add(damageable);
                    damageable.OnDeadEvent += HandleObjectDeadEvent;
                    SetTarget();
                }
            }
        }

        private void HandleObjectDeadEvent(IDamageable damageable)
        {
            //if (_inRangeTarget.Contains(damageable)) { _inRangeTarget.Remove(damageable); }
            damageable.OnDeadEvent -= HandleObjectDeadEvent;
            SetTarget();
        }

        private void SetTarget()
        {
            if (_inRangeTarget.Count == 0 && CurrentTarget == null) return;
            RefreshTargets();

            IDamageable prev = CurrentTarget;


            for (int i = 0; i < _inRangeTarget.Count; i++)
            {
                if (CurrentTarget == null)
                {
                    CurrentTarget = _inRangeTarget[i];
                    continue;
                }

                bool isRangeOut = Vector3.Distance(CurrentTarget.Transform.position, Owner.Transform.position) > _detectionRadius - 1f;
                bool isNear = Vector3.Distance(CurrentTarget.Transform.position, Owner.Transform.position) > Vector3.Distance(_inRangeTarget[i].Transform.position, Owner.Transform.position);

                if (isRangeOut && isNear)
                {
                    CurrentTarget = _inRangeTarget[i];
                }
            }

            if (prev != CurrentTarget)
                OnTargetChangedEvent?.Invoke(CurrentTarget, prev);
        }

        private void RefreshTargets()
        {
            if (CurrentTarget != null && (CurrentTarget.Transform == null || CurrentTarget.Transform.gameObject == null || CurrentTarget.IsDead))
            {
                CurrentTarget = null;
                OnTargetChangedEvent?.Invoke(CurrentTarget, null);
            }

            int cnt = _inRangeTarget.Count;
            for (int i = cnt - 1; i >= 0; i--)
            {
                if (_inRangeTarget[i] == null || _inRangeTarget[i].Transform == null || _inRangeTarget[i].Transform.gameObject == null || _inRangeTarget[i].IsDead)
                {
                    _inRangeTarget.RemoveAt(i);
                }
            }

            if (_inRangeTarget.Count == 0)
            {
                CurrentTarget = null;
                OnTargetChangedEvent?.Invoke(CurrentTarget, null);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                if (_inRangeTarget.Contains(damageable))
                {
                    _inRangeTarget.Remove(damageable);
                    damageable.OnDeadEvent -= HandleObjectDeadEvent;
                    SetTarget();
                }
            }
        }
    }
}
