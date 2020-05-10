using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(MeshRenderer), typeof(Collider))]
public class XRDragableElement : MonoBehaviour
{
    public Transform parentToDrag;
    public bool isScalableElement = false;

    [Header("State")]
    [ReadOnly]
    public bool isDrag = false;

    private MeshRenderer meshRenderer;
    private Collider collider;
    private Vector3 parentOffset;

    public UnityEvent onDragEnter;
    public UnityEvent onDragStay;
    public UnityEvent onDragExit;

    private void Awake()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();
       
        if (collider == null)
            collider = GetComponent<Collider>();

        collider.isTrigger = true;

        //If don't set a parent to drag, drag itself
        if (parentToDrag == null)
            parentToDrag = transform;

        parentOffset = parentToDrag.position - transform.position;
    }

    //Runs only in editor
    private void OnValidate()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();

        if(collider == null)
            collider = GetComponent<Collider>();

        collider.isTrigger = true;
    }

    private void DragByController(Collider collider)
    {
        XRController xrController = collider.GetComponentInChildren<XRController>();
        if (xrController == null)
        {
            xrController = collider.GetComponentInParent<XRController>();
            if (xrController == null)
                return;
        }

        if (xrController.inputDevice != null)
        {
            bool isPressed;
            if (xrController.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out isPressed) && isPressed)
            {
                parentToDrag.position = collider.transform.position + parentOffset;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            isDrag = true;
            onDragEnter?.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isDrag)
        {
            DragByController(other);
            onDragStay?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            isDrag = false;
            onDragExit?.Invoke();
        }
    }
}