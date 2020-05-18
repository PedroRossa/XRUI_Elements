using JetBrains.Annotations;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRManipulable2D : MonoBehaviour
{
    internal class InteractionElement
    {
        private Vector3 initialElementPosition;

        private Vector3 initialInteractablePosition;
        private Quaternion initialInteractableRotation;

        public XRBaseInteractor Interactor { get; set; }
        public XRManipulableGrabInteractable Interactable { get; private set; }
        public Vector3 InitialElementPosition { get => initialElementPosition; }
        public Vector3 InitialInteractablePosition { get => initialInteractablePosition; }
        public Quaternion InitialInteractableRotation { get => initialInteractableRotation; }
        public bool IsToResetInteractable { get; set; }

        public InteractionElement()
        {
            ClearElement();
        }

        public InteractionElement(XRBaseInteractor interactor)
        {
            Interactor = interactor;
            if (interactor != null)
            {
                Interactable = (XRManipulableGrabInteractable)interactor.selectTarget;
                initialElementPosition = Interactable.transform.position;
                initialInteractablePosition = Interactable.transform.localPosition;
                initialInteractableRotation = Interactable.transform.localRotation;
            }
            else
            {
                ClearElement();
            }
        }

        private void ClearElement()
        {
            Interactor = null;
            Interactable = null;
            initialElementPosition = initialInteractablePosition = Vector3.zero;
            initialInteractableRotation = Quaternion.identity;
        }

        public void SetInteractableToInitialState()
        {
            if (Interactable != null)
            {
                IsToResetInteractable = false;
                Interactable.transform.localPosition = initialInteractablePosition;
                Interactable.transform.localRotation = initialInteractableRotation;
                Interactable.transform.localScale = Vector3.one;

                ClearElement();
            }
        }
    }

    public Transform content;
    public bool showByProximity = false;

    [MinMaxSlider(0.05f, 100.0f)]
    public Vector2 minMaxScale;

    [Range(0.01f, 0.1f)]
    public float interactablesOffset = 0.1f;

    [BoxGroup("Scale Interactables")]
    public XRManipulableGrabInteractable topLeft;
    [BoxGroup("Scale Interactables")]
    public XRManipulableGrabInteractable topRight;
    [BoxGroup("Scale Interactables")]
    public XRManipulableGrabInteractable bottomLeft;
    [BoxGroup("Scale Interactables")]
    public XRManipulableGrabInteractable bottomRight;

    [BoxGroup("Rotation Interactables")]
    public XRManipulableGrabInteractable left;
    [BoxGroup("Rotation Interactables")]
    public XRManipulableGrabInteractable right;
    [BoxGroup("Rotation Interactables")]
    public XRManipulableGrabInteractable top;
    [BoxGroup("Rotation Interactables")]
    public XRManipulableGrabInteractable bottom;

    [ReadOnly]
    public bool isTwoHandsGrabing = false;

    public UnityEvent onTwoHandsGrabingStart;
    public UnityEvent onTwoHandsGrabingStay;
    public UnityEvent onTwoHandsGrabingEnd;


    private InteractionElement interactionA;
    private InteractionElement interactionB;
    private Quaternion originalRotation;

    Vector3 minBound;
    Vector3 maxBound;

    [Button]
    public void UpdateManipulablesPosition()
    {
        CalculateInteractablesDistance();
        SetScaleInteractablesPosition();
        SetRotationInteractablesPosition();
    }

    private void Start()
    {
        ConfigureInteractables();

        interactionA = new InteractionElement();
        interactionB = new InteractionElement();


        CalculateInteractablesDistance();
    }

    private void ConfigureInteractables()
    {
        foreach (var item in GetComponentsInChildren<XRManipulableGrabInteractable>(true).ToList())
        {
            item.GetComponent<XRBaseFeedback>().alphaColorByDistance = showByProximity;
            if (showByProximity)
                item.GetComponent<XRBaseFeedback>().SetColor(Color.clear);

            item.onSelectEnter.AddListener(OnInteractableSelectEnter);
            item.onSelectExit.AddListener(OnInteractableSelectExit);
        }
    }

    private void Update()
    {
        if (interactionA.Interactable != null && interactionB.Interactable != null)
        {
            if (!isTwoHandsGrabing)
            {
                isTwoHandsGrabing = true;
                interactionA.Interactable.StopMoving();
                interactionB.Interactable.StopMoving();
                SetInteractablesVisibility(false);
                onTwoHandsGrabingStart?.Invoke();
            }
            else
            {
                if (interactionB.Interactable.isScaleElement && interactionA.Interactable.isScaleElement)
                    ScaleContent();
                if (interactionB.Interactable.isRotationElement && interactionA.Interactable.isRotationElement)
                    RotateContent();
                onTwoHandsGrabingStay?.Invoke();
            }
        }
        else
        {
            if (isTwoHandsGrabing)
            {
                isTwoHandsGrabing = false;

                SetInteractablesVisibility(true);
                onTwoHandsGrabingEnd?.Invoke();
            }
        }

        if (interactionA.IsToResetInteractable)
            interactionA.SetInteractableToInitialState();
        if (interactionB.IsToResetInteractable)
            interactionB.SetInteractableToInitialState();
    }


    private void OnInteractableSelectEnter(XRBaseInteractor interactor)
    {
        if (interactionA.Interactor == null)
            interactionA = new InteractionElement(interactor);
        else
            interactionB = new InteractionElement(interactor);

        if (interactionA.Interactor != null && interactionB.Interactor != null)
            originalRotation = transform.rotation;
    }

    private void OnInteractableSelectExit(XRBaseInteractor interactor)
    {
        if (interactor.Equals(interactionA.Interactor))
            interactionA.IsToResetInteractable = true;

        if (interactor.Equals(interactionB.Interactor))
            interactionB.IsToResetInteractable = true;
    }


    public void SetInteractablesVisibility(bool value)
    {
        foreach (XRBaseInteractable item in GetComponentsInChildren<XRManipulableGrabInteractable>(true).ToList())
            item.gameObject.SetActive(value);

        if (!value)//make selected elements always enabled
        {
            if (interactionA != null && interactionA.Interactable != null)
                interactionA.Interactable.gameObject.SetActive(true);
            if (interactionB != null && interactionB.Interactable != null)
                interactionB.Interactable.gameObject.SetActive(true);
        }
    }


    private void CalculateInteractablesDistance()
    {
        minBound = Vector3.one * float.MaxValue;
        maxBound = Vector3.one * float.MinValue;
        foreach (Renderer item in content.GetComponentsInChildren<Renderer>())
        {
            if (item.bounds.min.x < minBound.x) minBound.x = item.bounds.min.x;
            if (item.bounds.min.y < minBound.y) minBound.y = item.bounds.min.y;
            if (item.bounds.min.z < minBound.z) minBound.z = item.bounds.min.z;

            if (item.bounds.max.x > maxBound.x) maxBound.x = item.bounds.max.x;
            if (item.bounds.max.y > maxBound.y) maxBound.y = item.bounds.max.y;
            if (item.bounds.max.z > maxBound.z) maxBound.z = item.bounds.max.z;
        }

        //Add offset
        minBound -= Vector3.one * interactablesOffset;
        maxBound += Vector3.one * interactablesOffset;
    }

    private void SetScaleInteractablesPosition()
    {
        float z = (maxBound.z + minBound.z) / 2;

        topLeft.transform.position = new Vector3(minBound.x, maxBound.y, z);
        topRight.transform.position = new Vector3(maxBound.x, maxBound.y, z);
        bottomLeft.transform.position = new Vector3(minBound.x, minBound.y, z);
        bottomRight.transform.position = new Vector3(maxBound.x, minBound.y, z);
    }

    private void SetRotationInteractablesPosition()
    {
        Vector3 middle = (maxBound + minBound) / 2;

        left.transform.position = new Vector3(minBound.x, middle.y, middle.z);
        right.transform.position = new Vector3(maxBound.x, middle.y, middle.z);
        top.transform.position = new Vector3(middle.x, maxBound.y, middle.z);
        bottom.transform.position = new Vector3(middle.x, minBound.y, middle.z);
    }


    private void ScaleContent()
    {
        Vector3 fPos = interactionA.Interactable.transform.position;
        Vector3 sPos = interactionB.Interactable.transform.position;

        float sqrMagnitude = (fPos - sPos).sqrMagnitude;
        float clampedValue = Mathf.Clamp(sqrMagnitude, minMaxScale.x, minMaxScale.y);

        transform.localScale = Vector3.one * clampedValue;
    }

    private void RotateContent()
    {
        Vector3 originalDir = (interactionA.InitialElementPosition - interactionB.InitialElementPosition).normalized;

        Vector3 fPos = interactionA.Interactable.transform.position;
        Vector3 sPos = interactionB.Interactable.transform.position;

        Vector3 currentDir = (fPos - sPos).normalized;

        Quaternion diffDir = Quaternion.FromToRotation(originalDir, currentDir);

        transform.rotation = diffDir * originalRotation;
    }
}
