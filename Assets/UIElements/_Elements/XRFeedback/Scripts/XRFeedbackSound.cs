using NaughtyAttributes;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(AudioSource))]
public class XRFeedbackSound : XRFeedbackBaseType
{
    [ShowIf("runOnNear")]
    public AudioClip nearEnterAudioClip;
    [ShowIf("runOnNear")]
    public AudioClip nearExitAudioClip;

    [ShowIf("runOnTouch")]
    public AudioClip touchEnterAudioClip;
    [ShowIf("runOnTouch")]
    public AudioClip touchExitAudioClip;

    [ShowIf("runOnSelect")]
    public AudioClip selectEnterAudioClip;
    [ShowIf("runOnSelect")]
    public AudioClip selectExitAudioClip;

    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }


    protected override void OnNearEnterFeedbackFunction(XRController controller)
    {
        if (nearEnterAudioClip != null)
        {
            audioSource.clip = nearEnterAudioClip;
            audioSource.Play();
        }
    }

    protected override void OnNearExitFeedbackFunction(XRController controller)
    {
        if (nearExitAudioClip != null)
        {
            audioSource.clip = nearExitAudioClip;
            audioSource.Play();
        }
    }

    protected override void OnTouchEnterFeedbackFunction(XRController controller)
    {
        if (touchEnterAudioClip != null)
        {
            audioSource.clip = touchEnterAudioClip;
            audioSource.Play();
        }
    }

    protected override void OnTouchExitFeedbackFunction(XRController controller)
    {
        if (touchExitAudioClip != null)
        {
            audioSource.clip = touchExitAudioClip;
            audioSource.Play();
        }
    }

    protected override void OnSelectEnterFeedbackFunction(XRController controller)
    {
        if (selectEnterAudioClip != null)
        {
            audioSource.clip = selectEnterAudioClip;
            audioSource.Play();
        }
    }

    protected override void OnSelectExitFeedbackFunction(XRController controller)
    {
        if (selectExitAudioClip != null)
        {
            audioSource.clip = selectExitAudioClip;
            audioSource.Play();
        }
    }
}
