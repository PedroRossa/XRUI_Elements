using System;
using TMPro;
using UnityEngine;
using Vizlab.Package.Support;

public class ScaleNotification : MonoBehaviour
{
    #region Variables
    [Tooltip("Notification Text Prefab")]
    public GameObject notification;
    /// <summary>
    /// Object with Renderer or mesh renderer to get the scale to notify
    /// </summary>
    public GameObject target;
    /// <summary>
    /// Empty object to represent the line on X-axis, this  object will be used to add line renderer  
    /// </summary>
    public GameObject lineX;
    /// <summary>
    /// Empty object to represent the line on Y-axis, this  object will be used to add line renderer  
    /// </summary>
    public GameObject lineY;
    /// <summary>
    /// Empty object to represent the line on Z-axis, this  object will be used to add line renderer  
    /// </summary>
    public GameObject lineZ;
    /// <summary>
    /// Empty object to represent the scaled line on X-axis, this  object will be used to add line renderer  
    /// </summary>
    public GameObject lineXScaled;
    /// <summary>
    /// Empty object to represent the scaled line on Y-axis, this  object will be used to add line renderer  
    /// </summary>
    public GameObject lineYScaled;
    /// <summary>
    /// Empty object to represent the scaled line on Z-axis, this  object will be used to add line renderer  
    /// </summary>
    public GameObject lineZScaled;
    /// <summary>
    /// Color  of x-axis
    /// </summary>
    public Color lineRendererXColor = new Color(0, 1, 0, 0.5f);
    /// <summary>
    /// Color  of y-axis
    /// </summary>
    public Color lineRendererYColor = new Color(1, 0, 0, 0.5f);
    /// <summary>
    /// Color  of z-axis
    /// </summary>
    public Color lineRendererZColor = new Color(0, 0, 1, 0.5f);
    /// <summary>
    /// With of line renderer 
    /// </summary>
    public float lineRendererWidth = 0.2f;

    private LineRenderer lineRendererX;
    private LineRenderer lineRendererY;
    private LineRenderer lineRendererZ;

    private LineRenderer lineRendererXScaled;
    private LineRenderer lineRendererYScaled;
    private LineRenderer lineRendererZScaled;

    private Bounds bounds;

    private GameObject notificationTextX;
    private GameObject notificationTextY;
    private GameObject notificationTextZ;

    private TextMeshPro textX;
    private TextMeshPro textY;
    private TextMeshPro textZ;

    private GameObject pointStart;

    private float oririnalSizeX;
    private float oririnalSizeY;
    private float oririnalSizeZ;

    private bool isShowing = false;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
    }
    public void InitializeAll()
    {
        try
        {
            InitializeLines();
            InitializeNotification();
            InitializeRenderer();
        }
        catch (Exception ex)
        {
            //VRDebug.Instance.Log("Initialize Scale script::" + ex, LogType.Error);
        }
    }
    private void InitializeLines()
    {

        lineRendererX = Helper.CreateLineRendererOnObject(lineX, lineRendererWidth, lineRendererWidth, lineRendererXColor, lineRendererXColor);
        lineRendererY = Helper.CreateLineRendererOnObject(lineY, lineRendererWidth, lineRendererWidth, lineRendererYColor, lineRendererYColor);
        lineRendererZ = Helper.CreateLineRendererOnObject(lineZ, lineRendererWidth, lineRendererWidth, lineRendererZColor, lineRendererZColor);

        Color lineRendererXColorS = lineRendererXColor;
        Color lineRendererYColorS = lineRendererYColor;
        Color lineRendererZColorS = lineRendererZColor;

        lineRendererXColorS.a = 1;
        lineRendererYColorS.a = 1;
        lineRendererZColorS.a = 1;

        lineRendererXScaled = Helper.CreateLineRendererOnObject(lineXScaled, lineRendererWidth * 1.3f, lineRendererWidth * 1.3f, lineRendererXColorS, lineRendererXColorS);
        lineRendererYScaled = Helper.CreateLineRendererOnObject(lineYScaled, lineRendererWidth * 1.3f, lineRendererWidth * 1.3f, lineRendererYColorS, lineRendererYColorS);
        lineRendererZScaled = Helper.CreateLineRendererOnObject(lineZScaled, lineRendererWidth * 1.3f, lineRendererWidth * 1.3f, lineRendererZColorS, lineRendererZColorS);
    }
    private void InitializeNotification()
    {
        notificationTextX = Instantiate(notification);
        notificationTextY = Instantiate(notification);
        notificationTextZ = Instantiate(notification);

        notificationTextX.transform.SetParent(this.transform);
        notificationTextY.transform.SetParent(this.transform);
        notificationTextZ.transform.SetParent(this.transform);


        textX = notificationTextX.GetComponent<TextMeshPro>();
        if (textX)
            textX.SetText("");
        textY = notificationTextY.GetComponent<TextMeshPro>();
        if (textY)
            textY.SetText("");
        textZ = notificationTextZ.GetComponent<TextMeshPro>();
        if (textZ)
        {
            textZ.SetText("");
            textZ.alignment = TextAlignmentOptions.Right;
        }

        RectTransform rectTX = notificationTextX.GetComponent<RectTransform>();
        if (rectTX)
        {
            rectTX.anchoredPosition = new Vector2(0, 1);
            rectTX.pivot = new Vector2(0, 1);
        }

        RectTransform rectTY = notificationTextY.GetComponent<RectTransform>();
        if (rectTY)
        {
            rectTY.anchoredPosition = Vector2.zero;
            rectTY.pivot = Vector2.zero;
        }
        RectTransform rectTZ = notificationTextZ.GetComponent<RectTransform>();
        if (rectTZ)
        {
            rectTZ.anchoredPosition = Vector2.one;
            rectTZ.pivot = new Vector2(1.25f, 1);
            Vector3 scaleRZ = rectTZ.localScale;
            scaleRZ.x = scaleRZ.x * -1;
            rectTZ.localScale = scaleRZ;
        }
    }
    private void InitializeRenderer()
    {
        //bounds = target.GetComponent<Renderer>()?.bounds?? target.GetComponentInChildren<Renderer>()?.bounds ?? target.GetComponent<MeshRenderer>()?.bounds ?? target.GetComponentInChildren<MeshRenderer>().bounds;
        Renderer rend = target.GetComponent<Renderer>() ?? target.GetComponentInChildren<Renderer>();
        if (rend)
            bounds = rend.bounds;
        else
        {
            MeshRenderer mRend = target.GetComponent<MeshRenderer>() ?? target.GetComponentInChildren<MeshRenderer>();
            if (mRend)
                bounds = mRend.bounds;
            else
                Debug.LogError("Target bounds can't be access");
        }

        pointStart = new GameObject("pointLines");

        Vector3 position = bounds.center - bounds.extents;
        pointStart.transform.position = position;
        pointStart.transform.SetParent(target.transform);

        oririnalSizeX = target.transform.localScale.x;
        oririnalSizeY = target.transform.localScale.y;
        oririnalSizeZ = target.transform.localScale.z;

        SetShowInfos(false);
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void UpdateInfos()
    {
        if (!isShowing)
            SetShowInfos(true);

        Vector3 start = target.transform.position - (target.transform.forward * bounds.extents.z);
        start = start - (target.transform.right * bounds.extents.x);

        DrawLines(pointStart.transform.position);

        SetNotification(pointStart.transform.position, bounds);
    }
    public void SetShowInfos(bool show)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(show);
        }
        isShowing = show;
    }
    private void DrawLines(Vector3 start)
    {
        DrawLines(lineRendererX, lineRendererXScaled, start, target.transform.right, bounds.size.x, target.transform.localScale.x, oririnalSizeX);
        DrawLines(lineRendererY, lineRendererYScaled, start, target.transform.forward, bounds.size.z, target.transform.localScale.z, oririnalSizeY);
        DrawLines(lineRendererZ, lineRendererZScaled, start, target.transform.up, bounds.size.y, target.transform.localScale.y, oririnalSizeZ);
    }
    private void DrawLines(LineRenderer line, LineRenderer lineScaled, Vector3 start, Vector3 dir, float boundAxis, float scaledAxis, float originalSizeAxis)
    {
        Vector3 endPos = start + (dir * boundAxis);

        line.SetPosition(0, start);
        line.SetPosition(1, endPos);

        if (scaledAxis != originalSizeAxis)
        {
            lineScaled.SetPosition(0, endPos);
            lineScaled.SetPosition(1, start + (dir * boundAxis * scaledAxis / originalSizeAxis));
        }
        else
        {
            lineScaled.SetPosition(0, Vector3.zero);
            lineScaled.SetPosition(1, Vector3.zero);
        }

    }
    private void SetNotification(Vector3 start, Bounds bounds)
    {
        //X
        notificationTextX.transform.position = start;
        float cmX = (float)Math.Round((bounds.size.x * (target.transform.localScale.x / oririnalSizeX)), 4) * 100;
        textX.SetText("Size X : " + Math.Round(cmX, 2) + "cm");
        notificationTextX.transform.rotation = target.transform.rotation;

        //Y  
        notificationTextY.transform.position = start;
        float cmY = (float)Math.Round((bounds.size.y * (target.transform.localScale.y / oririnalSizeY)), 4) * 100;
        textY.SetText("Size Y : " + Math.Round(cmY, 2) + "cm");
        notificationTextY.transform.rotation = target.transform.rotation;
        notificationTextY.transform.Rotate(0f, 0f, 90f, Space.Self);

        //Z
        notificationTextZ.transform.position = start;
        float cmZ = (float)Math.Round((bounds.size.z * (target.transform.localScale.z / oririnalSizeZ)), 4) * 100;
        textZ.SetText("Size Z : " + Math.Round(cmZ, 2) + "cm");
        notificationTextZ.transform.rotation = target.transform.rotation;
        notificationTextZ.transform.Rotate(0f, -90f, 0f, Space.Self);
    }

}