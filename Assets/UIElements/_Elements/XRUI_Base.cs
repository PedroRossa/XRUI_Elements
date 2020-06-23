using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(XRUI_Colors))]
public abstract class XRUI_Base : MonoBehaviour
{
    public bool isEnabled = true;

    protected XRUI_Colors xrUIColors;
    public XRUI_Feedback xrFeedback;

    //TODO: Se não tiver xrFeedback setado, dar um warning de que tu precisa ter um xrFeedback
    //TODO: Criar naughty attributes button pra criar automatico um xrfeedback

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

        if (!isEnabled)
            return;
    }

    private void LookForXRFeedback()
    {
        if (xrFeedback != null)
            return;

        xrFeedback = GetComponentInChildren<XRUI_Feedback>(); //try to find a xrFeedback on object or any child element
        if (xrFeedback == null)
            throw new System.Exception("XRUIBase needs a XRFeedback reference to work properly.");
    }

    [Button]
    public void CreateXRUIFeedback()
    {
        if (xrFeedback == null)
            xrFeedback = gameObject.AddComponent<XRUI_Feedback>();
        else
            Debug.Log("You already have a XRFeedback on your game object or a child of it");
    }
}
