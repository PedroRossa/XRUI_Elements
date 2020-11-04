using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRUI_NearDetectionLocked
{
    /// <summary>
    /// Class to configurate a near detection in a XRUI element
    /// </summary>
    public class XRUI_NearDetection : MonoBehaviour
    {
        private XRUI_Feedback feedbackBase;
        private bool isNear;
        public bool IsNear { get => isNear; }

        private void Awake()
        {
            feedbackBase = GetComponentInParent<XRUI_Feedback>();

            if (!feedbackBase)
                throw new System.Exception("Don't have Feedbackbase");
        }

        void OnTriggerEnter(Collider other)
        {
            XRController controller = other.GetComponent<XRController>();

            if (controller == null)
                controller = other.GetComponentInParent<XRController>();

            if (controller != null && !IsNear)
            {
                isNear = true;
                feedbackBase.onNearEnter?.Invoke(controller);
            }
        }

        void OnTriggerExit(Collider other)
        {
            XRController controller = other.GetComponent<XRController>();

            if (controller == null)
                controller = other.GetComponentInParent<XRController>();

            if (controller != null && IsNear)
            {
                isNear = false;
                feedbackBase.onNearExit?.Invoke(controller);
            }
        }

        public void OnEnterAction(XRBaseInteractor xRBase)
        {
            XRController controller = xRBase.GetComponent<XRController>();
            if (controller != null)
            {
                isNear = true;
                feedbackBase.onNearEnter?.Invoke(controller);
            }
        }
        public void OnExitAction(XRBaseInteractor xRBase)
        {
            XRController controller = xRBase.GetComponent<XRController>();
            if (controller != null && IsNear)
            {
                isNear = false;
                feedbackBase.onNearExit?.Invoke(controller);
            }
        }
    }
}

