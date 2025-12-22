using System;
using UnityEngine;

namespace Work.KJY.Code.Test
{
    public class TestMove : MonoBehaviour
    {
        private void Update()
        {
            transform.position += Vector3.forward * Time.deltaTime;
        }
    }
}