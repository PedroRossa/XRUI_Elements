using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRUIColors), typeof(XRFeedback))]
public abstract class XRUIBase : MonoBehaviour
{
    public bool isEnabled = true;

    protected XRUIColors xrUIColors;
    protected XRFeedback xrFeedback;

    protected virtual void OnValidate()
    {
        xrUIColors = GetComponent<XRUIColors>();
        xrFeedback = GetComponent<XRFeedback>();
    }

    protected virtual void Awake()
    {
        xrUIColors = GetComponent<XRUIColors>();
        xrFeedback = GetComponent<XRFeedback>();
    }

    //TODO: Criar um sistema de templates de cor
}
