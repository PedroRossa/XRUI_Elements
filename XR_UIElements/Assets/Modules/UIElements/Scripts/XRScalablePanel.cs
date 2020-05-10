using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class XRScalablePanel : MonoBehaviour
{
    private XRDragableElement[] dragElements;

    public Material dragElementsMaterial;
    public bool showDragableByProximity = true;

    [ReadOnly]
    public bool scalling = false;

    [ReadOnly]
    public XRDragableElement firstDargElement;
    [ReadOnly]
    public XRDragableElement secondDragElement;

    public UnityEvent onScaleBegin;
    public UnityEvent onScale;
    public UnityEvent onScaleEnd;

    private void Awake()
    {

    }

    private void Update()
    {
        if(firstDargElement != null)
        {
            if (!firstDargElement.isDrag)
                firstDargElement = null;
        }
        if(secondDragElement != null)
        {
            if (!secondDragElement.isDrag)
                secondDragElement = null;
        }


        foreach (XRDragableElement item in dragElements)
        {
            if (firstDargElement == null)
            {
                if (item.isScalableElement && item.isDrag)
                    firstDargElement = item;
            }
            else if (secondDragElement == null)
            {
                if (item.isScalableElement && item.isDrag)
                    secondDragElement = item;
            }
            else
                continue;
        }

        ScalePanel();
    }

    private void OnValidate()
    {
        SetDragableElementsState();
    }

    private void SetDragableElementsState()
    {
        dragElements = GetComponentsInChildren<XRDragableElement>();

        if (dragElementsMaterial == null)
        {
            dragElementsMaterial = new Material(Shader.Find("Unlit/TransparentColor"));
            dragElementsMaterial.color = Color.magenta;
        }
        foreach (XRDragableElement item in dragElements)
        {
            item.GetComponent<MeshRenderer>().sharedMaterial = dragElementsMaterial;
            item.GetComponentInChildren<XRFeedback>().alphaByDistance = showDragableByProximity;
        }
    }


    private void ScalePanel()
    {
        if (firstDargElement != null && secondDragElement != null)
        {
            Debug.Log(Vector3.Distance(firstDargElement.transform.position, secondDragElement.transform.position));
        }
    }
}
