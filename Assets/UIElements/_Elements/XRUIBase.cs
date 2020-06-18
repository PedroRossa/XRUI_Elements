using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRUIBase : MonoBehaviour
{
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
    }

    protected virtual void Awake()
    {
        ConfigureSound();
    }

    private void ConfigureSound()
    {
        if (playSound && soundClick != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = soundClick;
        }
    }

    private void OnTouchEnterFunction(XRController controller)
    {
        isTouch = true;

        if (playSound && soundClick != null)
            audioSource.Play();

        if (useHaptics)
            controller.SendHapticImpulse(hapticsIntensity, hapticsDuration);

        //Call event if exists
        onTouchEnter?.Invoke();
    }

    private void OnTouchExitFunction(XRController controller)
    {
        isTouch = false;

        //Call event if exists
        onTouchExit?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isEnabled)
            return;

        XRController controller = other.GetComponent<XRController>();
        if (controller != null)
            OnTouchEnterFunction(controller);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isEnabled)
            return;

        if (isTouch)
        {
            //Call event if exists
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
            OnTouchExitFunction(controller);
        }
    }
}
