using NaughtyAttributes;
using System;
using System.CodeDom;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(MeshRenderer), typeof(Collider))]
public class XRDraggableElement : MonoBehaviour
{
    public Transform parentToDrag;

    public bool changeColorOnDrag = true;

    [ShowIf("changeColorOnDrag")] 
    public Color dragColor = Color.cyan;

    public bool isScalableElement = false;
    public bool isRotationElement = false;

    [Header("State")]
    [ReadOnly]
    public bool isDragging = false;
    [ReadOnly]
    public bool isTouching = false;

    private Color meshColor;
    private MeshRenderer meshRenderer;
    private Collider elementCollider;

    private Collider interactableCollider;

    public UnityEvent onDragEnter;
    public UnityEvent onDragStay;
    public UnityEvent onDragExit;

    //Runs only in editor
    private void OnValidate()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        elementCollider = GetComponent<Collider>();
        elementCollider.isTrigger = true;

        //make a copy of mesh material to can be changed without change other objetct with the same material
        if (meshRenderer.sharedMaterial == null)
            meshRenderer.sharedMaterial = new Material(Shader.Find("Unlit/TransparentColor"));
        else
            meshRenderer.sharedMaterial = new Material(meshRenderer.sharedMaterial);
        
        //Save the original color
        meshColor = meshRenderer.sharedMaterial.color;

    }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
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
            DragByController();

            //TODO: Here can be implemented other type of drag action
        }
    }

    public void SetColor(Color color)
    {
        if (meshRenderer.sharedMaterial == null)
            throw new Exception("This object doesn't have a material.");

        meshRenderer.sharedMaterial.color = color;
    }

    public Color GetColor()
    {
        if (meshRenderer.sharedMaterial == null)
            throw new Exception("This object doesn't have a material.");

        return meshRenderer.sharedMaterial.color;
    }

    public Collider InteractableCollider()
    {
        return interactableCollider;
    }

    private XRController GetXRControllerFromCollider(Collider collider)
    {
        XRController xrController = collider.GetComponentInParent<XRController>();

        if (xrController == null)
            xrController = collider.GetComponentInChildren<XRController>();

        return xrController;
    }

    private void DragByController()
    {
        XRController xrController = GetXRControllerFromCollider(interactableCollider);

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