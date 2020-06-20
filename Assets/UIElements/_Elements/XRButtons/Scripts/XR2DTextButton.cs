using TMPro;
using UnityEngine;

public class XR2DTextButton : XR2DButton
{
    [Header("Text Properties")]
    public string txtValue;
    public int fontSize = 20;

    private TextMeshPro tmpField;

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

        backgroundSprite.color = isEnabled ? uiColors.normalColor : uiColors.disabledColor;
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

        touchManagement.onTouchEnter.AddListener(() => { backgroundSprite.color = uiColors.touchColor; });
        touchManagement.onTouchExit.AddListener(() => { backgroundSprite.color = uiColors.normalColor; });

        if (backgroundSprite != null)
            backgroundSprite.color = backgroundColor;
    }
}
