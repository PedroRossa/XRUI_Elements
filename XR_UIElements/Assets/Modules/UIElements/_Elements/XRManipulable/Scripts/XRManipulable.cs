using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRManipulable : MonoBehaviour
{
    internal class InteractionElement
    {
        private Vector3 initialElementPosition;

        private Vector3 initialInteractablePosition;
        private Quaternion initialInteractableRotation;

        public XRBaseInteractor Interactor { get; set; }
        public XRManipulableGrabInteractable Interactable { get; private set; }
        public Vector3 InitialElementPosition { get => initialElementPosition; }
        public Vector3 InitialInteractablePosition { get => initialInteractablePosition; }
        public Quaternion InitialInteractableRotation { get => initialInteractableRotation; }
        public bool IsToResetInteractable { get; set; }

        public InteractionElement()
        {
            ClearElement();
        }

        public InteractionElement(XRBaseInteractor interactor)
        {
            Interactor = interactor;
            if (interactor != null)
            {
                Interactable = (XRManipulableGrabInteractable)interactor.selectTarget;
                initialElementPosition = Interactable.transform.position;
                initialInteractablePosition = Interactable.transform.localPosition;
                initialInteractableRotation = Interactable.transform.localRotation;
            }
            else
            {
                ClearElement();
            }
        }

        private void ClearElement()
        {
            Interactor = null;
            Interactable = null;
            initialElementPosition = initialInteractablePosition = Vector3.zero;
            initialInteractableRotation = Quaternion.identity;
        }

        public void SetInteractableToInitialState()
        {
            if (Interactable != null)
            {
                IsToResetInteractable = false;
                Interactable.transform.localPosition = initialInteractablePosition;
                Interactable.transform.localRotation = initialInteractableRotation;
                Interactable.transform.localScale = Vector3.one;

                ClearElement();
            }
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

    [ReadOnly]
    public bool isTwoHandsGrabing = false;

    public UnityEvent onTwoHandsGrabingStart;
    public UnityEvent onTwoHandsGrabingStay;
    public UnityEvent onTwoHandsGrabingEnd;


    private InteractionElement interactionA;
    private InteractionElement interactionB;
    private Quaternion originalRotation;

    private GameObject wireBox;

    private Vector3 minBound;
    private Vector3 maxBound;


    private void Start()
    {
        interactionA = new InteractionElement();
        interactionB = new InteractionElement();

        UpdateManipulables();
    }

    private void Update()
    {
        if (interactionA.Interactable != null && interactionB.Interactable != null)
        {
            if (!isTwoHandsGrabing)
            {
                isTwoHandsGrabing = true;
                interactionA.Interactable.StopMoving();
                interactionB.Interactable.StopMoving();
                SetInteractablesVisibility(false);
                onTwoHandsGrabingStart?.Invoke();
            }
            else
            {
                if (interactionB.Interactable.isScaleElement && interactionA.Interactable.isScaleElement)
                    ScaleContent();
                if (interactionB.Interactable.isRotationElement && interactionA.Interactable.isRotationElement)
                    RotateContent();
                onTwoHandsGrabingStay?.Invoke();
            }
        }
        else
        {
            if (isTwoHandsGrabing)
            {
                isTwoHandsGrabing = false;

                SetInteractablesVisibility(true);
                onTwoHandsGrabingEnd?.Invoke();
            }
        }

        if (interactionA.IsToResetInteractable)
            interactionA.SetInteractableToInitialState();
        if (interactionB.IsToResetInteractable)
            interactionB.SetInteractableToInitialState();
    }


    private void ConfigureInteractables()
    {
        foreach (var item in GetComponentsInChildren<XRManipulableGrabInteractable>(true).ToList())
        {
            item.GetComponent<XRBaseFeedback>().alphaColorByDistance = showByProximity;
            if (showByProximity)
                item.GetComponent<XRBaseFeedback>().SetColor(Color.clear);

            item.onSelectEnter.AddListener(OnInteractableSelectEnter);
            item.onSelectExit.AddListener(OnInteractableSelectExit);
        }
    }
    
    private void OnInteractableSelectEnter(XRBaseInteractor interactor)
    {
        if (interactionA.Interactor == null)
            interactionA = new InteractionElement(interactor);
        else
            interactionB = new InteractionElement(interactor);

        if (interactionA.Interactor != null && interactionB.Interactor != null)
            originalRotation = transform.rotation;
    }

    private void OnInteractableSelectExit(XRBaseInteractor interactor)
    {
        if (interactor.Equals(interactionA.Interactor))
            interactionA.IsToResetInteractable = true;

        if (interactor.Equals(interactionB.Interactor))
            interactionB.IsToResetInteractable = true;
    }


    #region Interactable Creation Function
    
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

    private void ConfigureManipulable(ref GameObject go, bool isScale)
    {
        XRManipulableGrabInteractable manipulable = go.AddComponent<XRManipulableGrabInteractable>();
        manipulable.trackRotation = false;
        manipulable.throwOnDetach = false;

        manipulable.parentToMove = transform;

        manipulable.isScaleElement = isScale;
        manipulable.isRotationElement = !isScale;
        manipulable.moveParent = true;
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

    private void CreateManipulableElement(string name, Vector3 position, bool isScale)
    {
        GameObject go = CreateManipulableObject(isScale);

        ConfigureManipulable(ref go, isScale);
        ConfigureFeedback(ref go);

        go.GetComponent<Rigidbody>().useGravity = false;
        go.GetComponent<Rigidbody>().isKinematic = true;

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
            CreateManipulableElement("frontLeft", new Vector3(minBound.x, middle.y, minBound.z), false);
            CreateManipulableElement("frontRight", new Vector3(maxBound.x, middle.y, minBound.z), false);
            CreateManipulableElement("frontTop", new Vector3(middle.x, maxBound.y, minBound.z), false);
            CreateManipulableElement("frontBottom", new Vector3(middle.x, minBound.y, minBound.z), false);

            CreateManipulableElement("middleTopLeft", new Vector3(minBound.x, maxBound.y, middle.z), false);
            CreateManipulableElement("middleTopRight", new Vector3(maxBound.x, maxBound.y, middle.z), false);
            CreateManipulableElement("middleBottomLeft", new Vector3(minBound.x, minBound.y, middle.z), false);
            CreateManipulableElement("middleBottomRight", new Vector3(maxBound.x, minBound.y, middle.z), false);

            CreateManipulableElement("backLeft", new Vector3(minBound.x, middle.y, maxBound.z), false);
            CreateManipulableElement("backRight", new Vector3(maxBound.x, middle.y, maxBound.z), false);
            CreateManipulableElement("backTop", new Vector3(middle.x, maxBound.y, maxBound.z), false);
            CreateManipulableElement("backBottom", new Vector3(middle.x, minBound.y, maxBound.z), false);
        }
        else
        {
            CreateManipulableElement("frontLeft", new Vector3(minBound.x, middle.y, middle.z), false);
            CreateManipulableElement("frontRight", new Vector3(maxBound.x, middle.y, middle.z), false);
            CreateManipulableElement("frontTop", new Vector3(middle.x, maxBound.y, middle.z), false);
            CreateManipulableElement("frontBottom", new Vector3(middle.x, minBound.y, middle.z), false);
        }
    }

    #endregion


    private void ScaleContent()
    {
        Vector3 fPos = interactionA.Interactable.transform.position;
        Vector3 sPos = interactionB.Interactable.transform.position;


        float sqrMagnitude = (fPos - sPos).sqrMagnitude;
        float clampedValue = Mathf.Clamp(sqrMagnitude, minMaxScale.x, minMaxScale.y);

        transform.localScale = Vector3.one * clampedValue;
    }

    private void RotateContent()
    {
        Vector3 originalDir = (interactionA.InitialElementPosition - interactionB.InitialElementPosition).normalized;

        Vector3 fPos = interactionA.Interactable.transform.position;
        Vector3 sPos = interactionB.Interactable.transform.position;

        Vector3 currentDir = (fPos - sPos).normalized;
        Quaternion diffDir = Quaternion.FromToRotation(originalDir, currentDir);

        transform.rotation = diffDir * originalRotation;
    }


    public void SetInteractablesVisibility(bool value)
    {
        foreach (XRBaseInteractable item in GetComponentsInChildren<XRManipulableGrabInteractable>(true).ToList())
            item.gameObject.SetActive(value);

        if (!value)//make selected elements always enabled
        {
            if (interactionA != null && interactionA.Interactable != null)
                interactionA.Interactable.gameObject.SetActive(true);
            if (interactionB != null && interactionB.Interactable != null)
                interactionB.Interactable.gameObject.SetActive(true);
        }
    }

    [Button]
    public void UpdateManipulables()
    {
        CalculateInteractablesDistance();

        SetWireframeBox();
        SetScaleInteractablesPosition();
        SetRotationInteractablesPosition();

        ConfigureInteractables();
        SetInteractablesVisibility(showInteractables);
    }
}
