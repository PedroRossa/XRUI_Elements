using UnityEngine.Events;

public class XRUI_ButtonBase : XRUI_Base
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
    }
}
