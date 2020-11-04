using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Base class of XRUI 2DButtons
/// </summary>
public class XRUI_2DButtonBase : XRUI_ButtonBase
{
    public SpriteRenderer backgroundSprite;

    protected override void Awake()
    {
        base.Awake();
        Initialize();

        xrFeedback.onTouchEnter.AddListener((XRController controller) => { onClickDown?.Invoke(); });
        xrFeedback.onTouchExit.AddListener((XRController controller) => { onClickUp?.Invoke(); });
        if (xrFeedback.XRInteractable != null)
            xrFeedback.XRInteractable.onSelectEnter.AddListener((XRBaseInteractor) => { onClickDown?.Invoke(); });
    }

    /// <summary>
    /// After the end of the frame, set that the button can't be actived and starts TimerTick() coroutine
    /// </summary>
    /// <returns></returns>
    public IEnumerator ResetCanActiveButton()
    {
        yield return new WaitForEndOfFrame();
        canActiveButton = false;
        StartCoroutine(TimerTick());
    }

    protected virtual void Initialize() { }
}
