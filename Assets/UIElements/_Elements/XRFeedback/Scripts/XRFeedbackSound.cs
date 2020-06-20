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

    protected override void ConfigureNearFeedback()
    {
        if (nearEnterAudioClip != null)
        {
            xrFeedback.onNearEnter.AddListener((XRController controller) =>
            {
                audioSource.clip = nearEnterAudioClip;
                audioSource.Play();
            });
        }

        if (nearExitAudioClip != null)
        {
            xrFeedback.onNearExit.AddListener((XRController controller) =>
            {
                audioSource.clip = nearExitAudioClip;
                audioSource.Play();
            });
        }
    }

    protected override void ConfigureTouchFeedback()
    {
        if (touchEnterAudioClip != null)
        {
            xrFeedback.onTouchEnter.AddListener((XRController controller) =>
            {
                audioSource.clip = touchEnterAudioClip;
                audioSource.Play();
            });
        }

        if (touchExitAudioClip != null)
        {
            xrFeedback.onTouchExit.AddListener((XRController controller) =>
            {
                audioSource.clip = touchExitAudioClip;
                audioSource.Play();
            });
        }
    }

    protected override void ConfigureSelectFeedback()
    {
        if (selectEnterAudioClip != null)
        {
            xrFeedback.onSelectEnter.AddListener((XRController controller) =>
            {
                audioSource.clip = selectEnterAudioClip;
                audioSource.Play();
            });
        }

        if (selectExitAudioClip != null)
        {
            xrFeedback.onSelectExit.AddListener((XRController controller) =>
            {
                audioSource.clip = selectExitAudioClip;
                audioSource.Play();
            });
        }
    }
}
