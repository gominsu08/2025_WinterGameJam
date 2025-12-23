using UnityEngine;

namespace Work.GMS.Code.Data
{
    public class DataContainer : MonoBehaviour
    {
        public static DataContainer Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private float _underSnowRadius = 0.3f;
        private float _upperSnowRadius = 0.3f;
        private float _gold = 0f;


        public float UnderSnowRadius => _underSnowRadius;
        public float UpperSnowRadius => _upperSnowRadius;
        public float Gold => _gold;

        public void SetRadius(float under, float upper)
        {
            _upperSnowRadius = upper;
            _underSnowRadius = under;
        }

        public void SetGold(float gold)
        {
            _gold = gold;
        }

        public Vector2 GetSnowmanSize()
        {
            return new Vector2(_upperSnowRadius, _underSnowRadius);
        }

    }
}