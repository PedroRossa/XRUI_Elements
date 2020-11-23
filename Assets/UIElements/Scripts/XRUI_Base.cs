using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Base class of XRUI elements
/// </summary>
[RequireComponent(typeof(XRUI_Colors))]
public abstract class XRUI_Base : MonoBehaviour
{
    /// <summary>
    /// Is the button enabled?
    /// </summary>
    public bool isEnabled = true;

    /// <summary>
    /// The xrUIColors object's reference
    /// </summary>
    [HideInInspector]
    public XRUI_Colors xrUIColors;
    public XRUI_Feedback xrFeedback;

    /// <summary>
    /// Callback called when the button action is validated
    /// </summary>
    protected virtual void OnValidate()
    {
        xrUIColors = GetComponent<XRUI_Colors>();
        LookForXRFeedback();

        xrFeedback.isEnabled = isEnabled;

        //Check if the uicolor is used with a Feedback Color script. If it's, refresh the normal color on target
        XRUI_FeedbackColor feedbackColor = GetComponentInChildren<XRUI_FeedbackColor>();
        if (feedbackColor != null)
            feedbackColor.RefreshElementColor();
    }

    protected virtual void Awake()
    {
        xrUIColors = GetComponent<XRUI_Colors>();
        LookForXRFeedback();

        xrFeedback.isEnabled = isEnabled;
    }

    /// <summary>
    /// If it is null, try to set xrFeedback reference to the XRUI_Feedback with a component in children
    /// </summary>
    private void LookForXRFeedback()
    {
        if (xrFeedback != null)
            return;

        xrFeedback = GetComponentInChildren<XRUI_Feedback>(); //try to find a xrFeedback on object or any child element
        if (xrFeedback == null)
            throw new System.Exception("XRUIBase needs a XRFeedback reference to work properly.");
    }

    /// <summary>
    /// Create a XRUIFeedback component in the button if it hasn't
    /// </summary>
    [Button]
    public void CreateXRUIFeedback()
    {
        if (xrFeedback == null)
            xrFeedback = gameObject.AddComponent<XRUI_Feedback>();
        else
            Debug.Log("You already have a XRFeedback on your game object or a child of it");
    }
}
