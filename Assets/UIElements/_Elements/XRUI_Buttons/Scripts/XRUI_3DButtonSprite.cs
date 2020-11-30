using NaughtyAttributes;
using System.Collections;
using UnityEngine;

/// <summary>
/// XRUI 3DButton identified by a Sprite
/// </summary>
public class XRUI_3DButtonSprite : XRUI_3DButtonBase
{
    [Header("Feedback Properties")]
    public bool backgroundFeedback = true;

    [HideIf("backgroundFeedback")]
    public Color buttonColor = Color.white;
    [HideIf("backgroundFeedback")]
    public Color buttonDisabledColor = Color.white;

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

    /// <summary>
    /// Return if the button has an icon attached to it
    /// </summary>
    /// <returns></returns>
    public bool HasIconSet()
    {
        return icon != null;
    }

    [HideInInspector]
    public XRUI_FeedbackColor feedbackColor;

    protected override void OnValidate()
    {
        base.OnValidate();

        if (buttonMesh == null)
            buttonMesh = buttonObject.GetComponent<MeshRenderer>();

        buttonRigidBody = buttonObject.GetComponent<Rigidbody>();

        ConfigureButtonMaterial();
        SetColors();
    }

    protected override void Awake()
    {
        base.Awake();

        buttonMesh = buttonObject.GetComponent<MeshRenderer>();
        buttonRigidBody = buttonObject.GetComponent<Rigidbody>();

        ConfigureButtonMaterial();

        SetColors();
        try
        {
            feedbackColor = GetComponentInChildren<XRUI_FeedbackColor>();
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("Feedback color missing in: " + gameObject.name + "\nYou need to put a XRUI_FeedbackColor component" +
                " in a child game object");
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

    private void RefreshSprite()
    {
        if (iconSprite == null || icon == null)
            return;

        iconSprite.transform.localScale = Vector3.one * iconScale;
        iconSprite.sprite = icon;
    }

    /// <summary>
    /// Set the color of the button mesh after refresh the button sprite
    /// </summary>
    private void SetColors()
    {
        RefreshSprite();

        if (buttonMesh == null)
            return;

        buttonMesh.sharedMaterial = new Material(buttonMesh.sharedMaterial);

        if (backgroundFeedback)
        {
            xrUIColors.target = buttonMesh.transform;
            iconSprite.color = isEnabled ? iconColor : iconDisabledColor;
            buttonMesh.sharedMaterial.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
        }
        else
        {
            xrUIColors.target = iconSprite.transform;
            iconSprite.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
            buttonMesh.sharedMaterial.color = isEnabled ? buttonColor : buttonDisabledColor;
        }
    }

    protected override void ConfigureButtonMaterial()
    {
        if (buttonMesh == null)
            return;

        buttonMesh.sharedMaterial = new Material(buttonMaterial);

        if (backgroundFeedback)
            buttonMaterial.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
        else
            iconSprite.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;

        buttonMesh.material = buttonMaterial;
    }
}
