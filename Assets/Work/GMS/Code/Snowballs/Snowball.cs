using System.Drawing;
using UnityEngine;

namespace Work.GMS.Code.Snowballs
{
    public class Snowball : MonoBehaviour
    {
        [SerializeField] private SphereCollider snowCollider;

        public SnowManager snowManager;
        public LayerMask groundLayer;

        [Header("Snow Ball")]
        public float checkDistance = 1.2f;
        private float currentRadius = 0.3f;
        Vector3 lastDigPosition;

        public void SetSnow(float currentSnowRadius)
        {
            snowCollider.transform.localScale = Vector3.one * currentSnowRadius;
            currentRadius = currentSnowRadius;
            ProcessSnow();
           // Debug.Log($"[Snowball] SetSnow: Radius = {currentSnowRadius}");
        }

        void ProcessSnow()
        {
            Ray ray = new Ray(transform.position, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, currentRadius + checkDistance, groundLayer))
            {
                if (!snowManager.HasSnow(hit))
                    return;

                // 너무 자주 파지 않도록 거리 제한
                if (Vector3.Distance(lastDigPosition, hit.point) < 0.3f)
                    return;

                lastDigPosition = hit.point;

                snowManager.RemoveAndDeformSnow(
                    hit,
                    currentRadius
                );
            }
        }

        float GetSlopeFactor(Vector3 normal)
        {
            float slope = Vector3.Dot(normal, Vector3.up);
            return Mathf.Clamp01(slope);
        }
    }
}