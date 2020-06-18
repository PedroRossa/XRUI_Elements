using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class XRProgressBarBase : MonoBehaviour
{
    [Header("Prefab References")]
    public Transform background;
    public Transform totalProgress;
    public Transform progressElement;
    public Transform progressPointElement;
    public TextMeshPro tmpProgress;

    public bool isEnabled = true;

    [Header("Color Properties")]
    public Color backgroundColor = Color.white;
    public Color32 totalProgressColor = new Color32(27, 140, 175, 255);

    [Header("State Colors")]
    public Color32 normalColor = new Color32(10, 198, 242, 255);
    public Color32 proximityColor = new Color32(27, 140, 175, 255);
    public Color32 selectColor = new Color32(17, 100, 128, 255);
    public Color32 disabledColor = new Color32(128, 128, 128, 255);

    public bool playSound = true;
    [ShowIf("playSound")]
    public AudioClip soundClick;

    public bool useHaptics = true;
    [ShowIf("useHaptics")]
    [Range(0, 1)]
    public float hapticsIntensity;
    [ShowIf("useHaptics")]
    [Range(0, 1)]
    public float hapticsDuration;

    [Header("Visualization Properties")]
    public bool showBackground = true;
    public bool showProgressPoint = true;
    public bool showProgressText = true;

    [Header("Progress")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float progress = 0.75f;
    public float Progress
    {
        get => progress;
        set => SetProgress(value);
    }

    [ReadOnly]
    public bool isTouch = false;

    public XRUIElements_Helper.UnityFloatEvent onProgressChange;

    private SpriteRenderer currentProgressSprite;
    private AudioSource audioSource;
    private XR2DDragInteractable dragInteractable;
    private XROutlineFeedback outlineFeedback;


    protected virtual void OnValidate()
    {
        SetElementsVisibility();
        UpdateProgress();
        UpdateColors();
    }

    protected virtual void Awake()
    {
        SetElementsVisibility();
        UpdateColors();

        dragInteractable = GetComponentInChildren<XR2DDragInteractable>();
        outlineFeedback = GetComponentInChildren<XROutlineFeedback>();

        if (dragInteractable != null)
        {
            dragInteractable.interactable.onHoverEnter.AddListener(WhenTouchStart);
            dragInteractable.interactable.onHoverExit.AddListener(WhenTouchEnd);
        }

        if (outlineFeedback != null)
            outlineFeedback.proximityColor = proximityColor;

        if (playSound && soundClick != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = soundClick;
        }

    }

    protected virtual void Update()
    {
        if (dragInteractable.isSelected)
        {
            SetProgress(dragInteractable.normalizedValue);
            UpdateProgress();
        }
    }

    private void SetElementsVisibility()
    {
        if (background != null)
            background.gameObject.SetActive(showBackground);

        if (progressPointElement != null)
            progressPointElement.gameObject.SetActive(showProgressPoint);

        if (tmpProgress != null)
            tmpProgress.gameObject.SetActive(showProgressText);
    }

    private void SetProgress(float value)
    {
        if (progress == value) return;

        //limit the value between 0 and 1
        progress = Mathf.Clamp01(value);

        UpdateProgress();
        onProgressChange?.Invoke(progress);
    }

    private void UpdateProgress()
    {
        if (progressElement != null)
        {
            if (currentProgressSprite == null)
                currentProgressSprite = progressElement.GetComponentInChildren<SpriteRenderer>();

            progressElement.localScale = new Vector3(Progress, 1, 1);
        }

        if (tmpProgress != null)
        {
            int currentProgress = (int)(Progress * 100);
            tmpProgress.text = currentProgress.ToString() + " %";
        }

        //Get child object that is containered between 0 - 1
        if (progressPointElement != null && progressPointElement.childCount > 0)
            progressPointElement.GetChild(0).transform.localPosition = new Vector3(Progress, 0, 0);
    }

    protected abstract void UpdateColors();

    protected virtual void WhenTouchStart(XRBaseInteractor interactor)
    {
        isTouch = true;

        if (playSound && soundClick != null)
            audioSource.Play();

        if (useHaptics)
        {
            XRController controller = interactor.GetComponent<XRController>();
            if (controller != null)
                controller.SendHapticImpulse(hapticsIntensity, hapticsDuration);
        }

        progressPointElement.transform.localScale *= 1.1f; //increase the scale in 10%
    }

    protected virtual void WhenTouchEnd(XRBaseInteractor interactor)
    {
        isTouch = false;

        progressPointElement.transform.localScale /= 1.1f; //decrease the scale in 10%
    }
}
