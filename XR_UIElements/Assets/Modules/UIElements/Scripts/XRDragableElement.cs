using Boo.Lang;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(MeshRenderer), typeof(Collider))]
public class XRDragableElement : MonoBehaviour
{
    public Transform parentToDrag;
    public Color meshColor = Color.magenta;
    public bool isScalableElement = false;
    public bool isRotationElement = false;

    [Header("State")]
    [ReadOnly]
    public bool isDragging = false;

    private bool isTouching = false;
    private XRController xrController;

    private MeshRenderer meshRenderer;
    private Collider elementCollider;

    public UnityEvent onDragEnter;
    public UnityEvent onDragStay;
    public UnityEvent onDragExit;

    //Runs only in editor
    private void OnValidate()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer.sharedMaterial == null)
            meshRenderer.sharedMaterial = new Material(Shader.Find("Unlit/TransparentColor"));

        meshRenderer.sharedMaterial.color = meshColor;

        if (elementCollider == null)
            elementCollider = GetComponent<Collider>();

        elementCollider.isTrigger = true;
    }

    private void Awake()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();

        if (elementCollider == null)
            elementCollider = GetComponent<Collider>();

        elementCollider.isTrigger = true;

        //If don't set a parent to drag, drag itself
        if (parentToDrag == null)
            parentToDrag = transform;
    }

    private void FixedUpdate()
    {
        if(isTouching)
            DragByController();
    }

    public void SetColor(Color color)
    {
        meshRenderer.sharedMaterial.color = color;
    }

    public Color GetColor()
    {
        if (meshRenderer.sharedMaterial == null)
            return Color.white;

        return meshRenderer.sharedMaterial.color;
    }

    public XRController GetDraggingXRController()
    {
        return xrController;
    }

    private XRController GetXRControllerByCollider(Collider collider)
    {
        XRController xrController = collider.GetComponentInChildren<XRController>();
        if (xrController == null)
        {
            xrController = collider.GetComponentInParent<XRController>();
            if (xrController == null)
                return null;
        }
        return xrController;
    }

    private void DragByController()
    {
        if (xrController == null || xrController.inputDevice == null)
        {
            isDragging = false;
            return;
        }

        bool isPressed;
        if (xrController.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out isPressed) && isPressed)
        {
            if (!isDragging)
            {
                isDragging = true;
                onDragEnter?.Invoke();
            }
            else
            {
                MoveElement();
                onDragStay?.Invoke();
            }
        }

        if ((!isPressed && isDragging) || !isDragging)
        {
            isDragging = false;
            onDragExit?.Invoke();
        }
    }

    private void MoveElement()
    {
        Vector3 parentOffset = parentToDrag.position - transform.position;
        parentToDrag.position = xrController.GetComponentInChildren<Collider>().transform.position + parentOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            isTouching = true;
            xrController = GetXRControllerByCollider(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            xrController = null;
            isTouching = false;
            isDragging = false;
        }
    }
}