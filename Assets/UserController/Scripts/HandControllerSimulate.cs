using System.Collections;
using UnityEngine;

namespace UserController
{
    [ExecuteInEditMode]
    public class HandControllerSimulate : MonoBehaviour
    {
        public Animator anim;
        public GrabbingType grabType = GrabbingType.None;
        [Range(0.0025f, 1f)]
        public float animationFrame = 1f;
        // Start is called before the first frame update
        void Start()
        {
            if (anim == null)
                anim = GetComponent<Animator>();

        }
        private void OnValidate()
        {
            anim.Update(Time.deltaTime);
        }
        private void Update()
        {
            float value = Mathf.Clamp(animationFrame, 0f, 1f);
            switch (grabType)
            {
                case GrabbingType.None:
                    anim.SetFloat("TriggerHandGrab", 0f);
                    anim.SetFloat("TriggerFingerGrab", 0f);
                    break;
                case GrabbingType.FingerGrab:
                    anim.SetFloat("TriggerFingerGrab", value);
                    anim.SetFloat("TriggerHandGrab", 0f);
                    break;
                case GrabbingType.HandGrab:
                    anim.SetFloat("TriggerHandGrab", value);
                    anim.SetFloat("TriggerFingerGrab", 0f);
                    break;
            }
        }
        private void OnDestroy()
        {
        }
        private IEnumerator Animation()
        {
            float value = Mathf.Clamp(animationFrame, 0f, 1f);
            switch (grabType)
            {
                case GrabbingType.None:
                    anim.SetFloat("TriggerHandGrab", 0f);
                    anim.SetFloat("TriggerFingerGrab", 0f);
                    break;
                case GrabbingType.FingerGrab:
                    anim.SetFloat("TriggerFingerGrab", value);
                    anim.SetFloat("TriggerHandGrab", 0f);
                    break;
                case GrabbingType.HandGrab:
                    anim.SetFloat("TriggerHandGrab", value);
                    anim.SetFloat("TriggerFingerGrab", 0f);
                    break;
            }
            yield return new WaitForSeconds(0.3f);

        }
        
    }
}