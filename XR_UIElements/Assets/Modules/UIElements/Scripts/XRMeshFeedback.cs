using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(MeshRenderer), typeof(Collider), typeof(Outline))]
public class XRMeshFeedback : MonoBehaviour
{
    private AudioSource audioSource;
    private MeshRenderer elementMesh;
    private Collider elementCollider;
    private Outline outline;

    [Header("Feedback Properties")]
    public bool alphaByDistance = true;
    public bool useHapticFeedback = true;
    public bool playSound = true;

    [Header("Proximity Properties")]
    [HideIf("alphaByDistance")]
    public Color proximityColor = new Color(0.75f, 1.0f, 0.75f, 1.0f);
    public float proximityScaleMultiplier = 1;
    public Collider proximityCollider;
    [ShowIf(EConditionOperator.And, "playSound", "detectProximity")]
    public AudioClip proximitySound;
    [ReadOnly]
    public bool isInProximityArea;

    [Header("Haptic Properties")]
    [ShowIf("useHapticFeedback")]
    [Range(0.0f, 1.0f)]
    public float hapticAmplitude = 0.15f;
    [ShowIf("useHapticFeedback")]
    [Range(0.0f, 5.0f)]
    public float hapticProximityFrequence = 0.75f;


    public UnityEvent onProximityAreaEnter;
    public UnityEvent onProximityAreaStay;
    public UnityEvent onProximityAreaExit;

    private Color originalColor;
    private Vector3 originalScale;
    private Vector3 onProximityEnterPos;

    private void Awake()
    {
        elementMesh = GetComponent<MeshRenderer>();
        elementCollider = elementMesh.GetComponent<Collider>();
        outline = GetComponent<Outline>();

        if (elementMesh.sharedMaterial == null)
            elementMesh.sharedMaterial = new Material(Shader.Find("Unlit/TransparentColor"));
        else //Copy the material to don't change every meshFeedbacks
            elementMesh.sharedMaterial = new Material(elementMesh.sharedMaterial);

        outline.OutlineColor = proximityColor;

        originalColor = elementMesh.sharedMaterial.color;
        originalScale = elementMesh.transform.localScale;

        if (alphaByDistance)
            SetFeedback(Color.clear, 1);

        if (playSound)
            ConfigureAudioSource();

        if (proximityCollider == null)
            throw new System.Exception("The element don't have a trigger Collider attached");
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
        elementMesh.sharedMaterial.color = color;
        elementMesh.transform.localScale = originalScale * scaleMultiplier;
    }

    private void ProximityUpdateByDistance(Transform interactable)
    {
        Color alphaColor = originalColor;
        float maxDistance = Vector3.Distance(onProximityEnterPos, transform.position);
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
        if (!alphaByDistance)
            SetFeedback(proximityColor, proximityScaleMultiplier);

        if (playSound && proximitySound != null)
        {
            audioSource.Stop();
            audioSource.clip = proximitySound;
            audioSource.Play();
        }

        if (useHapticFeedback)
            HapticFeedback(interactable, hapticAmplitude, hapticProximityFrequence);
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
            isInProximityArea = true;
            onProximityEnterPos = other.transform.position;

            OnProximityAreaEnterFunction(other.transform);
            onProximityAreaEnter?.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            if (isInProximityArea)
            {
                OnProximityAreaStayFunction(other.transform);
                onProximityAreaStay?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            isInProximityArea = false;

            OnProximityAreaExitFunction(other.transform);
            onProximityAreaExit?.Invoke();
        }
    }
}
