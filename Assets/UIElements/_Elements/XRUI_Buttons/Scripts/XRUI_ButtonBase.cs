using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class XRUI_ButtonBase : XRUI_Base
{
    /// <summary>
    /// On Click Down of the button callback
    /// </summary>
    public UnityEvent onClickDown;
    /// <summary>
    /// On Click Up of the button callback
    /// </summary>
    public UnityEvent onClickUp;

    /// <summary>
    /// Others buttons to deselect when this one is selected 
    /// </summary>
    public XRUI_ButtonBase[] buttonsToDisableOnClickUp;
    /// <summary>
    /// Is the button action triggered?
    /// </summary>
    [HideInInspector]
    public bool isOn;

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
    }

    protected void OnEnable()
    {
        canActiveButton = true;
    }

    /// <summary>
    /// Invoke on Click Down callback
    /// </summary>
    [Button]
    public void OnClickDown()
    {
        onClickDown.Invoke();
    }
    /// <summary>
    /// Invoke On Click Up callback
    /// </summary>
    [Button]
    public void OnClickUp()
    {
        onClickUp.Invoke();
    }
    /// <summary>
    /// After a constant time in seconds, set that the button can be active again
    /// </summary>
    /// <returns></returns>
    public IEnumerator TimerTick()
    {
        yield return new WaitForSeconds(secondsToValidateClick);
        canActiveButton = true;
    }
}
