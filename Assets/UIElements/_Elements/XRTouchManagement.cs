using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class XRTouchManagement : MonoBehaviour
{
    public bool playSound = true;
    [ShowIf("playSound")]
    public AudioClip soundClip;

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

    public void ConfigureSound(AudioSource audioSource)
    {
        this.audioSource = audioSource;
        if (playSound && soundClip != null)
        {
            audioSource.playOnAwake = false;
            audioSource.clip = soundClip;
        }
    }

    private void OnTouchEnterFunction(XRController controller)
    {
        isTouch = true;

        if (playSound && soundClip != null && audioSource != null)
            audioSource.Play();

        if (useHaptics)
            controller.SendHapticImpulse(hapticsIntensity, hapticsDuration);

        //Call event if exists
        onTouchEnter?.Invoke();
    }

    private void OnTouchExitFunction()
    {
        isTouch = false;

        //Call event if exists
        onTouchExit?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        XRController controller = other.GetComponent<XRController>();
        if (controller != null)
            OnTouchEnterFunction(controller);
    }

    private void OnTriggerStay(Collider other)
    {
        if (isTouch)
        {
            //Call event if exists
            onTouchStay?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        XRController controller = other.GetComponent<XRController>();
        if (controller != null)
            OnTouchExitFunction();
    }
}
