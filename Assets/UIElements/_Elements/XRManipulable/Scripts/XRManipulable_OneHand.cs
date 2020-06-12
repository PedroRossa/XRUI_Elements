using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRManipulable_OneHand : MonoBehaviour
{
    [Header("General References")]
    public Transform content;
    public Transform scaleElements;
    public Transform rotationElements;

    [Header("General Configurations")]
    //TODO: Make readonly until solve the problem of alpha calculation
    [NaughtyAttributes.ReadOnly]
    public bool showByProximity = false;
    public bool showInteractables = true;

    [MinMaxSlider(0.005f, 20.0f)]
    public Vector2 minMaxScale = new Vector2(0.01f, 10);
    [Range(0.01f, 0.1f)]
    public float interactablesOffset = 0.1f;
    [Range(0.1f, 1.0f)]
    public float interactablesSize = 0.1f;
    [Range(0.5f, 10.0f)]
    public float proximityColliderRadius;


    public bool is3DManipulable = false;
    public bool showWireBox = true;

    [Dropdown("FeedbackTypes")]
    public string feedbackType;

    private List<string> FeedbackTypes { get { return new List<string>() { "Mesh", "Outline" }; } }

    [Header("Colors")]
    [ShowIf("showWireBox")]
    public Color wireBoxColor = Color.white;
    public Color scaleElementColor = Color.green;
    public Color rotationElementColor = Color.blue;
    public Color proximityColor = Color.magenta;

    public UnityEvent onGrabStart;
    public UnityEvent onGrabStay;
    public UnityEvent onGrabEnd;

    public UnityEvent onScaleStart;
    public UnityEvent onScaleStay;
    public UnityEvent onScaleEnd;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    private bool isSelected;

    private GameObject wireBox;

    private Vector3 minBound;
    private Vector3 maxBound;

    private GameObject manipulableCopy;

    public struct InteractableData
    {
        public XRManipulableInteractable interactable;

        public Vector3 initialPosition;
        public Quaternion initialRotation;

        public Vector3 initialLocalPosition;

        public InteractableData(XRManipulableInteractable interactable)
        {
            this.interactable = interactable;
            initialPosition = interactable.transform.position;
            initialLocalPosition = interactable.transform.localPosition;
            initialRotation = interactable.transform.rotation;
        }

        public void ResetTransform()
        {
            interactable.transform.localPosition = initialLocalPosition;
            interactable.transform.rotation = initialRotation;
        }
    }

    private InteractableData currentInteractable;

    private void Start()
    {
        UpdateManipulables();
    }

    private void Update()
    {
        if (currentInteractable.interactable != null)
        {
            //onTwoHandsGrabingStart?.Invoke();

            if (currentInteractable.interactable.isScaleElement)
                ScaleContent();
            if (currentInteractable.interactable.isRotationElement)
                RotateContent();
            //onTwoHandsGrabingStay?.Invoke();

            if (isSelected)
            {
                if (manipulableCopy.GetComponent<LineRenderer>() != null)
                {
                    manipulableCopy.GetComponent<LineRenderer>().SetPosition(0, manipulableCopy.transform.position);
                    manipulableCopy.GetComponent<LineRenderer>().SetPosition(1, currentInteractable.interactable.transform.position);
                }
            }
            else if (!isSelected && currentInteractable.interactable.transform.parent != null)
            {
                currentInteractable.ResetTransform();
                currentInteractable.interactable = null;
            }
        }
        else
        {
            //onTwoHandsGrabingEnd?.Invoke();
        }
    }


    private void ConfigureInteractables()
    {
        foreach (var item in GetComponentsInChildren<XRManipulableInteractable>(true).ToList())
        {
            item.GetComponent<XRBaseFeedback>().alphaColorByDistance = showByProximity;
            if (showByProximity)
                item.GetComponent<XRBaseFeedback>().SetColor(Color.clear);

            item.onSelectEnter.AddListener(OnInteractableSelectEnter);
            item.onSelectExit.AddListener(OnInteractableSelectExit);
        }
    }

    private void CreateManipulableCopy()
    {
        //copy and positionate on the intial manipulable position
        manipulableCopy = Instantiate(currentInteractable.interactable.gameObject);
        manipulableCopy.transform.SetParent(currentInteractable.interactable.transform.parent);
        manipulableCopy.transform.localPosition = currentInteractable.interactable.transform.localPosition;
        manipulableCopy.transform.localScale = currentInteractable.interactable.transform.localScale;
        manipulableCopy.transform.localRotation = currentInteractable.interactable.transform.localRotation;

        //Remove components
        Destroy(manipulableCopy.GetComponent<XRManipulableInteractable>());
        Destroy(manipulableCopy.GetComponent<Rigidbody>());

        foreach (Collider item in manipulableCopy.GetComponents<Collider>())
            Destroy(item);

        //Set manipulable copy color with transparency
        MeshRenderer mr = manipulableCopy.GetComponent<MeshRenderer>();
        Color originalColor = mr.material.color;
        originalColor.a = 0.25f;
        mr.material.color = originalColor;

        //Add a LineRenderer to conect original and copy manipulables
        LineRenderer lr = manipulableCopy.AddComponent<LineRenderer>();
        lr.startWidth = 0.0025f;
        lr.endWidth = 0.0025f;
        lr.useWorldSpace = true;

        lr.material = new Material(Shader.Find("Unlit/TransparentColor"));
        Color lineColor = new Color(1, 1, 1, 0.35f);
        lr.material.color = lineColor;
        lr.material.color = lineColor;

        lr.positionCount = 2;
        lr.SetPosition(0, manipulableCopy.transform.position);
        lr.SetPosition(1, currentInteractable.interactable.transform.position);
    }

    private void OnInteractableSelectEnter(XRBaseInteractor interactor)
    {
        isSelected = true;
        if (interactor.selectTarget.GetType() != typeof(XRManipulableInteractable))
            return;
        if (currentInteractable.interactable == null)
        {
            currentInteractable = new InteractableData((XRManipulableInteractable)interactor.selectTarget);
            originalRotation = transform.rotation;
            originalPosition = transform.position;
            originalScale = transform.localScale;
            CreateManipulableCopy();
        }
    }

    private void OnInteractableSelectExit(XRBaseInteractor interactor)
    {
        Destroy(manipulableCopy);
        isSelected = false;

    }


    #region Interactable Elements Creation Function

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

    private void SetWireframeBox()
    {
        Transform wireTransform = transform.Find("Wirebox");
        if (wireTransform != null)
            DestroyImmediate(wireTransform.gameObject);

        wireBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wireBox.name = "Wirebox";

        MeshRenderer wireBoxMesh = wireBox.GetComponent<MeshRenderer>();
        wireBoxMesh.sharedMaterial = new Material(Shader.Find("Unlit/Wireframe"));
        wireBoxMesh.sharedMaterial.SetFloat("_WireframeVal", 0.01f);
        wireBoxMesh.sharedMaterial.SetColor("_Color", wireBoxColor);

        //Get the middle of de content boundbox
        wireBox.transform.position = (maxBound + minBound) / 2;
        //Adjust scale
        wireBox.transform.localScale = (maxBound - minBound);
        wireBox.transform.SetParent(transform);
        wireBox.transform.SetAsFirstSibling();

        //if is a 2d manipulabe make te box a plane (put 0 on z scale)
        if (!is3DManipulable)
            wireBox.transform.localScale = new Vector3(wireBox.transform.localScale.x, wireBox.transform.localScale.y, 0);

        wireBox.SetActive(showWireBox);
    }

    private GameObject CreateManipulableObject(bool isScale)
    {
        GameObject go;
        if (isScale)
        {
            go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.SetParent(scaleElements);

            go.GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Unlit/TransparentColor"));
            go.GetComponent<MeshRenderer>().sharedMaterial.color = scaleElementColor;
        }
        else
        {
            go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.SetParent(rotationElements);

            go.GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Unlit/TransparentColor"));
            go.GetComponent<MeshRenderer>().sharedMaterial.color = rotationElementColor;
        }
        return go;
    }

    //TODO: Solve this rotationAxis to be called in a better way
    private void ConfigureManipulable(ref GameObject go, bool isScale, Vector3 rotationAxis)
    {
        XRManipulableInteractable manipulable = go.AddComponent<XRManipulableInteractable>();
        manipulable.trackRotation = false;
        manipulable.throwOnDetach = false;

        manipulable.isScaleElement = isScale;
        manipulable.isRotationElement = !isScale;
        manipulable.rotationAxis = rotationAxis;
    }

    private void ConfigureFeedback(ref GameObject go)
    {
        XRBaseFeedback feedback;
        if (feedbackType.Equals("Outline"))
            feedback = go.AddComponent<XROutlineFeedback>();
        else
            feedback = go.AddComponent<XRMeshFeedback>();

        feedback.proximityColor = proximityColor;
        feedback.proximityColliderSize = proximityColliderRadius;

        feedback.AutoconfigureColliderAsSphere();
    }

    private void CreateManipulableElement(string name, Vector3 position, bool isScale, Vector3 rotationAxis = new Vector3())
    {
        GameObject go = CreateManipulableObject(isScale);

        ConfigureManipulable(ref go, isScale, rotationAxis);
        ConfigureFeedback(ref go);

        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        rb.constraints = RigidbodyConstraints.FreezeRotation;

        go.name = name;
        go.transform.localScale = Vector3.one * interactablesSize;
        go.transform.position = position;
    }

    private void SetScaleInteractablesPosition()
    {
        while (scaleElements.childCount > 0)
            DestroyImmediate(scaleElements.GetChild(0).gameObject);

        if (is3DManipulable)
        {
            CreateManipulableElement("frontTopLeft", new Vector3(minBound.x, maxBound.y, minBound.z), true);
            CreateManipulableElement("frontTopRight", new Vector3(maxBound.x, maxBound.y, minBound.z), true);
            CreateManipulableElement("frontBottomLeft", new Vector3(minBound.x, minBound.y, minBound.z), true);
            CreateManipulableElement("frontBottomRight", new Vector3(maxBound.x, minBound.y, minBound.z), true);

            CreateManipulableElement("backTopLeft", new Vector3(minBound.x, maxBound.y, maxBound.z), true);
            CreateManipulableElement("backTopRight", new Vector3(maxBound.x, maxBound.y, maxBound.z), true);
            CreateManipulableElement("backBottomLeft", new Vector3(minBound.x, minBound.y, maxBound.z), true);
            CreateManipulableElement("backBottomRight", new Vector3(maxBound.x, minBound.y, maxBound.z), true);
        }
        else
        {
            Vector3 middle = (maxBound + minBound) / 2;
            CreateManipulableElement("topLeft", new Vector3(minBound.x, maxBound.y, middle.z), true);
            CreateManipulableElement("topRight", new Vector3(maxBound.x, maxBound.y, middle.z), true);
            CreateManipulableElement("bottomLeft", new Vector3(minBound.x, minBound.y, middle.z), true);
            CreateManipulableElement("bottomRight", new Vector3(maxBound.x, minBound.y, middle.z), true);
        }
    }

    private void SetRotationInteractablesPosition()
    {
        while (rotationElements.childCount > 0)
            DestroyImmediate(rotationElements.GetChild(0).gameObject);

        Vector3 middle = (maxBound + minBound) / 2;
        if (is3DManipulable)
        {
            CreateManipulableElement("frontLeft", new Vector3(minBound.x, middle.y, minBound.z), false, Vector3.up);
            CreateManipulableElement("frontRight", new Vector3(maxBound.x, middle.y, minBound.z), false, Vector3.up);
            CreateManipulableElement("frontTop", new Vector3(middle.x, maxBound.y, minBound.z), false, Vector3.right);
            CreateManipulableElement("frontBottom", new Vector3(middle.x, minBound.y, minBound.z), false, Vector3.right);

            CreateManipulableElement("middleTopLeft", new Vector3(minBound.x, maxBound.y, middle.z), false, Vector3.forward);
            CreateManipulableElement("middleTopRight", new Vector3(maxBound.x, maxBound.y, middle.z), false, Vector3.forward);
            CreateManipulableElement("middleBottomLeft", new Vector3(minBound.x, minBound.y, middle.z), false, Vector3.forward);
            CreateManipulableElement("middleBottomRight", new Vector3(maxBound.x, minBound.y, middle.z), false, Vector3.forward);

            CreateManipulableElement("backLeft", new Vector3(minBound.x, middle.y, maxBound.z), false, Vector3.up);
            CreateManipulableElement("backRight", new Vector3(maxBound.x, middle.y, maxBound.z), false, Vector3.up);
            CreateManipulableElement("backTop", new Vector3(middle.x, maxBound.y, maxBound.z), false, Vector3.right);
            CreateManipulableElement("backBottom", new Vector3(middle.x, minBound.y, maxBound.z), false, Vector3.right);
        }
        else
        {
            CreateManipulableElement("frontLeft", new Vector3(minBound.x, middle.y, middle.z), false, Vector3.up);
            CreateManipulableElement("frontRight", new Vector3(maxBound.x, middle.y, middle.z), false, Vector3.up);
            CreateManipulableElement("frontTop", new Vector3(middle.x, maxBound.y, middle.z), false, Vector3.right);
            CreateManipulableElement("frontBottom", new Vector3(middle.x, minBound.y, middle.z), false, Vector3.right);
        }
    }

    #endregion

    private void ScaleContent()
    {
        Vector3 interactablePos = currentInteractable.interactable.transform.position;

        Vector3 pivot = 2 * originalPosition - currentInteractable.initialPosition;


        Debug.DrawLine(originalPosition, currentInteractable.initialPosition, Color.magenta);

        float initialDistance = Vector3.Distance(pivot, currentInteractable.initialPosition);
        float distance = Vector3.Distance(pivot, interactablePos);

        float t = distance / initialDistance;

        ScaleAround(transform, pivot, originalScale * t);

        //transform.localScale = Vector3.one * t;
    }


    private void RotateContent()
    {
        Vector3 originalDirection = (currentInteractable.initialPosition - originalPosition).normalized;
        Vector3 currentDirection = (currentInteractable.interactable.transform.position - transform.position).normalized;

        originalDirection = Vector3.ProjectOnPlane(originalDirection, originalRotation * currentInteractable.interactable.rotationAxis);
        currentDirection = Vector3.ProjectOnPlane(currentDirection, originalRotation * currentInteractable.interactable.rotationAxis);

        Quaternion differenceDirection = Quaternion.FromToRotation(originalDirection, currentDirection);
        transform.rotation = differenceDirection * originalRotation;
    }

    public void SetInteractablesVisibility(bool value)
    {
        foreach (XRBaseInteractable item in GetComponentsInChildren<XRManipulableInteractable>(true).ToList())
            item.gameObject.SetActive(value);

        if (!value)//make selected elements always enabled
        {
            if (currentInteractable.interactable != null)
                currentInteractable.interactable.gameObject.SetActive(true);
        }
    }

    [Button]
    public void UpdateManipulables()
    {
        if (content.childCount <= 0)
            return;

        CalculateInteractablesDistance();

        SetWireframeBox();
        SetScaleInteractablesPosition();
        SetRotationInteractablesPosition();

        ConfigureInteractables();
        SetInteractablesVisibility(showInteractables);
    }

    ///<summary>
    /// Scales the target around an arbitrary point by scaleFactor.
    /// This is relative scaling, meaning using  scale Factor of Vector3.one
    /// will not change anything and new Vector3(0.5f,0.5f,0.5f) will reduce
    /// the object size by half.
    /// The pivot is assumed to be the position in the space of the target.
    /// Scaling is applied to localScale of target.
    /// </summary>
    /// <param name="target">The object to scale.</param>
    /// <param name="pivot">The point to scale around in space of target.</param>
    /// <param name="scaleFactor">The factor with which the current localScale of the target will be multiplied with.</param>
    public static void ScaleAroundRelative(Transform target, Vector3 pivot, Vector3 scaleFactor)
    {
        // pivot
        var pivotDelta = target.localPosition - pivot;
        pivotDelta.Scale(scaleFactor);
        target.localPosition = pivot + pivotDelta;

        // scale
        var finalScale = target.localScale;
        finalScale.Scale(scaleFactor);
        target.localScale = finalScale;
    }

    /// <summary>
    /// Scales the target around an arbitrary pivot.
    /// This is absolute scaling, meaning using for example a scale factor of
    /// Vector3.one will set the localScale of target to x=1, y=1 and z=1.
    /// The pivot is assumed to be the position in the space of the target.
    /// Scaling is applied to localScale of target.
    /// </summary>
    /// <param name="target">The object to scale.</param>
    /// <param name="pivot">The point to scale around in the space of target.</param>
    /// <param name="scaleFactor">The new localScale the target object will have after scaling.</param>
    public static void ScaleAround(Transform target, Vector3 pivot, Vector3 newScale)
    {
        // pivot
        Vector3 pivotDelta = target.position - pivot; // diff from object pivot to desired pivot/origin
        Vector3 scaleFactor = new Vector3(
            newScale.x / target.localScale.x,
            newScale.y / target.localScale.y,
            newScale.z / target.localScale.z);
        pivotDelta.Scale(scaleFactor);
        target.position = pivot + pivotDelta;

        //scale
        target.localScale = newScale;
    }
}
