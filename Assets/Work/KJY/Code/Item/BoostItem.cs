using System;
using UnityEngine;
using DG.Tweening;

namespace Work.KJY.Code.Item
{
    public class BoostItem : MonoBehaviour
    {
        [SerializeField] private float rotSpeed;
        [SerializeField] private float floatingHeight = 0.5f;
        [SerializeField] private float floatingDuration = 1f;
        [SerializeField] private float defaultYPos;
        
        private float rotValue;
        private bool _isMoving = false;

        private void Start()
        {
            transform.position = new Vector3(transform.position.x, defaultYPos, transform.position.z);
            StartFloatingAnimation();
        }
        
        private void Update()
        {
            ItemRotAndPos();
        }

        private void ItemRotAndPos()
        {
            Quaternion rotation = transform.rotation;
            rotValue += rotSpeed * Time.deltaTime;
            rotValue %= 360;
            rotation.eulerAngles = new Vector3(0f, rotValue, 0f);
            
            transform.rotation = rotation;
        }

        private void StartFloatingAnimation()
        {
            if (_isMoving) return;

            _isMoving = true;
            transform.DOMoveY(defaultYPos + floatingHeight, floatingDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .OnKill(() => _isMoving = false);
        }
    }
}