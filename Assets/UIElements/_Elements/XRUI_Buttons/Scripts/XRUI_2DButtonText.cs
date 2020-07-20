using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRUI_2DButtonText : XRUI_ButtonBase
{
    public SpriteRenderer backgroundSprite;
    [Header("Text Properties")]
    public string txtValue;
    public int fontSize = 20;

    private TextMeshPro tmpField;

    public GameObject distanceCollider;
    protected override void OnValidate()
    {
        base.OnValidate();
        Initialize();

    }

    protected override void Awake()
    {
        base.Awake();
        Initialize();

        xrFeedback.onTouchEnter.AddListener((XRController controller) => { onClickDown?.Invoke(); });
        xrFeedback.onTouchExit.AddListener((XRController controller) => { onClickUp?.Invoke(); });
    }
    private void Start()
    {
     //its here because, just to text awake happens before create interaction on feedback
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
    private void Initialize()
    {
        if (tmpField == null)
            tmpField = GetComponentInChildren<TextMeshPro>();

        tmpField.text = txtValue;
        tmpField.fontSize = fontSize;

        backgroundSprite.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
    }
}
