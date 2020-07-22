using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRUI_2DButtonBase : XRUI_ButtonBase
{
    public SpriteRenderer backgroundSprite;

    protected override void Awake()
    {
        base.Awake();
        Initialize();

        xrFeedback.onTouchEnter.AddListener((XRController controller) => { onClickDown?.Invoke(); });
        xrFeedback.onTouchExit.AddListener((XRController controller) => { onClickUp?.Invoke(); });
    }
    private void Start()
    {
        if (xrFeedback.XRInteractable != null)
            xrFeedback.XRInteractable.onSelectEnter.AddListener((XRBaseInteractor) => { onClickDown?.Invoke(); });
    }

    protected virtual void Initialize() { }
}
