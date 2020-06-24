using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRUI_Feedback))]
public abstract class XRUI_FeedbackBaseType : MonoBehaviour
{
    public bool runOnNear = true;
    public bool runOnTouch = true;
    public bool runOnSelect = true;

    protected XRUI_Feedback xrFeedback;

    protected virtual void Awake()
    {
        xrFeedback = GetComponent<XRUI_Feedback>();

        if (!xrFeedback.isEnabled)
            return;

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
