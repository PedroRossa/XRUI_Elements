using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Abstract class that modelate touch, near and select events
/// </summary>
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
    }

    private void Start()
    {
          if (!xrFeedback.isEnabled)
            return;

        if (runOnNear)
        {
            xrFeedback.onNearEnter.AddListener(OnNearEnterFeedbackFunction);
            xrFeedback.onNearExit.AddListener(OnNearExitFeedbackFunction);

            if (xrFeedback.AllowDistanceEvents)
            {
                if (xrFeedback.XRInteractable != null)
                {
                    xrFeedback.XRInteractable.onHoverEnter.AddListener((XRBaseInteractor) => { OnNearEnterFeedbackFunction(XRBaseInteractor.GetComponent<XRController>()); });
                    xrFeedback.XRInteractable.onHoverExit.AddListener((XRBaseInteractor) => { OnNearExitFeedbackFunction(XRBaseInteractor.GetComponent<XRController>()); });
                }
            }
        }

        if (runOnTouch)
        {
            xrFeedback.onTouchEnter.AddListener(OnTouchEnterFeedbackFunction);
            xrFeedback.onTouchExit.AddListener(OnTouchExitFeedbackFunction);
            if (xrFeedback.AllowDistanceEvents)
            {
                if (xrFeedback.XRInteractable != null)
                {
                    xrFeedback.XRInteractable.onHoverEnter.AddListener((XRBaseInteractor) => { OnTouchEnterFeedbackFunction(XRBaseInteractor.GetComponent<XRController>()); });
                    xrFeedback.XRInteractable.onHoverExit.AddListener((XRBaseInteractor) => { OnTouchExitFeedbackFunction(XRBaseInteractor.GetComponent<XRController>()); });
                }
            }
        }

        if (runOnSelect)
        {
            xrFeedback.onSelectEnter.AddListener(OnSelectEnterFeedbackFunction);
            xrFeedback.onSelectExit.AddListener(OnSelectExitFeedbackFunction);
            if (xrFeedback.AllowDistanceEvents)
            {
                if (xrFeedback.XRInteractable != null)
                {
                    xrFeedback.XRInteractable.onSelectEnter.AddListener((XRBaseInteractor) => { OnSelectEnterFeedbackFunction(XRBaseInteractor.GetComponent<XRController>()); });
                    xrFeedback.XRInteractable.onSelectExit.AddListener((XRBaseInteractor) => { OnSelectExitFeedbackFunction(XRBaseInteractor.GetComponent<XRController>()); });
                }
            }
        }
    }

    protected abstract void OnNearEnterFeedbackFunction(XRController controller);
    protected abstract void OnNearExitFeedbackFunction(XRController controller);

    protected abstract void OnTouchEnterFeedbackFunction(XRController controller);
    protected abstract void OnTouchExitFeedbackFunction(XRController controller);

    protected abstract void OnSelectEnterFeedbackFunction(XRController controller);
    protected abstract void OnSelectExitFeedbackFunction(XRController controller);
}
