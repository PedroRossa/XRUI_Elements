using TMPro;
using UnityEngine;

public class XR2DTextButton : XR2DButton
{
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
        if (tmpField == null)
            tmpField = GetComponentInChildren<TextMeshPro>();

        tmpField.text = txtValue;
        tmpField.fontSize = fontSize;

        onTouchEnter.AddListener(OnTouchEnterFunction);
        onTouchExit.AddListener(OnTouchExitFunction);
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
