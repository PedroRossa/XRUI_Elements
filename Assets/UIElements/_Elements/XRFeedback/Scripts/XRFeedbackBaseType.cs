using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRFeedback))]
public abstract class XRFeedbackBaseType : MonoBehaviour
{
    public bool runOnNear;
    public bool runOnTouch;
    public bool runOnSelect;

    protected XRFeedback xrFeedback;

    protected virtual void Awake()
    {
        xrFeedback = GetComponent<XRFeedback>();

        if (runOnNear)
        {
            xrFeedback.onNearEnter.AddListener(OnNearEnterFeedbackFunction);
            xrFeedback.onNearExit.AddListener(OnNearExitFeedbackFunction);
        }

        if (runOnTouch)
        {
            xrFeedback.onTouchEnter.AddListener(OnTouchEnterFeedbackFunction);
            xrFeedback.onTouchExit.AddListener(OnTouchExitFeedbackFunction);
        }

        if (runOnSelect)
        {
            xrFeedback.onSelectEnter.AddListener(OnSelectEnterFeedbackFunction);
            xrFeedback.onSelectExit.AddListener(OnSelectExitFeedbackFunction);
        }
    }

    protected abstract void OnNearEnterFeedbackFunction(XRController controller);
    protected abstract void OnNearExitFeedbackFunction(XRController controller);

    protected abstract void OnTouchEnterFeedbackFunction(XRController controller);
    protected abstract void OnTouchExitFeedbackFunction(XRController controller);

    protected abstract void OnSelectEnterFeedbackFunction(XRController controller);
    protected abstract void OnSelectExitFeedbackFunction(XRController controller);
}
