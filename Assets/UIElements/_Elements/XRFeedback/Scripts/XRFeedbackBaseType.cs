using UnityEngine;

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
            ConfigureNearFeedback();
        if (runOnTouch)
            ConfigureTouchFeedback();
        if (runOnSelect)
            ConfigureSelectFeedback();
    }

    protected abstract void ConfigureNearFeedback();
    protected abstract void ConfigureTouchFeedback();
    protected abstract void ConfigureSelectFeedback();
}
