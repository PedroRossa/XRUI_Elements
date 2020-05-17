using JetBrains.Annotations;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private Vector3 initialInteractableScale;

        public XRBaseInteractor Interactor { get; set; }
        public XRManipulableGrabInteractable Interactable { get; private set; }
        public Vector3 InitialElementPosition { get => initialElementPosition; }
        public Vector3 InitialInteractablePosition { get => initialInteractablePosition; }
        public Quaternion InitialInteractableRotation { get => initialInteractableRotation; }
        public Vector3 InitialInteractableScale { get => initialInteractableScale; }
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
                initialInteractableScale = Interactable.transform.localScale;
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
            initialInteractableScale = Vector3.one;
        }

        public void SetInteractableToInitialState()
        {
            if (Interactable != null)
            {
                IsToResetInteractable = false;
                Interactable.transform.localPosition = initialInteractablePosition;
                Interactable.transform.localRotation = initialInteractableRotation;
                Interactable.transform.localScale = initialInteractableScale;

                ClearElement();
            }
        }
    }

    public bool showByProximity = false;

    [MinMaxSlider(0.05f, 100.0f)]
    public Vector2 minMaxScale;

    [ReadOnly]
    public bool isTwoHandsGrabing = false;

    public UnityEvent onTwoHandsGrabingStart;
    public UnityEvent onTwoHandsGrabingStay;
    public UnityEvent onTwoHandsGrabingEnd;


    private InteractionElement interactionA;
    private InteractionElement interactionB;
    private List<XRManipulableGrabInteractable> interactables = new List<XRManipulableGrabInteractable>();
    private Quaternion originalRotation;

    private void Start()
    {
        interactables = GetComponentsInChildren<XRManipulableGrabInteractable>().ToList();
        SetInteractableEvents();

        interactionA = new InteractionElement();
        interactionB = new InteractionElement();
    }

    private void SetInteractableEvents()
    {
        foreach (XRBaseInteractable item in interactables)
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


    private void SetInteractablesVisibility(bool value)
    {
        foreach (XRBaseInteractable item in interactables)
            item.gameObject.SetActive(value);

        if (!value)//make selected elements aways enabled
        {
            if (interactionA.Interactable != null)
                interactionA.Interactable.gameObject.SetActive(true);
            if (interactionB.Interactable != null)
                interactionB.Interactable.gameObject.SetActive(true);
        }
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
