using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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
    }

    protected virtual void Start()
    {
        if (xrFeedback.XRInteractable != null)
            xrFeedback.XRInteractable.onSelectEnter.AddListener((XRBaseInteractor) => { onClickDown?.Invoke(); });
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
