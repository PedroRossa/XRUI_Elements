using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class XRBaseFeedback : MonoBehaviour
{
    [Header("Feedback Properties")]

    //TODO: Make readonly until solve the problem of alpha calculation
    [NaughtyAttributes.ReadOnly]
    public bool alphaColorByDistance = false;
    public bool useHapticFeedback = true;
    public bool playSound = true;

    [Header("Proximity Properties")]
    public Color proximityColor = new Color(0.75f, 1.0f, 0.75f, 1.0f);
    [ShowIf("playSound")]
    public AudioClip proximitySound;
    [NaughtyAttributes.ReadOnly]
    public bool isInProximityArea;
    [NaughtyAttributes.ReadOnly]
    public Collider proximityCollider;
    [HideInInspector]
    public float proximityColliderSize = 1.5f;

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
    private XRBaseControllerInteractor interactor;

    protected virtual void Awake()
    {
        if (playSound)
            ConfigureAudioSource();

        onProximityAreaEnter = new UnityEvent();
        onProximityAreaStay = new UnityEvent();
        onProximityAreaExit = new UnityEvent();

        //if the feedback doesn't have a proximity collider configure a sphere
        if(proximityCollider == null)
            AutoconfigureColliderAsSphere();
    }

    public void DeleteProximityCollider()
    {
        if (proximityCollider == null) return;

        foreach (Collider item in GetComponents<Collider>())
        {
            //get the proximity collider and remove it
            if (proximityCollider.Equals(item))
            {
                DestroyImmediate(item);
                break;
            }
        }
    }

    [Button]
    public void AutoconfigureColliderAsSphere()
    {
        DeleteProximityCollider();

        proximityCollider = gameObject.AddComponent<SphereCollider>();
        proximityCollider.isTrigger = true;

        ((SphereCollider)proximityCollider).radius = proximityColliderSize;
    }

    [Button]
    public void AutoconfigureColliderAsBox()
    {
        DeleteProximityCollider();

        proximityCollider = gameObject.AddComponent<BoxCollider>();
        proximityCollider.isTrigger = true;

        ((BoxCollider)proximityCollider).size = Vector3.one * proximityColliderSize;
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

    //TODO: This function doesn't work properlly so it's disabled
    //It needs to calculate the alpha by distance of the interactor from the center of the proximitycollider;
    protected virtual void UpdateColorByDistance()
    {
        Color alphaColor = originalColor;

        //float currDistance = Vector3.Distance(onProximityStayPos, proximityCollider.bounds.center);

        //float maxDis = proximityCollider.bounds.size.magnitude / 2;
        //float lerpVal = Mathf.Clamp01(currDistance / maxDis);

        //alphaColor.a = lerpVal;

        SetColor(alphaColor);
    }

    private void OnProximityAreaEnterFunction()
    {
        onProximityEnterPos = interactor.GetComponent<Collider>().transform.position;

        if (!alphaColorByDistance)
            SetColor(proximityColor);

        if (playSound && proximitySound != null)
            audioSource.Play();

        if (useHapticFeedback)
           interactor.SendHapticImpulse(hapticAmplitude, hapticProximityFrequence);
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
        if (other.CompareTag("XRInteractor"))
        {
            interactor = other.GetComponent<XRBaseControllerInteractor>();
            isInProximityArea = true;
            OnProximityAreaEnterFunction();
            onProximityAreaEnter?.Invoke();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("XRInteractor"))
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
        if (other.CompareTag("XRInteractor"))
        {
            interactor = null;
            isInProximityArea = false;
            OnProximityAreaExitFunction();
            onProximityAreaExit?.Invoke();
        }
    }
}
