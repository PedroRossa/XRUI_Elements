using NaughtyAttributes;
using UnityEngine;

public class XR2DSpriteButton : XR2DButton
{
    [Header("Icon Properties")]
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

    private bool HasIconSet()
    {
        return icon != null ? true : false;
    }

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
                backgroundSprite.color = isEnabled ? normalColor : disabledColor;
            }
            else
                iconSprite.color = isEnabled ? normalColor : disabledColor;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        onTouchEnter.AddListener(OnTouchEnterFunction);
        onTouchExit.AddListener(OnTouchExitFunction);
    }

    private void OnTouchEnterFunction()
    {
        //Aplly on parent because it's a empty object with normalized scale
        iconSprite.transform.parent.localScale = Vector3.one * 1.1f;
        if (backgroundFeedback)
            backgroundSprite.color = touchColor;
        else
            iconSprite.color = touchColor;
    }

    private void OnTouchExitFunction()
    {
        //Aplly on parent because it's a empty object with normalized scale
        iconSprite.transform.parent.localScale = Vector3.one;
        if (backgroundFeedback)
            backgroundSprite.color = normalColor;
        else
            iconSprite.color = normalColor;
    }
}
