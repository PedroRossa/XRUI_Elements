using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
public class XRFeedback : MonoBehaviour
{
    private List<string> ElementTypes { get { return new List<string>() { "Mesh Renderer", "Sprite Renderer" }; } }
    private AudioSource audioSource;

    [Dropdown("ElementTypes")]
    public string elementType = "Mesh Renderer";

    [ShowIf("IsMeshRenderer")]
    public MeshRenderer meshFeedback;
    [ShowIf("IsSpriteRenderer")]
    public SpriteRenderer spriteFeedback;

    [ReadOnly]
    public Collider elementCollider;

    private bool IsMeshRenderer() { return elementType.Equals("Mesh Renderer"); }
    private bool IsSpriteRenderer() { return elementType.Equals("Sprite Renderer"); }

    [Header("Feedback Properties")]
    public bool alphaByDistance = true;
    public bool detectProximity = true;
    public bool useHapticFeedback = true;
    public bool playSound = true;

    [Header("Proximity Properties")]
    [ShowIf("detectProximity")]
    public Color proximityColor = new Color(0.75f, 1.0f, 0.75f, 1.0f);
    [ShowIf("detectProximity")]
    public float proximityScaleMultiplier = 1;
    [ShowIf("detectProximity")]
    public Collider proximityCollider;
    [ShowIf(EConditionOperator.And, "playSound", "detectProximity")]
    public AudioClip proximitySound;
    [ShowIf("detectProximity")]
    [ReadOnly]
    public bool isInProximityArea;

    [Header("Haptic Properties")]
    [ShowIf("useHapticFeedback")]
    [Range(0.0f, 1.0f)]
    public float hapticAmplitude = 0.15f;
    [ShowIf("useHapticFeedback")]
    [Range(0.0f, 5.0f)]
    public float hapticHoverFrequence = 0.75f;
    [ShowIf(EConditionOperator.And, "useHapticFeedback", "detectTouch")]
    [Range(0.0f, 5.0f)]
    public float hapticTouchFrequence = 0.25f;


    [ShowIf("detectProximity")]
    public UnityEvent onProximityAreaEnter;
    [ShowIf("detectProximity")]
    public UnityEvent onProximityAreaStay;
    [ShowIf("detectProximity")]
    public UnityEvent onProximityAreaExit;

    private Color originalColor;
    private Vector3 originalScale;
    private Vector3 onHoverEnterPos;

    private void Awake()
    {
        if (meshFeedback != null)
        {
            if (meshFeedback.sharedMaterial == null)
                meshFeedback.sharedMaterial = new Material(Shader.Find("Unlit/TransparentColor"));

            originalColor = meshFeedback.sharedMaterial.color;
            originalScale = meshFeedback.transform.localScale;
            elementCollider = meshFeedback.GetComponent<Collider>();
            meshFeedback.sharedMaterial = new Material(meshFeedback.sharedMaterial);
        }
        else if (spriteFeedback != null)
        {
            originalColor = spriteFeedback.color;
            originalScale = spriteFeedback.transform.localScale;
            elementCollider = spriteFeedback.GetComponent<Collider>();
        }

        if (alphaByDistance)
        {
            if (meshFeedback != null)
                meshFeedback.sharedMaterial = new Material(Shader.Find("Unlit/TransparentColor"));

            SetFeedback(Color.clear, 1);
        }

        if (playSound)
            ConfigureAudioSource();
    }

    private void ConfigureAudioSource()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            playSound = false;
            return;
        }

        audioSource.playOnAwake = false;
    }

    private void SetFeedback(Color color, float scaleMultiplier)
    {
        if (elementType.Equals("Mesh Renderer"))
        {
            if (meshFeedback != null)
            {
                meshFeedback.sharedMaterial.color = color;
                meshFeedback.transform.localScale = originalScale * scaleMultiplier;
            }
        }
        else if (elementType.Equals("Sprite Renderer"))
        {
            if (spriteFeedback != null)
            {
                spriteFeedback.color = color;
                spriteFeedback.transform.localScale = originalScale * scaleMultiplier;
            }
        }
    }

    private void ProximityUpdateByDistance(Transform interactable)
    {
        Color alphaColor = proximityColor;
        float maxDistance = Vector3.Distance(onHoverEnterPos, transform.position);
        float distance = Vector3.Distance(interactable.position, transform.position);

        float normalizedDistance = (1 / maxDistance * distance);
        alphaColor.a = 1 - normalizedDistance;

        SetFeedback(alphaColor, proximityScaleMultiplier);
    }


    private void HapticFeedback(Transform interactable, float amplitude, float repeatTime)
    {
        XRController controller = interactable.GetComponentInChildren<XRController>();
        if (controller == null)
            controller = interactable.GetComponentInParent<XRController>();

        if (controller == null)
            return;

        StopAllCoroutines();
        StartCoroutine(HapticCoroutine(controller.inputDevice, amplitude, repeatTime));
    }

    private void HapticFeedbackStop(Transform interactable)
    {
        XRController controller = interactable.GetComponentInChildren<XRController>();
        if (controller == null)
            controller = interactable.GetComponentInParent<XRController>();

        if (controller == null)
            return;
        HapticCapabilities capabilities;
        if (controller.inputDevice.TryGetHapticCapabilities(out capabilities))
        {
            if (capabilities.supportsImpulse)
            {
                controller.inputDevice.StopHaptics();
            }
        }
    }

    IEnumerator HapticCoroutine(InputDevice inputDevice, float amplitude, float repeatIn)
    {
        while (isInProximityArea)
        {
            HapticCapabilities capabilities;
            if (inputDevice.TryGetHapticCapabilities(out capabilities))
            {
                if (capabilities.supportsImpulse)
                {
                    inputDevice.SendHapticImpulse(0, amplitude, 0.1f);
                }
            }
            yield return new WaitForSeconds(repeatIn);
        }
    }


    private void OnProximityAreaEnterFunction(Transform interactable)
    {
        SetFeedback(proximityColor, proximityScaleMultiplier);

        if (playSound && proximitySound != null)
        {
            audioSource.Stop();
            audioSource.clip = proximitySound;
            audioSource.Play();
        }

        if (useHapticFeedback)
            HapticFeedback(interactable, hapticAmplitude, hapticHoverFrequence);
    }

    private void OnProximityAreaStayFunction(Transform interactable)
    {
        if (alphaByDistance)
            ProximityUpdateByDistance(interactable);
    }

    private void OnProximityAreaExitFunction(Transform interactable)
    {
        if (alphaByDistance)
            SetFeedback(Color.clear, 1);
        else
            SetFeedback(originalColor, 1);

        if (useHapticFeedback)
            HapticFeedbackStop(interactable);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            if (detectProximity)
            {
                isInProximityArea = true;
                onHoverEnterPos = other.transform.position;

                OnProximityAreaEnterFunction(other.transform);
                onProximityAreaEnter?.Invoke();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            if (detectProximity)
            {
                if (isInProximityArea)
                {
                    OnProximityAreaStayFunction(other.transform);
                    onProximityAreaStay?.Invoke();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            if (detectProximity)
            {
                isInProximityArea = false;

                OnProximityAreaExitFunction(other.transform);
                onProximityAreaExit?.Invoke();
            }
        }
    }
}
