using NaughtyAttributes;
using UnityEngine;

public class XR2DSpriteButton : XR2DButton
{
    public SpriteRenderer iconSprite;

    [Header("Sprite Properties")]
    [ShowAssetPreview(32, 32)]
    public Sprite icon;
    [ShowIf("HasIconSet")]
    [Range(0.001f, 1.0f)]
    public float iconScale = 0.1f;

    [ShowIf("backgroundFeedback")]
    public Color iconColor = Color.black;
    [ShowIf("backgroundFeedback")]
    public Color iconDisabledColor = Color.gray;

    public bool HasIconSet() { return icon != null ? true : false; }

    protected override void OnValidate()
    {
        base.OnValidate();

        if (iconSprite != null)
        {
            if (icon != null)
            {
                iconSprite.sprite = icon;
                iconSprite.transform.localScale = Vector3.one * iconScale;
            }
            if (backgroundFeedback)
            {
                iconSprite.color = isEnabled ? iconColor : iconDisabledColor;
                backgroundSprite.color = isEnabled ? uiColors.normalColor : uiColors.disabledColor;
            }
            else
            {
                iconSprite.color = isEnabled ? uiColors.normalColor : uiColors.disabledColor;
                backgroundSprite.color = backgroundColor;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        touchManagement.onTouchEnter.AddListener(OnTouchEnterFunction);
        touchManagement.onTouchExit.AddListener(OnTouchExitFunction);
    }

    protected void OnTouchEnterFunction()
    {
        //Aplly on parent because it's a empty object with normalized scale
        iconSprite.transform.parent.localScale = Vector3.one * 1.1f;
        if (backgroundFeedback)
            backgroundSprite.color = uiColors.touchColor;
        else
            iconSprite.color = uiColors.touchColor;
    }

    protected void OnTouchExitFunction()
    {
        //Aplly on parent because it's a empty object with normalized scale
        iconSprite.transform.parent.localScale = Vector3.one;
        if (backgroundFeedback)
            backgroundSprite.color = uiColors.normalColor;
        else
            iconSprite.color = uiColors.normalColor;
    }
}
