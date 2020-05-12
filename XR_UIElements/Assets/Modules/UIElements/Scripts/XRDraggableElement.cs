using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(MeshRenderer), typeof(Collider))]
public class XRDraggableElement : MonoBehaviour
{
    public Transform parentToDrag;
    public Color meshColor = Color.magenta;
    public Color dragColor = Color.cyan;
    public bool isScalableElement = false;
    public bool isRotationElement = false;

    [Header("State")]
    [ReadOnly]
    public bool isDragging = false;

    private bool isTouching = false;

    private MeshRenderer meshRenderer;
    private Collider elementCollider;
    private Collider interactableCollider;

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
        if (isTouching)
        {
            XRController xrController = interactableCollider.GetComponentInParent<XRController>();

            if (xrController == null)
                xrController = interactableCollider.GetComponentInChildren<XRController>();

            if (xrController != null)
                DragByController(xrController);

            //TODO: Here can be implemented other type of drag action
        }
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

    public Collider InteractableCollider()
    {
        return interactableCollider;
    }

    private void DragByController(XRController xrController)
    {
        if (xrController == null || xrController.inputDevice == null)
        {
            SetColor(meshColor);
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
                SetColor(dragColor);
                MoveElement(xrController.GetComponentInChildren<Collider>().transform.position);
                onDragStay?.Invoke();
            }
        }

        if ((!isPressed && isDragging) || !isDragging)
        {
            SetColor(meshColor);
            isDragging = false;
            onDragExit?.Invoke();
        }
    }

    private void MoveElement(Vector3 newPosition)
    {
        Vector3 parentOffset = parentToDrag.position - transform.position;
        parentToDrag.position = newPosition + parentOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            isTouching = true;
            interactableCollider = other;
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
            interactableCollider = null;
            isTouching = false;
            isDragging = false;
        }
    }
}