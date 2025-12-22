using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace Work.Utils.Feedbacks
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class CameraImpulsFeedback : Feedback
    {
        private CinemachineImpulseSource impulseSource;
        [SerializeField] private float force = 0.1f;
        private void Awake()
        {
            impulseSource = GetComponent<CinemachineImpulseSource>();
        }
        public override void CreateFeedback()
        {
            impulseSource.GenerateImpulse(force);
        }

        public override void StopFeedback()
        {
            //impulseSource.
        }
    }
}
