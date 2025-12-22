using UnityEngine;

namespace Work.Utils.Feedbacks
{
    public abstract class Feedback : MonoBehaviour
    {
        public abstract void CreateFeedback();

        public abstract void StopFeedback();
    }
}