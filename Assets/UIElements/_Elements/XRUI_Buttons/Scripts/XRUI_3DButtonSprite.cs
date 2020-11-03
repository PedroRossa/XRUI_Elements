using NaughtyAttributes;
using UnityEngine;

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

    public bool HasIconSet()
    {
        return icon != null;
    }

    private XRUI_FeedbackColor feedbackColor;

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
        catch (System.NullReferenceException) {
            Debug.LogError("Feedback color missing in: " + gameObject.name + "\nYou need to put a XRUI_FeedbackColor component" +
                " in a child game object");
        }
    }

    protected override void EventConfiguration()
    {
        onClickDown.AddListener(
                () =>
                {
                    if (canActiveButton)
                    {
                        if (isEnabled)
                        {
                            isOn = !isOn;
                            if (backgroundFeedback)
                                buttonMesh.sharedMaterial.color = isOn ? xrUIColors.selectColor : xrUIColors.normalColor;
                            else
                                iconSprite.color = isOn ? xrUIColors.selectColor : xrUIColors.normalColor;

                            if (feedbackColor != null)
                                feedbackColor.memoryColor = buttonMesh.sharedMaterial.color;

                            foreach (var button in buttonsToDisableOnClickUp)
                            {
                                if (button.isEnabled)
                                {
                                    button.buttonMesh.sharedMaterial.color = xrUIColors.normalColor;
                                    button.isOn = false;
                                }
                            }
                        }
                    }
                });
    }

    private void RefreshSprite()
    {
        if (iconSprite == null || icon == null)
            return;

        iconSprite.transform.localScale = Vector3.one * iconScale;
        iconSprite.sprite = icon;
    }

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
