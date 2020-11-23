using NaughtyAttributes;
using System.Collections;
using UnityEngine;

/// <summary>
/// XRUI 2DButton identified by a Sprite
/// </summary>
public class XRUI_2DButtonSprite : XRUI_2DButtonBase
{
    public bool backgroundFeedback;
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

    protected override void Initialize()
    {
        SetSpriteColors();
    }
    private void SetSpriteColors()
    {
        if (backgroundFeedback)
        {
            xrUIColors.target = backgroundSprite.transform;
            iconSprite.color = isEnabled ? iconColor : iconDisabledColor;
            backgroundSprite.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
        }
        else
        {
            xrUIColors.target = iconSprite.transform;
            iconSprite.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
            backgroundSprite.color = isEnabled ? backgroundColor : backgroundDisabledColor;
        }
    }

    protected override void EventConfiguration()
    {
        onClickDown.AddListener(
            () =>
            {
                ButtonsListener(this);
            }
        );
    }

    protected void ButtonsListener(XRUI_ButtonBase currentButton)
    {
        //If the button is enabled and it can be active, then it toggle between xrUIColors.selectColor and xrUI.normalColor
        //The others enabled buttons will be set to xrUI.normalColor
        if (currentButton.canActiveButton)
        {
            if (currentButton.isEnabled)
            {
                StartCoroutine(ButtonsListenerCoroutine(currentButton));
            }
        }
    }

    protected IEnumerator ButtonsListenerCoroutine(XRUI_ButtonBase currentButton)
    {
        //I HATE parallel programming ;-;
        XRUI_3DButtonSprite xRUI_3DButtonSprite = currentButton as XRUI_3DButtonSprite;
        if (xRUI_3DButtonSprite)
        {
            while (xRUI_3DButtonSprite.isMoving)
                yield return null;
        }

        currentButton.isOn = !currentButton.isOn;

        if (xRUI_3DButtonSprite)
        {
            if (xRUI_3DButtonSprite.backgroundFeedback)
                xRUI_3DButtonSprite.buttonMesh.sharedMaterial.color =
                    xRUI_3DButtonSprite.isOn ? xRUI_3DButtonSprite.xrUIColors.selectColor : xRUI_3DButtonSprite.xrUIColors.normalColor;
            else
                xRUI_3DButtonSprite.iconSprite.color =
                    xRUI_3DButtonSprite.isOn ? xRUI_3DButtonSprite.xrUIColors.selectColor : xRUI_3DButtonSprite.xrUIColors.normalColor;

            if (xRUI_3DButtonSprite.feedbackColor != null)
                xRUI_3DButtonSprite.feedbackColor.memoryColor = xRUI_3DButtonSprite.buttonMesh.sharedMaterial.color;
        }
        else
        {
            XRUI_2DButtonSprite xRUI_2DButtonSprite = currentButton as XRUI_2DButtonSprite;
            if (xRUI_2DButtonSprite)
            {
                if (xRUI_2DButtonSprite.backgroundFeedback)
                    xRUI_2DButtonSprite.backgroundSprite.color =
                        xRUI_2DButtonSprite.isOn ? xRUI_2DButtonSprite.xrUIColors.selectColor :
                        xRUI_2DButtonSprite.xrUIColors.normalColor;
                else
                    iconSprite.color = xRUI_2DButtonSprite.isOn ? xrUIColors.selectColor : xrUIColors.normalColor;
            }

        }

        foreach (var button in currentButton.buttonsToDisableOnClickUp)
        {
            if (button.isEnabled)
            {
                if (button as XRUI_2DButtonBase)
                    (button as XRUI_2DButtonBase).backgroundSprite.color = xrUIColors.normalColor;
                else
                {
                    (button as XRUI_3DButtonBase).buttonMesh.sharedMaterial.color = xrUIColors.normalColor;

                    XRUI_3DButtonSprite button3dSprite = (button as XRUI_3DButtonSprite);
                    if (button3dSprite)
                    {
                        if (button3dSprite.feedbackColor != null)
                        {
                            button3dSprite.feedbackColor.memoryColor = button3dSprite.buttonMesh.sharedMaterial.color;
                        }
                    }
                }

                button.isOn = false;
            }
        }
    }
}
