using NaughtyAttributes;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRUI_2DButtonSprite : XRUI_ButtonBase
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

    public GameObject distanceCollider;
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

        xrFeedback.onTouchEnter.AddListener((XRController controller) => { onClickDown?.Invoke(); });
        xrFeedback.onTouchExit.AddListener((XRController controller) => { onClickUp?.Invoke(); });
    }
    private void Start()
    {
        if (xrFeedback.allowDistanceEvents)
        {
            XRBaseInteractable interactable = gameObject.GetComponent<XRBaseInteractable>();
            if (interactable == null)
                interactable = gameObject.GetComponentInChildren<XRBaseInteractable>();

            if (interactable != null)
                interactable.onSelectEnter.AddListener((XRBaseInteractor) => { onClickDown?.Invoke(); });
        }
        else if (distanceCollider != null)
        {
            distanceCollider.SetActive(false);
        }
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
}
