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

    [Header("State")]
    [ReadOnly]
    public bool isDragging = false;

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

        if (meshRenderer.sharedMaterial == null)
            meshRenderer.sharedMaterial = new Material(Shader.Find("Unlit/TransparentColor"));

        meshRenderer.sharedMaterial.color = meshColor;

        if (collider == null)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            isDragging = true;
            onDragEnter?.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isDragging)
        {
            DragByController(other);
            onDragStay?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            isDragging = false;
            onDragExit?.Invoke();
        }
    }
}