using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRUI_2DButtonText : XRUI_ButtonBase
{
    public SpriteRenderer backgroundSprite;
    [Header("Text Properties")]
    public string txtValue;
    public int fontSize = 20;

    private TextMeshPro tmpField;

    protected override void OnValidate()
    {
        base.OnValidate();
        Initialize();
    }

    protected override void Awake()
    {
        base.Awake();
        Initialize();

        xrFeedback.onTouchEnter.AddListener((XRController controller) => { onClickDown?.Invoke(); });
        xrFeedback.onTouchExit.AddListener((XRController controller) => { onClickUp?.Invoke(); });
    }

    private void Initialize()
    {
        if (tmpField == null)
            tmpField = GetComponentInChildren<TextMeshPro>();

        tmpField.text = txtValue;
        tmpField.fontSize = fontSize;

        backgroundSprite.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
    }
}
