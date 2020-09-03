using NaughtyAttributes;
using System.Threading;
using UnityEngine.Events;

public class XRUI_ButtonBase : XRUI_Base
{
    public UnityEvent onClickDown;
    public UnityEvent onClickUp;

    /// <summary>
    /// Time in seconds to wait to validate click next time
    /// </summary>
    public int milissecondsToValidateClick = 500;

    /// <summary>
    /// Thread to count time to wait to validate click
    /// </summary>
    protected static Thread timer;
    /// <summary>
    /// Can I active the button action?
    /// </summary>
    protected bool canActiveButton = true;

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

    protected void Start()
    {
        timer = new Thread(TimerTick);
        timer.Start();
    }

    [Button]
    public void OnClickDown()
    {
        onClickDown.Invoke();
    }
    [Button]
    public void OnClickUp()
    {
        onClickUp.Invoke();
    }

    private void TimerTick()
    {
        while (true)
        {
            if (!canActiveButton)
            {
                Thread.Sleep(milissecondsToValidateClick);
                canActiveButton = true;
            }
        }
    }
}
