using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
public class XRFeedback : MonoBehaviour
{
    private List<string> ElementTypes { get { return new List<string>() { "Mesh Renderer", "Sprite Renderer" }; } }

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
    public bool detectHover = true;
    public bool detectTouch = true;
    public bool useHapticFeedback = true;

    [Header("Hover Properties")]
    [ShowIf("detectHover")]
    public Color hoverColor = new Color(0.75f, 1.0f, 0.75f, 1.0f);
    [ShowIf("detectHover")]
    public float hoverScaleMultiplier = 1;
    [ShowIf("detectHover")]
    public Collider hoverCollider;
    [ReadOnly]
    public bool isHover;

    [Header("Touch Properties")]
    [ShowIf("detectTouch")]
    public Color touchColor = new Color(0.85f, 0.85f, 0.5f, 1.0f);
    [ShowIf("detectTouch")]
    public float touchScaleMultiplier = 1;
    [ShowIf("detectTouch")]
    public float isTouchOffset = 0.025f;
    [ShowIf("detectTouch")]
    [ReadOnly]
    public bool isTouch;

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


    [ShowIf("detectHover")]
    public UnityEvent onHoverEnter;
    [ShowIf("detectHover")] 
    public UnityEvent onHoverStay;
    [ShowIf("detectHover")] 
    public UnityEvent onHoverExit;

    [ShowIf("detectTouch")]
    public UnityEvent onTouchEnter;
    [ShowIf("detectTouch")]
    public UnityEvent onTouchStay;
    [ShowIf("detectTouch")]
    public UnityEvent onTouchExit;


    private Color originalColor;
    private Vector3 originalScale;
    private Vector3 onHoverEnterPos;

    private void OnValidate()
    {
        if (detectHover)
        {
            if (hoverCollider == null)
                hoverCollider = gameObject.GetComponent<Collider>();

            hoverCollider.isTrigger = true;
        }
    }

    private void Awake()
    {
        if (meshFeedback != null)
        {
            if (alphaByDistance)
                meshFeedback.sharedMaterial.color = Color.clear;

            originalColor = meshFeedback.sharedMaterial.color;
            originalScale = meshFeedback.transform.localScale;
            elementCollider = meshFeedback.GetComponent<Collider>();
        }
        else if (spriteFeedback != null)
        {
            if (alphaByDistance)
                spriteFeedback.color = Color.clear;

            originalColor = spriteFeedback.color;
            originalScale = spriteFeedback.transform.localScale;
            elementCollider = spriteFeedback.GetComponent<Collider>();
        }
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

    private void HoverUpdateByDistance(Transform interactable)
    {
        Color alphaColor = hoverColor;
        float maxDistance = Vector3.Distance(onHoverEnterPos, transform.position);
        float distance = Vector3.Distance(interactable.position, transform.position);

        float normalizedDistance =  (1 / maxDistance * distance);
        alphaColor.a = 1 - normalizedDistance;

        SetFeedback(alphaColor, hoverScaleMultiplier);
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
        while (isHover || isTouch)
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


    private void OnHoverEnterFunction(Transform interactable)
    {
        SetFeedback(hoverColor, hoverScaleMultiplier);

        if (useHapticFeedback)
            HapticFeedback(interactable, hapticAmplitude, hapticHoverFrequence);
    }

    private void OnHoverStayFunction(Transform interactable)
    {
        if (alphaByDistance)
            HoverUpdateByDistance(interactable);
    }

    private void OnHoverExitFunction(Transform interactable)
    {
        SetFeedback(originalColor, 1);

        if (useHapticFeedback)
            HapticFeedbackStop(interactable);
    }



    private void CheckTouchState(Transform interactable)
    {
        Bounds elementBounds = elementCollider.bounds;
        Bounds elementBoundsrWithOffset = new Bounds(elementBounds.center, elementBounds.size * (1 + isTouchOffset));

        if (elementBoundsrWithOffset.Intersects(interactable.GetComponent<Collider>().bounds))
        {
            if (!isTouch)
            {
                OnTouchEnterFunction(interactable);
                onTouchEnter?.Invoke();
            }
            else
            {
                onTouchStay?.Invoke();
            }

            isHover = false;
            isTouch = true;
        }
        else
        {
            if (isTouch)
            {
                isTouch = false;
                isHover = true;

                OnTouchExitFunction(interactable);
                onTouchExit?.Invoke();

                //Call HoverEnter again
                OnHoverEnterFunction(interactable);
                onHoverEnter?.Invoke();
            }
        }
    }

    private void OnTouchEnterFunction(Transform interactable)
    {
        SetFeedback(touchColor, touchScaleMultiplier);

        if (useHapticFeedback)
            HapticFeedback(interactable, hapticAmplitude, hapticTouchFrequence);
    }

    private void OnTouchExitFunction(Transform interactable)
    {
        SetFeedback(hoverColor, hoverScaleMultiplier);

        if (useHapticFeedback)
            HapticFeedbackStop(interactable);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            if (detectHover)
            {
                isHover = true;
                onHoverEnterPos = other.transform.position;

                OnHoverEnterFunction(other.transform);
                onHoverEnter?.Invoke();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            if (detectTouch)
                CheckTouchState(other.transform);

            if (detectHover)
            {
                if (isHover)
                {
                    OnHoverStayFunction(other.transform);
                    onHoverStay?.Invoke();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            if (detectHover)
            {
                isHover = false;

                OnHoverExitFunction(other.transform);
                onHoverExit?.Invoke();
            }
        }
    }

}
