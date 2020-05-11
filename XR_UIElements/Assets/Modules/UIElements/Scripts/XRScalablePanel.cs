using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class XRScalablePanel : MonoBehaviour
{
    private List<XRDragableElement> dragElements = new List<XRDragableElement>();
    private List<XRDragableElement> scaleElements = new List<XRDragableElement>();

    private Color originalColor;
    public Color scallingColor = Color.yellow;

    public bool showDragableByProximity = true;

    [ReadOnly]
    public bool isScalling = false;

    public XRDragableElement firstScaleElement;
    public XRDragableElement secondScaleElement;

    public UnityEvent onScaleBegin;
    public UnityEvent onScale;
    public UnityEvent onScaleEnd;

    private void OnValidate()
    {
        foreach (XRDragableElement item in GetComponentsInChildren<XRDragableElement>())
            item.GetComponentInChildren<XRFeedback>().alphaByDistance = showDragableByProximity;
    }

    private void Awake()
    {
        ConfigureDragables();
    }

    private void ConfigureDragables()
    {
        scaleElements = new List<XRDragableElement>();
        dragElements = new List<XRDragableElement>();

        foreach (XRDragableElement item in GetComponentsInChildren<XRDragableElement>())
        {
            if (item.isScalableElement)
                scaleElements.Add(item);
            else
                dragElements.Add(item);
        }
        originalColor = scaleElements[0].GetColor();
    }


    private void Update()
    {
        firstScaleElement = null;
        secondScaleElement = null;

        foreach (XRDragableElement item in scaleElements)
        {
            if (item.isDragging)
            {
                if (firstScaleElement == null)
                {
                    firstScaleElement = item;
                    continue;
                }
                else if (secondScaleElement == null)
                {
                    secondScaleElement = item;
                    continue;
                }
            }
        }

        ScalePanel();
    }

    private void ScalePanel()
    {
        if (firstScaleElement != null && secondScaleElement != null)
        {
            firstScaleElement.SetColor(scallingColor);
            secondScaleElement.SetColor(scallingColor);
            isScalling = true;

            Vector3 fPos = firstScaleElement.transform.position;
            Vector3 sPos = secondScaleElement.transform.position;

            Vector3 centerPos = new Vector3(fPos.x + sPos.x, fPos.y + sPos.y, fPos.z + sPos.z) / 2;

            float scaleX = Mathf.Abs(fPos.x - sPos.x);
            float scaleY = Mathf.Abs(fPos.y - sPos.y);
            float scaleZ = Mathf.Abs(fPos.z - sPos.z);

            centerPos.x -= 0.5f;
            centerPos.y += 0.5f;

            Debug.Log(centerPos + " - " + new Vector3(scaleX, scaleY, scaleZ));
            //transform.position = centerPos;
            //transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

        }
        else
        {
            isScalling = false;
        }
    }

}
