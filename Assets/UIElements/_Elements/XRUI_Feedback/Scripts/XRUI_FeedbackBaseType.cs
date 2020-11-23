using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Abstract class to implement events of OnTouch, OnNear and OnSelect based on input controllers
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

    protected virtual void Start()
    {
        if (runOnNear)
        {
            xrFeedback.onNearEnter.AddListener(
                (controller) =>
                {
                    if (!xrFeedback.isEnabled)
                        return;

                    OnNearEnterFeedbackFunction(controller);
                }
            );
            xrFeedback.onNearExit.AddListener(
                (controller) =>
                {
                    if (!xrFeedback.isEnabled)
                        return;

                    OnNearExitFeedbackFunction(controller);
                }
            );
            if (xrFeedback.AllowDistanceEvents)
            {
                if (xrFeedback.XRInteractable != null)
                {
                    xrFeedback.XRInteractable.onHoverEnter.AddListener(
                        (XRBaseInteractor) =>
                        {
                            if (!xrFeedback.isEnabled)
                                return;

                            OnNearEnterFeedbackFunction(XRBaseInteractor.GetComponent<XRController>());
                        }
                    );
                    xrFeedback.XRInteractable.onHoverExit.AddListener(
                        (XRBaseInteractor) =>
                        {
                            if (!xrFeedback.isEnabled)
                                return;

                            OnNearExitFeedbackFunction(XRBaseInteractor.GetComponent<XRController>());
                        }
                    );
                }
            }
        }

        if (runOnTouch)
        {
            xrFeedback.onTouchEnter.AddListener(
                (controller) =>
                {
                    if (!xrFeedback.isEnabled)
                        return;

                    OnTouchEnterFeedbackFunction(controller);
                }
            );
            xrFeedback.onTouchExit.AddListener(
                (controller) =>
                {
                    if (!xrFeedback.isEnabled)
                        return;

                    OnTouchExitFeedbackFunction(controller);
                }
            );
            if (xrFeedback.AllowDistanceEvents)
            {
                if (xrFeedback.XRInteractable != null)
                {
                    xrFeedback.XRInteractable.onHoverEnter.AddListener(
                        (XRBaseInteractor) =>
                        {
                            if (!xrFeedback.isEnabled)
                                return;

                            OnTouchEnterFeedbackFunction(XRBaseInteractor.GetComponent<XRController>());
                        }
                    );
                    xrFeedback.XRInteractable.onHoverExit.AddListener(
                        (XRBaseInteractor) =>
                        {
                            if (!xrFeedback.isEnabled)
                                return;

                            OnTouchExitFeedbackFunction(XRBaseInteractor.GetComponent<XRController>());
                        }
                    );
                }
            }
        }

        if (runOnSelect)
        {
            xrFeedback.onSelectEnter.AddListener(
                (controller) =>
                {
                    if (!xrFeedback.isEnabled)
                        return;

                    OnSelectEnterFeedbackFunction(controller);
                }
            );
            xrFeedback.onSelectExit.AddListener(
                (controller) =>
                {
                    if (!xrFeedback.isEnabled)
                        return;

                    OnSelectExitFeedbackFunction(controller);
                }
            );
            if (xrFeedback.AllowDistanceEvents)
            {
                if (xrFeedback.XRInteractable != null)
                {
                    xrFeedback.XRInteractable.onSelectEnter.AddListener(
                        (XRBaseInteractor) =>
                        {
                            if (!xrFeedback.isEnabled)
                                return;

                            OnSelectEnterFeedbackFunction(XRBaseInteractor.GetComponent<XRController>());
                        }
                    );
                    xrFeedback.XRInteractable.onSelectExit.AddListener(
                        (XRBaseInteractor) =>
                        {
                            if (!xrFeedback.isEnabled)
                                return;

                            OnSelectExitFeedbackFunction(XRBaseInteractor.GetComponent<XRController>());
                        }
                    );
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
