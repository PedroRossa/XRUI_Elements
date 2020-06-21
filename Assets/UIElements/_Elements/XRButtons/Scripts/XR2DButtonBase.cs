using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XR2DButtonBase : XRUIBase
{
    public UnityEvent onClickDown;
    public UnityEvent onClickUp;

    protected override void OnValidate()
    {
        base.OnValidate();
        xrFeedback.isEnabled = isEnabled;
    }

    protected override void Awake()
    {
        base.Awake();
        xrFeedback.isEnabled = isEnabled;

        if (!isEnabled)
            return;

        xrFeedback.onTouchEnter.AddListener((XRController controller) => { onClickDown?.Invoke(); });
        xrFeedback.onTouchExit.AddListener((XRController controller) => { onClickUp?.Invoke(); });
    }
}
