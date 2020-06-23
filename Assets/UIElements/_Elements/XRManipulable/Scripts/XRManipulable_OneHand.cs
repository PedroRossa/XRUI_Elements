using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;
using UnityEngine.XR.Interaction.Toolkit;

public class XRManipulable_OneHand : MonoBehaviour
{
    protected class OriginalState
    {
        public Vector3 position;
        public Vector3 localPosition;
        public Vector3 scale;
        public Quaternion rotation;

        public OriginalState()
        {
            position = new Vector3();
            localPosition = new Vector3();
            scale = new Vector3();
            rotation = new Quaternion();
        }

        public OriginalState(Vector3 position, Vector3 localPosition, Vector3 scale, Quaternion rotation)
        {
            this.position = position;
            this.localPosition = localPosition;
            this.scale = scale;
            this.rotation = rotation;
        }
    }
   
    protected class InteractableData
    {
        public XRManipulableInteractable interactable;
        public OriginalState originalState;

        public InteractableData()
        {
            interactable = null;
            originalState = new OriginalState();
        }

        public InteractableData(XRManipulableInteractable interactable)
        {
            this.interactable = interactable;
            originalState = new OriginalState
            (
                interactable.transform.position,
                interactable.transform.localPosition,
                interactable.transform.localScale,
                interactable.transform.rotation
            );
        }

        public void ResetTransform()
        {
            interactable.transform.localScale = originalState.scale;
            interactable.transform.localPosition = originalState.localPosition;
            interactable.transform.rotation = originalState.rotation;
        }
    }

    [Header("General References")]
    public Transform content;
    public Transform scaleElements;
    public Transform rotationElements;

    [Header("General Configurations")]
    //TODO: Make readonly until solve the problem of alpha calculation
    [ReadOnly]
    public bool showByProximity = false;
    public bool showInteractables = true;

    [MinMaxSlider(0.1f, 100.0f)]
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

    [ReadOnly]
    private bool isSelected;
    [ReadOnly]
    private bool isScalling;
    [ReadOnly]
    private bool isRotating;

    #region UnityEvents
    public UnityEvent onScaleStart;
    public UnityEvent onScaleStay;
    public UnityEvent onScaleEnd;
    public UnityEvent onRotationStart;
    public UnityEvent onRotationStay;
    public UnityEvent onRotationEnd;
    #endregion 

    private OriginalState originalState;
    private InteractableData currentInteractable;

    private GameObject wireBox;

    private Vector3 minBound;
    private Vector3 maxBound;

    private GameObject manipulableCopy;

    private void Start()
    {
        currentInteractable = new InteractableData();
        UpdateManipulables();
    }

    private void Update()
    {
        if (currentInteractable.interactable != null)
        {
            if (currentInteractable.interactable.isScaleElement)
                ManageScale();
            if (currentInteractable.interactable.isRotationElement)
                ManageRotation();

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
                currentInteractable = new InteractableData();
            }
        }
    }

    private void ManageScale()
    {
        if (!isScalling)
        {
            onScaleStart?.Invoke();
            isScalling = true;
        }
        else
            onScaleStay?.Invoke();

        ScaleContent();
    }

    private void ManageRotation()
    {
        if (!isRotating)
        {
            onRotationStart?.Invoke();
            isRotating = true;
        }
        else
            onRotationStay?.Invoke();

        RotateContent();
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
            originalState = new OriginalState(transform.position, transform.localPosition, transform.localScale, transform.rotation);
            CreateManipulableCopy();
        }
    }

    private void OnInteractableSelectExit(XRBaseInteractor interactor)
    {
        if (isRotating)
        {
            onRotationEnd?.Invoke();
            isRotating = false;
        }

        if (isScalling)
        {
            onScaleEnd?.Invoke();
            isScalling = false;
        }

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
        Vector3 pivot = 2 * originalState.position - currentInteractable.originalState.position;

        float initialDistance = Vector3.Distance(pivot, currentInteractable.originalState.position);
        float distance = Vector3.Distance(pivot, interactablePos);

        float t = distance / initialDistance;
        //Limitate scale
        if ((originalState.scale.x * t) <= minMaxScale.y && (originalState.scale.x * t) >= minMaxScale.x)
            XRUI_Helper.ScaleAround(transform, pivot, originalState.scale * t);
    }

    private void RotateContent()
    {
        Vector3 originalDirection = (currentInteractable.originalState.position - originalState.position).normalized;
        Vector3 currentDirection = (currentInteractable.interactable.transform.position - transform.position).normalized;

        originalDirection = Vector3.ProjectOnPlane(originalDirection, originalState.rotation * currentInteractable.interactable.rotationAxis);
        currentDirection = Vector3.ProjectOnPlane(currentDirection, originalState.rotation * currentInteractable.interactable.rotationAxis);

        Quaternion differenceDirection = Quaternion.FromToRotation(originalDirection, currentDirection);
        transform.rotation = differenceDirection * originalState.rotation;
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
}
