using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class XR2DButton : MonoBehaviour
{
    public bool backgroundFeedback;
    public SpriteRenderer backgroundSprite;
    [HideIf("backgroundFeedback")]
    public Color32 backgroundColor = new Color32(255, 255, 255, 255);

    public bool isEnabled = true;

    [Header("State Colors")]
    public Color32 normalColor = new Color32(10, 198, 242, 255);
    public Color32 hoverColor = new Color32(27, 140, 175, 255);
    public Color32 touchColor = new Color32(17, 100, 128, 255);
    public Color32 disabledColor = new Color32(64, 64, 64, 255);

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

    [ReadOnly]
    public bool isTouch = false;

    public UnityEvent onTouchEnter;
    public UnityEvent onTouchStay;
    public UnityEvent onTouchExit;

    private AudioSource audioSource;

    protected virtual void OnValidate()
    {
        if (backgroundSprite != null)
            backgroundSprite.color = backgroundColor;
    }

    protected virtual void Awake()
    {
        if (playSound && soundClick != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = soundClick;
        }
    }

    private void WhenTouch(XRController controller)
    {
        if (playSound && soundClick != null)
            audioSource.Play();

        if (useHaptics)
            controller.SendHapticImpulse(hapticsIntensity, hapticsDuration);

        isTouch = true;
        onTouchEnter?.Invoke();
    }

    private void WhenTouchReleased()
    {
        isTouch = false;
        onTouchExit?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isEnabled)
            return;

        XRController controller = other.GetComponent<XRController>();
        if (controller != null)
        {
            WhenTouch(controller);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isEnabled)
            return;

        if (isTouch)
        {
            onTouchStay?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isEnabled)
            return;

        XRController controller = other.GetComponent<XRController>();
        if (controller != null)
        {
            WhenTouchReleased();
        }
    }
}
