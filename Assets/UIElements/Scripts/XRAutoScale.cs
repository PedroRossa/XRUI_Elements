using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractable))]
public class XRAutoScale : MonoBehaviour
{
    private XRBaseInteractor interactorA;
    private XRBaseInteractor interactorB;
    private XRBaseInteractable interactable;

    private Vector3 initialPositionA;
    private Vector3 initialPositionB;
    private Vector3 originalScale;
    private Transform originalParent;

    [MinMaxSlider(0.001f, 100.0f)]
    public Vector2 minMaxScale = new Vector2(0.01f, 10);

    public bool considerParent;

    [ReadOnly]
    public bool isScalling;

    [ShowIf("considerParent")]
    public Transform parentToScale;

    #region UnityEvents
    public UnityEvent onScaleStart;
    public UnityEvent onScaleStay;
    public UnityEvent onScaleEnd;
    #endregion 

    private void Awake()
    {
        originalParent = transform.parent;
        interactable = GetComponent<XRBaseInteractable>();

        interactable.onHoverEnter.AddListener(OnHoverEnter);
        interactable.onHoverExit.AddListener(OnHoverExit);
    }

    private void OnHoverEnter(XRBaseInteractor interactor)
    {
        //TODO: Solve here how to grap the object with two hands to scale!
        if (interactorA == null)
        {
            interactorA = interactor;
            initialPositionA = interactor.GetComponent<Collider>().transform.position;
        }
        else if (interactorB == null)
        {
            interactorB = interactor;
            initialPositionB = interactor.GetComponent<Collider>().transform.position;
        }

        originalScale = considerParent ? parentToScale.localScale : transform.localScale;
    }

    private void OnHoverExit(XRBaseInteractor interactor)
    {
        if (isScalling)
            return;
        if (interactor.Equals(interactorA))
            interactorA = null;
        if (interactor.Equals(interactorB))
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

            if (stateA && stateB)
                ManageScale();
            else
            {
                onScaleEnd?.Invoke();
                interactorA = null;
                interactorB = null;
            }
        }
    }

    private void ManageScale()
    {
        if (transform.parent == null && originalParent != null)
            transform.SetParent(originalParent);

        if (!isScalling)
        {
            onScaleStart?.Invoke();
            isScalling = true;
        }
        else
            onScaleStay?.Invoke();

        if (considerParent)
            Scale(parentToScale);
        else
            Scale(transform);
    }

    void Scale(Transform target)
    {
        Vector3 posA = interactorA.GetComponent<Collider>().transform.position;
        Vector3 posB = interactorB.GetComponent<Collider>().transform.position;

        //CHANGE SCALE
        float distance = Vector3.Distance(posA, posB);
        float initialDistance = Vector3.Distance(initialPositionA, initialPositionB);

        float t = distance / initialDistance;
        Vector3 newScale = originalScale * t;

        //Limitate scale
        if (newScale.x <= minMaxScale.y && newScale.y >= minMaxScale.x && distance != 0)
        {
            target.localScale = newScale;

            //CHANGE POSITION
            Vector3 middlePoint = (posA + posB) / 2;
            target.position = middlePoint;

            //CHANGE ROTATION
            Vector3 rotationDirection = (posB - posA);
            target.rotation = Quaternion.LookRotation(rotationDirection);
        }
    }
}
