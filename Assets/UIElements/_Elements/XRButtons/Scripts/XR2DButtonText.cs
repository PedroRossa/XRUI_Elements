using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class XR2DButtonText : XR2DButtonBase
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
