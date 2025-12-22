using UnityEngine;

namespace Work.KJY.Code.Item
{
    public class BoostChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayer;
        
        private void OnTriggerEnter(Collider other)
        {
            if ((playerLayer & (1 << other.gameObject.layer)) != 0)
            {
                Debug.Log("asd");
            }
        }
    }
}