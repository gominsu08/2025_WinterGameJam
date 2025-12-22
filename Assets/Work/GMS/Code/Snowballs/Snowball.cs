using DG.Tweening;
using UnityEngine;
using Work.Characters.Code;
using Work.Characters.FSM.Code;

namespace Work.GMS.Code.Snowballs
{
    public class Snowball : MonoBehaviour
    {
        [SerializeField] private SphereCollider snowCollider;
        [SerializeField] private ParticleSystem snowImpactEffect;
        public SnowManager snowManager;
        public LayerMask groundLayer, obstacleLayer;

        [Header("Snow Ball")]
        public float checkDistance = 1.2f;
        public float currentRadius = 0.3f;
        private Character _owner;
        private CharacterMovementCompo _mover;
        private StateCompo _stateCompo;
        Vector3 lastDigPosition;

        public void Init(Character character)
        {
            _owner = character;
            _mover = _owner.GetCompo<CharacterMovementCompo>();
            _stateCompo = _owner.GetCompo<StateCompo>();
        }

        public void SetSnow(float currentSnowRadius)
        {
            currentRadius = currentSnowRadius;
            snowCollider.transform.localScale = Vector3.one * currentRadius;
            transform.localPosition = new Vector3(0, (currentRadius / 2) - 0.776f, (currentRadius / 2));
            transform.Rotate((10 * _mover.CurrentSpeed + (currentRadius * 4)) * Time.deltaTime, 0, 0f);
            Ray ray = new Ray(transform.position, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, currentRadius + checkDistance, groundLayer))
            {
                // 너무 자주 파지 않도록 거리 제한
                if (Vector3.Distance(lastDigPosition, hit.point) < 0.15f)
                    return;

                lastDigPosition = hit.point;

                

                snowManager.RemoveAndDeformSnow(
                    hit,
                    currentRadius
                );
            }


            Debug.Log($"[Snowball] SetSnow: Radius = {currentSnowRadius}");
        }

        public bool IsOnSnow()
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, currentRadius + checkDistance, groundLayer))
            {
                return snowManager.HasSnow(hit);
            }
            return false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if ((obstacleLayer & (1 << collision.gameObject.layer)) != 0)
            {
                Knockback();
            }
        }

        [SerializeField] private float minRadius = 0.3f;
        [SerializeField] private float descount = 0.5f;

        public void Knockback()
        {
            _stateCompo.ChangeState("IDLE", true);
            _mover.SetCanMove(false);
            _stateCompo.SetCanStateChange(false);
            _mover.Knockback();
            SetSnow((currentRadius - descount) > minRadius ? (currentRadius - descount) : minRadius);
            ParticleSystem impact = Instantiate(snowImpactEffect, transform.position, Quaternion.identity);
            DOVirtual.DelayedCall(0.4f, () =>
            {
                //_mover.SetCanMove(true);
                _stateCompo.SetCanStateChange(true);
            });
        }
    }
}