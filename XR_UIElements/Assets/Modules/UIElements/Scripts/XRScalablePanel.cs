﻿using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class XRScalablePanel : MonoBehaviour
{
    private List<XRDragableElement> dragableElements = new List<XRDragableElement>();

    public bool feedbackByProximity = true;

    [Header("Scalling Properties")]
    public float minScaleFactor = 0.1f;
    public float maxScaleFactor = 100f;
    [ReadOnly]
    public bool isScalling = false;


    [Header("Scalling Properties")]
    [ReadOnly]
    public bool isRotating = false;

    public XRDragableElement firstSelectedElement;
    public XRDragableElement secondSelectedElement;

    public UnityEvent onScaleBegin;
    public UnityEvent onScale;
    public UnityEvent onScaleEnd;

    public UnityEvent onRotationBegin;
    public UnityEvent onRotation;
    public UnityEvent onRotationEnd;

    private Vector3 firstOriginalPos;
    private Vector3 secondOriginalPos;
    private Quaternion originalRotation;

    [ReadOnly]
    public bool holdWithTwoHands = false;

    private void OnValidate()
    {
        foreach (XRDragableElement item in GetComponentsInChildren<XRDragableElement>())
            item.GetComponentInChildren<XRFeedback>().alphaByDistance = feedbackByProximity;
    }

    private void Awake()
    {
        dragableElements = GetComponentsInChildren<XRDragableElement>().ToList();

        foreach (XRDragableElement item in dragableElements)
        {
            XRDragableElement curr = item;
            item.onDragEnter.AddListener(() => { OnDragElementEnter(curr); });
            item.onDragExit.AddListener(() => { OnDragElementExit(curr); });
        }
    }

    private void FixedUpdate()
    {
        UpdateSelectedElements();
        if (holdWithTwoHands)
        {
            ScaleEvents();
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

        if (firstSelectedElement != null && !firstSelectedElement.isDragging)
            firstSelectedElement = null;
        if (secondSelectedElement != null && !secondSelectedElement.isDragging)
            secondSelectedElement = null;

        if (firstSelectedElement != null && secondSelectedElement != null)
        {
            if (firstSelectedElement.GetDraggingXRController() != null && secondSelectedElement.GetDraggingXRController() != null)
                holdWithTwoHands = true;
        }

        SetNonSelectedDragableElements(!holdWithTwoHands);
    }

    private void OnPanelManipulationBegin()
    {
        firstOriginalPos = firstSelectedElement.GetDraggingXRController().transform.position;
        secondOriginalPos = secondSelectedElement.GetDraggingXRController().transform.position;

        originalRotation = transform.rotation;
    }

    private void OnScaleFunction()
    {
        Vector3 fPos = firstSelectedElement.GetDraggingXRController().transform.position;
        Vector3 sPos = secondSelectedElement.GetDraggingXRController().transform.position;

        //------------------------------------
        float distance = (fPos - sPos).magnitude;
        float norm = (distance - minScaleFactor) / (maxScaleFactor - minScaleFactor);
        norm = Mathf.Clamp01(norm);

        var minScale = Vector3.one * maxScaleFactor;
        var maxScale = Vector3.one * minScaleFactor;
        //transform.position = (fPos - sPos) / 2;


        transform.localScale = Vector3.Lerp(maxScale, minScale, norm);
        //------------------------------------
    }
    
    private void OnRotationFunction()
    {
        Vector3 originalDir = (firstOriginalPos - secondOriginalPos).normalized;

        Vector3 fPos = firstSelectedElement.GetDraggingXRController().transform.position;
        Vector3 sPos = secondSelectedElement.GetDraggingXRController().transform.position;

        Vector3 currentDir = (fPos - sPos).normalized;
        // Difference rot
        Quaternion diffDir = Quaternion.FromToRotation(originalDir, currentDir);

        // Apply
        transform.rotation = diffDir * originalRotation;

        Debug.DrawLine(firstOriginalPos, fPos, Color.magenta);
        Debug.DrawLine(secondOriginalPos, sPos, Color.magenta);
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


    private void OnDragElementEnter(XRDragableElement element)
    {
        if (firstSelectedElement == null)
            firstSelectedElement = element;
        else if (secondSelectedElement == null)
            secondSelectedElement = element;

        return;
    }

    private void OnDragElementExit(XRDragableElement element)
    {
        if (firstSelectedElement != null && firstSelectedElement.Equals(element))
            firstSelectedElement = null;
        else if (secondSelectedElement != null && secondSelectedElement.Equals(element))
            secondSelectedElement = null;
    }


    private void SetNonSelectedDragableElements(bool state)
    {
        foreach (var item in dragableElements)
        {
            if (item.Equals(firstSelectedElement) || item.Equals(secondSelectedElement))
                continue;

            item.gameObject.SetActive(state);
        }
    }
}
