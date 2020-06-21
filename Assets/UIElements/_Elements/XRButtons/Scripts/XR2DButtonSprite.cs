using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;

public class XR2DButtonSprite : XR2DButtonBase
{
    public bool backgroundFeedback;
    public SpriteRenderer backgroundSprite;
    [HideIf("backgroundFeedback")]
    public Color backgroundColor = Color.white;
    [HideIf("backgroundFeedback")]
    public Color backgroundDisabledColor = Color.white;

    [Header("Icon Properties")]
    public SpriteRenderer iconSprite;
    [ShowIf("backgroundFeedback")] 
    public Color iconColor = Color.white;
    [ShowIf("backgroundFeedback")]
    public Color iconDisabledColor = Color.white;
    [ShowAssetPreview(32, 32)]
    public Sprite icon;

    [ShowIf("HasIconSet")]
    [Range(0.001f, 1.0f)]
    public float iconScale = 0.1f;

    public bool HasIconSet() { return icon != null ? true : false; }

    protected override void OnValidate()
    {
        base.OnValidate();

        if (iconSprite == null || icon == null)
            return;

        iconSprite.transform.localScale = Vector3.one * iconScale;
        iconSprite.sprite = icon;

        SetSpriteColors();
    }

    protected override void Awake()
    {
        base.Awake();
        SetSpriteColors();
    }

    private void SetSpriteColors()
    {
        if (backgroundFeedback)
        {
            iconSprite.color = isEnabled ? iconColor : iconDisabledColor;
            backgroundSprite.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
            xrUIColors.target = backgroundSprite.transform;
        }
        else
        {
            iconSprite.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
            backgroundSprite.color = isEnabled ? backgroundColor : backgroundDisabledColor; 
            xrUIColors.target = iconSprite.transform;
        }
    }
}
