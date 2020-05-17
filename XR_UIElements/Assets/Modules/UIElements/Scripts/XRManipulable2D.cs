using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRManipulable2D: MonoBehaviour
{
    public bool showByProximity = false;

    private XRBaseInteractor interactorA;
    private XRBaseInteractor interactorB;
   
    private XRManipulableGrabInteractable interactableA;
    private XRManipulableGrabInteractable interactableB;

    private List<XRManipulableGrabInteractable> interactables = new List<XRManipulableGrabInteractable>();
    private Vector3 interactableAOriginalLocalPos;
    private Vector3 interactableBOriginalLocalPos;

    private Vector3 firstOriginalPos;
    private Vector3 secondOriginalPos;
    private Quaternion originalRotation;

    private bool resetPosition = false;

    private void Awake()
    {
        interactables = GetComponentsInChildren<XRManipulableGrabInteractable>().ToList();
        SetInteractableEvents();
        //interactorA.onSelectEnter.AddListener(InteractorASelect);
        //interactorA.onSelectExit.AddListener(InteractorASelectExit);
        //interactorA.onHoverEnter.AddListener(ChangeColor);
        //interactorA.onHoverExit.AddListener(BackColor);
        //
        //interactorB.onSelectEnter.AddListener(InteractorBSelect);
        //interactorB.onSelectExit.AddListener(InteractorBSelectExit);
        //interactorB.onHoverEnter.AddListener(ChangeColor);
        //interactorB.onHoverExit.AddListener(BackColor);
    }

    private void SetInteractableEvents()
    {
        foreach (var item in interactables)
        {
            item.onSelectEnter.AddListener(OnInteractableSelectEnter);
        }
    }

    private void OnInteractableSelectEnter(XRBaseInteractor interactor)
    {
        if (interactorA == null)
            interactorA = interactor;
        else
            interactorB = interactor;
        
        firstOriginalPos = interactorA.transform.position;
        interactableAOriginalLocalPos = interactorA.transform.localPosition;

        if (interactorB != null)
            originalRotation = transform.rotation;
    }

    private void Update()
    {
        //if (interactorA.isSelectActive && interactorB.isSelectActive)
        //{
        //    resetPosition = true;
         
        //    if (interactableA.isRotationElement && interactableB.isRotationElement)
        //        OnRotationFunction();
        //    if (interactableA.isScaleElement && interactableB.isScaleElement)
        //        OnScaleFunction();
        //}
        //else if((!interactorA.isSelectActive && !interactorB.isSelectActive) && resetPosition)
        //{
        //    resetPosition = false;

        //    interactableA.transform.localPosition = interactableAOriginalLocalPos;
        //    interactableB.transform.localPosition = interactableBOriginalLocalPos;

        //    interactableA = null;
        //    interactableB = null;
        //}
    }

    private void ChangeColor(XRBaseInteractable interactable)
    {
        MeshRenderer m = interactable.GetComponent<MeshRenderer>();
        m.sharedMaterial = new Material(m.sharedMaterial);

        m.sharedMaterial.color = Color.red;
    }

    private void BackColor(XRBaseInteractable interactable)
    {
        MeshRenderer m = interactable.GetComponent<MeshRenderer>();
        m.sharedMaterial = new Material(m.sharedMaterial);

        m.sharedMaterial.color = Color.white;
    }

    private void InteractorASelect(XRBaseInteractable interactable)
    {
        interactableA = (XRManipulableGrabInteractable)interactable;
        firstOriginalPos = interactable.transform.position;
        interactableAOriginalLocalPos = interactable.transform.localPosition;

        if (interactorB.isSelectActive)
            originalRotation = transform.rotation;
    }

    private void InteractorBSelect(XRBaseInteractable interactable)
    {
        interactableB = (XRManipulableGrabInteractable)interactable;
        secondOriginalPos = interactable.transform.position;
        interactableBOriginalLocalPos = interactable.transform.localPosition;

        if (interactorA.isSelectActive)
            originalRotation = transform.rotation;
    }


    private void InteractorASelectExit(XRBaseInteractable interactable)
    {
        interactable.transform.position = firstOriginalPos;
    }

    private void InteractorBSelectExit(XRBaseInteractable interactable)
    {
        interactable.transform.position = secondOriginalPos;
    }

    private void OnScaleFunction()
    {
        Vector3 fPos = interactorA.transform.position;
        Vector3 sPos = interactorB.transform.position;

        float distance = (fPos - sPos).magnitude;
        float norm = (distance - 0.01f) / (10f - 0.01f);
        norm = Mathf.Clamp01(norm);

        var minScale = Vector3.one * 10.0f;
        var maxScale = Vector3.one * 0.01f;

        transform.localScale = Vector3.Lerp(maxScale, minScale, norm);
    }


    private void OnRotationFunction()
    {
        Vector3 originalDir = (firstOriginalPos - secondOriginalPos).normalized;

        Vector3 fPos = interactorA.transform.position;
        Vector3 sPos = interactorB.transform.position;

        Vector3 currentDir = (fPos - sPos).normalized;

        Quaternion diffDir = Quaternion.FromToRotation(originalDir, currentDir);

        transform.rotation = diffDir * originalRotation;
    }

}
