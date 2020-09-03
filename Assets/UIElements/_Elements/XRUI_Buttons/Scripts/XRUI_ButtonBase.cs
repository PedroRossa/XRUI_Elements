using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class XRUI_ButtonBase : XRUI_Base
{
    public UnityEvent onClickDown;
    public UnityEvent onClickUp;

    /// <summary>
    /// Time in seconds to wait to validate click next time
    /// </summary>
    public float secondsToValidateClick = 0.2f;

    /// <summary>
    /// Can I active the button action?
    /// </summary>
    public bool canActiveButton = true;

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
        StartCoroutine(TimerTick());
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

    public IEnumerator TimerTick()
    {
        yield return new WaitForSeconds(secondsToValidateClick);
        canActiveButton = true;
    }
}
