using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class XR2DTextButton : XRUIBase
{
    public bool backgroundFeedback;
    public SpriteRenderer backgroundSprite;
    [HideIf("backgroundFeedback")]
    public Color32 backgroundColor = new Color32(255, 255, 255, 255);

    private TextMeshPro tmpField;

    [Header("Properties")]
    public string txtValue;
    public int fontSize = 20;

    protected override void OnValidate()
    {
        base.OnValidate();

        if (tmpField == null)
            tmpField = GetComponentInChildren<TextMeshPro>();

        //TODO: Discover how to set text color
        //tmpField.color = isEnabled ? normalColor : disabledColor;
        tmpField.text = txtValue;
        tmpField.fontSize = fontSize;

        //TODO: make a way to set feedback on text, current only on background
        backgroundFeedback = true;

        backgroundSprite.color = isEnabled ? normalColor : disabledColor;
    }

    protected override void Awake()
    {
        base.Awake();

        //TODO: make a way to set feedback on text, current only on background
        backgroundFeedback = true;

        if (tmpField == null)
            tmpField = GetComponentInChildren<TextMeshPro>();

        tmpField.text = txtValue;
        tmpField.fontSize = fontSize;

        onTouchEnter.AddListener(OnTouchEnterFunction);
        onTouchExit.AddListener(OnTouchExitFunction);

        if (backgroundSprite != null)
            backgroundSprite.color = backgroundColor;
    }

    private void OnTouchEnterFunction()
    {
        backgroundSprite.color = touchColor;
    }

    private void OnTouchExitFunction()
    {
        backgroundSprite.color = normalColor;
    }
}
