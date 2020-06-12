using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractable))]
public class XRAutoScale : MonoBehaviour
{
    private XRBaseInteractor interactorA;
    private XRBaseInteractor interactorB;
    private XRBaseInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();

        interactable.onHoverEnter.AddListener(OnHoverEnter);
        interactable.onHoverExit.AddListener(OnHoverExit);
    }

    private void OnHoverEnter(XRBaseInteractor interactor)
    {
        if (interactorA == null)
            interactorA = interactor;
        else if (interactorB == null)
            interactorB = interactor;
    }

    private void OnHoverExit(XRBaseInteractor interactor)
    {
        if (interactor.transform.Equals(interactorA))
            interactorA = null;
        if (interactor.transform.Equals(interactorB))
            interactorB = null;
    }

    private void Update()
    {
        if (interactorA != null && interactorB != null)
        {
            XRController controllerA = interactorA.GetComponent<XRController>();
            XRController controllerB = interactorB.GetComponent<XRController>();
           
            bool stateA;
            controllerA.inputDevice.IsPressed(InputHelpers.Button.Trigger, out stateA);
            bool stateB;
            controllerB.inputDevice.IsPressed(InputHelpers.Button.Trigger, out stateB);
            
            if(stateA && stateB)
                Scale();
        }
    }

    void Scale()
    {
        float distance = Vector3.Distance(interactorA.transform.position, interactorB.transform.position); //Change Scale
        transform.localScale = Vector3.one * distance;

        Vector3 middlePoint = (interactorA.transform.position  + interactorB.transform.position) / 2; //Change Position
        transform.position = middlePoint;

        Vector3 rotationDirection = (interactorB.transform.position - interactorA.transform.position); //Change Rotation
        transform.rotation = Quaternion.LookRotation(rotationDirection);
    }
}
