using TMPro;
using UnityEngine;

/// <summary>
/// XRUI 2DButton identified by Text
/// </summary>
public class XRUI_2DButtonText : XRUI_2DButtonBase
{
    [Header("Text Properties")]
    public string txtValue;
    public int fontSize = 20;
    protected TextMeshPro tmpField;

    protected override void OnValidate()
    {
        base.OnValidate();
        Initialize();

        if (xrFeedback.XRInteractable != null)
            xrFeedback.XRInteractable.onSelectEnter.AddListener(
                (XRBaseInteractor) =>
                {
                    if (!xrFeedback.isEnabled)
                        return;

                    onClickDown?.Invoke();
                }
            );
    }

    protected override void Initialize()
    {
        if (tmpField == null)
            tmpField = GetComponentInChildren<TextMeshPro>();

        tmpField.text = txtValue;
        tmpField.fontSize = fontSize;

        backgroundSprite.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
    }
}
