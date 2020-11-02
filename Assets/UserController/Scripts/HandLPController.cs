using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using InputManager;

namespace UserController
{
    public class HandLPController : MonoBehaviour
    {
        public Animator anim;

        public AxisHandler triggerHandler = null;
        public ButtonHandler ThumbTouch = null;
        public ButtonHandler GripTouch = null;
        public ButtonHandler TriggerTouch = null;

        private XRBaseControllerInteractor xRBaseController;
        private bool isOnInteractableEvent = false;
        private GrabbingType grabType = GrabbingType.HandGrab;

        void Start()
        {
            if (anim == null)
                anim = GetComponent<Animator>();

            xRBaseController = GetComponentInParent<XRBaseControllerInteractor>();
            if (xRBaseController != null)
            {
                xRBaseController.onHoverEnter.AddListener((XRBaseInteractor) => { isOnInteractableEvent = true; });
                xRBaseController.onHoverExit.AddListener((XRBaseInteractor) => { isOnInteractableEvent = false; });
                xRBaseController.onSelectEnter.AddListener((XRBaseInteractor) => { isOnInteractableEvent = true; });

            }

            triggerHandler.OnValueChange += OnTrigger;

            ThumbTouch.OnButtonDown += ThumbButtonDown;
            ThumbTouch.OnButtonUp += ThumbButtonUp;

            GripTouch.OnButtonDown += GripButtonDown;
            GripTouch.OnButtonUp += GripButtonUp;

            TriggerTouch.OnButtonDown += TriggerTouchButtonDown;
            TriggerTouch.OnButtonUp += TriggerTouchButtonUp;

        }
        private void ThumbButtonDown(XRController controller)
        {
            anim.SetBool("ThumbTouch", true);
        }
        private void ThumbButtonUp(XRController controller)
        {
            anim.SetBool("ThumbTouch", false);
        }
        private void GripButtonDown(XRController controller)
        {
            anim.SetBool("Grip", true);
        }
        private void GripButtonUp(XRController controller)
        {
            anim.SetBool("Grip", false);
        }
        private void TriggerTouchButtonDown(XRController controller)
        {
            if (!isOnInteractableEvent)
            {
                anim.SetBool("TriggerTouch", true);
                anim.SetFloat("Trigger", 0f);
            }
        }
        private void TriggerTouchButtonUp(XRController controller)
        {
            anim.SetBool("TriggerTouch", false);
            anim.SetFloat("TriggerFingerGrab", 0f);
            anim.SetFloat("TriggerHandGrab", 0f);
        }
        private void OnTrigger(XRController controller, float value)
        {
            if (!isOnInteractableEvent && value > 0.002f)
                return;

            float maximumValue = 0.2f;
            float normValue = Mathf.Clamp(value, 0f, maximumValue);

            switch (grabType)
            {
                case GrabbingType.None:
                    break;
                case GrabbingType.FingerGrab:
                        anim.SetFloat("TriggerFingerGrab", normValue);
                    break;
                case GrabbingType.HandGrab:
                    anim.SetFloat("TriggerHandGrab", normValue);
                    break;
                    
            }
        }

    }
}