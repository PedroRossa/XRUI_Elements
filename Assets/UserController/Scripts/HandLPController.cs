using System.Collections;
using System.Collections.Generic;
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
        // Start is called before the first frame update
        void Start()
        {
            if (anim == null)
                anim = GetComponent<Animator>();

            triggerHandler.OnValueChange += OnTrigger;

            ThumbTouch.OnButtonDown += ThumbButtonDown;
            ThumbTouch.OnButtonUp += ThumbButtonUp;

            GripTouch.OnButtonDown += GripButtonDown;
            GripTouch.OnButtonUp += GripButtonUp;

            TriggerTouch.OnButtonDown += TriggerTouchButtonDown;
            TriggerTouch.OnButtonUp += TriggerTouchButtonUp;
        }

        private void Update()
        {
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
            anim.SetBool("TriggerTouch", true);
        }
        private void TriggerTouchButtonUp(XRController controller)
        {
            anim.SetBool("TriggerTouch", false);
        }
        private void OnTrigger(XRController controller, float value)
        {
            anim.SetFloat("Trigger", value);
        }

    }
}