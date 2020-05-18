using NaughtyAttributes;
using System.Collections;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
public abstract class XRBaseFeedback : MonoBehaviour
{
    [Header("Feedback Properties")]
    public bool alphaColorByDistance = false;
    public bool useHapticFeedback = true;
    public bool playSound = true;

    [Header("Proximity Properties")]
    public Color proximityColor = new Color(0.75f, 1.0f, 0.75f, 1.0f);
    public Collider proximityCollider;
    [ShowIf("playSound")]
    public AudioClip proximitySound;
    [ReadOnly]
    public bool isInProximityArea;

    [Header("Haptic Properties")]
    [ShowIf("useHapticFeedback")]
    [Range(0.0f, 1.0f)]
    public float hapticAmplitude = 0.1f;
    [ShowIf("useHapticFeedback")]
    [Range(0.0f, 1.0f)]
    public float hapticProximityFrequence = 0.1f;


    public UnityEvent onProximityAreaEnter;
    public UnityEvent onProximityAreaStay;
    public UnityEvent onProximityAreaExit;

    protected Color originalColor;
    protected Vector3 onProximityEnterPos;
    protected Vector3 onProximityStayPos;

    private AudioSource audioSource;
    private XRBaseInteractor interactor;

    protected virtual void Awake()
    {
        if (playSound)
            ConfigureAudioSource();

        onProximityAreaEnter = new UnityEvent();
        onProximityAreaStay = new UnityEvent();
        onProximityAreaExit = new UnityEvent();
    }

    private void ConfigureAudioSource()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null || proximitySound == null)
        {
            playSound = false;
            return;
        }

        audioSource.clip = proximitySound;
        audioSource.playOnAwake = false;
    }

    public abstract void SetColor(Color color);

    protected virtual void UpdateColorByDistance()
    {
        Color alphaColor = originalColor;
        float distance = Vector3.Distance(onProximityStayPos, proximityCollider.bounds.center);
        float totalDistance = Vector3.Distance(Vector3.zero, proximityCollider.bounds.size);

        float normalizedDistance = 1.0f - Mathf.Clamp01(distance / totalDistance);
        alphaColor.a = normalizedDistance;

        SetColor(alphaColor);
    }

    private void OnProximityAreaEnterFunction()
    {
        onProximityEnterPos = interactor.GetComponent<Collider>().transform.position;

        if (!alphaColorByDistance)
            SetColor(proximityColor);

        if (playSound && proximitySound != null)
            audioSource.Play();

        //TODO: Deal here to send haptic on Ray Interactor to!
        if (useHapticFeedback)
            ((XRDirectInteractor)interactor).SendHapticImpulse(hapticAmplitude, hapticProximityFrequence);
    }

    private void OnProximityAreaStayFunction()
    {
        if (alphaColorByDistance)
        {
            onProximityStayPos = interactor.GetComponent<Collider>().transform.position;
            UpdateColorByDistance();
        }
    }

    private void OnProximityAreaExitFunction()
    {
        if (alphaColorByDistance)
            SetColor(Color.clear);
        else
            SetColor(originalColor);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("interactable"))
        {
            interactor = other.GetComponent<XRBaseInteractor>();
            isInProximityArea = true;
            OnProximityAreaEnterFunction();
            onProximityAreaEnter?.Invoke();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("interactable"))
        {
            if (isInProximityArea)
            {
                OnProximityAreaStayFunction();
                onProximityAreaStay?.Invoke();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("interactable"))
        {
            interactor = null;
            isInProximityArea = false;
            OnProximityAreaExitFunction();
            onProximityAreaExit?.Invoke();
        }
    }
}
