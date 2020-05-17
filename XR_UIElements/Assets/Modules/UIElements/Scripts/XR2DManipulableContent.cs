using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class XR2DManipulableContent : MonoBehaviour
{
    private List<XRDraggableElement> dragableElements = new List<XRDraggableElement>();

    [Header("General Properties")]
    public bool feedbackByProximity = true;

    public bool manipulationIsActive = true;

    public bool soundFeedback = true;
    public bool hapticsFeedback = true;
    public bool showOutFrame = false;

    public bool activeScale = true;
    public bool activeRotation = true;

    [ShowIf("showOutFrame")]
    public MeshRenderer outFrame;
    [ShowIf("showOutFrame")]
    [Range(0.001f, 0.1f)]
    public float frameSize = 0.01f;
    [ShowIf("showOutFrame")]
    public Color frameColor = Color.white;

    [Header("State")]
    [ReadOnly]
    public bool isRotating = false;
    [ReadOnly]
    public bool isScalling = false;
    [ReadOnly]
    public bool holdWithTwoHands = false;

    [Header("Scalling Properties")]
    public float minScaleFactor = 0.1f;
    public float maxScaleFactor = 100f;


    [ShowIf("activeScale")]
    public UnityEvent onScaleBegin;
    [ShowIf("activeScale")]
    public UnityEvent onScale;
    [ShowIf("activeScale")]
    public UnityEvent onScaleEnd;


    [ShowIf("activeRotation")]
    public UnityEvent onRotationBegin;
    [ShowIf("activeRotation")]
    public UnityEvent onRotation;
    [ShowIf("activeRotation")]
    public UnityEvent onRotationEnd;

    private XRDraggableElement firstSelectedElement;
    private XRDraggableElement secondSelectedElement;

    private Vector3 firstOriginalPos;
    private Vector3 secondOriginalPos;
    private Quaternion originalRotation;

    [Button]
    public void UpdateVisual()
    {
        if (outFrame != null)
        {
            outFrame.sharedMaterial = new Material(Shader.Find("Unlit/Wireframe"));
            outFrame.sharedMaterial.SetFloat("_WireframeVal", frameSize);
            outFrame.sharedMaterial.SetColor("_Color", frameColor);
            outFrame.gameObject.SetActive(showOutFrame);
        }

        foreach (XRDraggableElement item in GetComponentsInChildren<XRDraggableElement>(true))
        {
            XRFeedback currFeedback = item.GetComponentInChildren<XRFeedback>();
            currFeedback.alphaByDistance = feedbackByProximity;
            currFeedback.playSound = soundFeedback;
            currFeedback.useHapticFeedback = hapticsFeedback;

            if (!activeScale && item.isScalableElement)
            {
                item.gameObject.SetActive(false);
                continue;
            }
            if (!activeRotation && item.isRotationElement)
            {
                item.gameObject.SetActive(false);
                continue;
            }

            item.gameObject.SetActive(manipulationIsActive);
        }
    }

    private void Awake()
    {
        if (outFrame != null)
            outFrame.gameObject.SetActive(showOutFrame);

        dragableElements = GetComponentsInChildren<XRDraggableElement>().ToList();

        foreach (XRDraggableElement item in dragableElements)
        {
            XRDraggableElement curr = item;
            item.onDragEnter.AddListener(() => { OnDragElementEnter(curr); });
            item.onDragExit.AddListener(() => { OnDragElementExit(curr); });
            
            if (!activeScale && item.isScalableElement)
            {
                item.gameObject.SetActive(false);
                continue;
            }
            if (!activeRotation && item.isRotationElement)
            {
                item.gameObject.SetActive(false);
                continue;
            }

            item.gameObject.SetActive(manipulationIsActive);
        }
    }

    private void FixedUpdate()
    {
        UpdateSelectedElements();

        if (holdWithTwoHands)
        {
            if(activeScale)
                ScaleEvents();
            
            if(activeRotation)
                RotationEvents();
        }
        else
        {
            if (isScalling)
            {
                isScalling = false;
                onScaleEnd?.Invoke();
            }
            if (isRotating)
            {
                isRotating = false;
                onRotationEnd?.Invoke();
            }
        }
    }

    private void UpdateSelectedElements()
    {
        holdWithTwoHands = false;

        if (firstSelectedElement != null && secondSelectedElement != null)
        {
            Collider firstCollider = firstSelectedElement.InteractableCollider();
            Collider secondCollider = secondSelectedElement.InteractableCollider();

            if (firstCollider == null)
                firstSelectedElement = null;

            if (secondCollider == null)
                secondSelectedElement = null;

            if (firstCollider != null && secondCollider != null)
                holdWithTwoHands = true;
        }
        SetDragableElementsVisibility(!holdWithTwoHands);
    }


    private void OnPanelManipulationBegin()
    {
        firstOriginalPos = firstSelectedElement.InteractableCollider().transform.position;
        secondOriginalPos = secondSelectedElement.InteractableCollider().transform.position;

        originalRotation = transform.rotation;
    }

    private void OnScaleFunction()
    {
        Vector3 fPos = firstSelectedElement.InteractableCollider().transform.position;
        Vector3 sPos = secondSelectedElement.InteractableCollider().transform.position;

        float distance = (fPos - sPos).magnitude;
        float norm = (distance - minScaleFactor) / (maxScaleFactor - minScaleFactor);
        norm = Mathf.Clamp01(norm);

        var minScale = Vector3.one * maxScaleFactor;
        var maxScale = Vector3.one * minScaleFactor;

        transform.localScale = Vector3.Lerp(maxScale, minScale, norm);
    }

    private void OnRotationFunction()
    {
        Vector3 originalDir = (firstOriginalPos - secondOriginalPos).normalized;

        Vector3 fPos = firstSelectedElement.InteractableCollider().transform.position;
        Vector3 sPos = secondSelectedElement.InteractableCollider().transform.position;

        Vector3 currentDir = (fPos - sPos).normalized;

        Quaternion diffDir = Quaternion.FromToRotation(originalDir, currentDir);

        transform.rotation = diffDir * originalRotation;
    }


    private void ScaleEvents()
    {
        if (firstSelectedElement.isScalableElement && secondSelectedElement.isScalableElement)
        {
            if (!isScalling)
            {
                isScalling = true;
                OnPanelManipulationBegin();
                onScaleBegin?.Invoke();
            }
            else
            {
                OnScaleFunction();
                onScale?.Invoke();
            }
        }
    }

    private void RotationEvents()
    {
        if (firstSelectedElement.isRotationElement && secondSelectedElement.isRotationElement)
        {
            if (!isRotating)
            {
                isRotating = true;
                OnPanelManipulationBegin();
                onRotationBegin?.Invoke();
            }
            else
            {
                OnRotationFunction();
                onRotation?.Invoke();
            }
        }
    }


    private void OnDragElementEnter(XRDraggableElement element)
    {
        if (firstSelectedElement == null)
            firstSelectedElement = element;
        else if (secondSelectedElement == null)
            secondSelectedElement = element;

        return;
    }

    private void OnDragElementExit(XRDraggableElement element)
    {
        if (firstSelectedElement != null && firstSelectedElement.Equals(element))
            firstSelectedElement = null;
        else if (secondSelectedElement != null && secondSelectedElement.Equals(element))
            secondSelectedElement = null;
    }


    public void SetDragableElementsVisibility(bool state, bool considerateSelectedElements = false)
    {
        foreach (var item in dragableElements)
        {
            if (!considerateSelectedElements)
            {
                if (item.Equals(firstSelectedElement) || item.Equals(secondSelectedElement))
                    continue;
            }
            item.gameObject.SetActive(state);
        }
    }
}
