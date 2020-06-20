using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRNearDetectionLocked
{
    public class XRNearDetection : MonoBehaviour
    {
        private XRFeedback feedbackBase;
        private bool isNear;
        public bool IsNear { get => isNear; }

        private void Awake()
        {
            feedbackBase = GetComponentInParent<XRFeedback>();

            if (!feedbackBase)
                throw new System.Exception("Don't have Feedbackbase");
        }

        void OnTriggerEnter(Collider other)
        {
            XRController controller = other.GetComponent<XRController>();
            if (controller != null && !IsNear)
            {
                isNear = true;
                feedbackBase.onNearEnter?.Invoke(controller);

            }
        }

        void OnTriggerExit(Collider other)
        {
            XRController controller = other.GetComponent<XRController>();
            if (controller != null && IsNear)
            {
                isNear = false;
                feedbackBase.onNearExit?.Invoke(controller);
            }
        }
    }
}

