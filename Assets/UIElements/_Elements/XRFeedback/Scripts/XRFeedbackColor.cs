using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRUIColors))]
public class XRFeedbackColor : XRFeedbackBaseType
{
    public enum VisualFeedbackType
    {
        MeshRenderer = 1,
        SpriteRenderer = 2,
        Outline = 3
    }

    public VisualFeedbackType feedbackType = VisualFeedbackType.Outline;

    private XRUIColors xrUIColors;
    private MeshRenderer meshRenderer;
    private SpriteRenderer spriteRenderer;
    private Outline outline;

    private void OnValidate()
    {
        CheckComponentsByType();

        if (xrUIColors == null)
            xrUIColors = GetComponent<XRUIColors>();
        RefreshObjectNormalColor();
    }

    protected override void Awake()
    {
        base.Awake();

        if (xrUIColors == null)
            xrUIColors = GetComponent<XRUIColors>();
        RefreshObjectNormalColor();

        InitializeByType();
    }


    private void RefreshObjectNormalColor()
    {
        if (meshRenderer != null)
        {
            meshRenderer.sharedMaterial = new Material(meshRenderer.sharedMaterial);
            meshRenderer.sharedMaterial.color = xrUIColors.normalColor;
        }
        else if (spriteRenderer != null)
            xrUIColors.normalColor = spriteRenderer.color;
    }

    private void CheckComponentsByType()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (feedbackType == VisualFeedbackType.MeshRenderer && meshRenderer == null)
        {
            Debug.LogError("MeshRenderer Feedback it's only aplicable on gameobjects with a MeshRenderer attached");
            Debug.LogError("Automatically changed to Outline Feedback");
            feedbackType = VisualFeedbackType.Outline;
        }

        if (feedbackType == VisualFeedbackType.SpriteRenderer && spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer Feedback it's only aplicable on gameobjects with a SpriteRenderer attached");
            Debug.LogError("Automatically changed to Outline Feedback");
            feedbackType = VisualFeedbackType.Outline;
        }
    }

    private void InitializeByType()
    {
        CheckComponentsByType();

        if (feedbackType == VisualFeedbackType.Outline && outline == null)
            outline = gameObject.AddComponent<Outline>();
    }


    private void SetNearFeedbackListenersByType()
    {
        switch (feedbackType)
        {
            case VisualFeedbackType.MeshRenderer:
                {
                    xrFeedback.onNearEnter.AddListener((XRController controller) => { meshRenderer.material.color = xrUIColors.nearColor; });
                    xrFeedback.onNearExit.AddListener((XRController controller) => { meshRenderer.material.color = xrUIColors.normalColor; });
                    break;
                }
            case VisualFeedbackType.SpriteRenderer:
                {
                    xrFeedback.onNearEnter.AddListener((XRController controller) => { spriteRenderer.color = xrUIColors.nearColor; });
                    xrFeedback.onNearExit.AddListener((XRController controller) => { spriteRenderer.color = xrUIColors.normalColor; });
                    break;
                }
            case VisualFeedbackType.Outline:
                {
                    xrFeedback.onNearEnter.AddListener((XRController controller) => { outline.OutlineColor = xrUIColors.nearColor; });
                    xrFeedback.onNearExit.AddListener((XRController controller) => { outline.OutlineColor = xrUIColors.normalColor; });
                    break;
                }
            default:
                break;
        }
    }

    private void SetTouchFeedbackListenersByType()
    {
        switch (feedbackType)
        {
            case VisualFeedbackType.MeshRenderer:
                {
                    xrFeedback.onTouchEnter.AddListener((XRController controller) => 
                    { 
                        meshRenderer.material.color = xrUIColors.touchColor; 
                    });

                    xrFeedback.onTouchExit.AddListener((XRController controller) =>
                    {
                        meshRenderer.material.color = runOnNear ? xrUIColors.nearColor : xrUIColors.normalColor;
                    });
                    break;
                }
            case VisualFeedbackType.SpriteRenderer:
                {
                    xrFeedback.onTouchEnter.AddListener((XRController controller) => 
                    { 
                        spriteRenderer.color = xrUIColors.touchColor; 
                    });
                    
                    xrFeedback.onTouchExit.AddListener((XRController controller) =>
                    {
                        spriteRenderer.color = runOnNear ? xrUIColors.nearColor : xrUIColors.normalColor;
                    });
                    break;
                }
            case VisualFeedbackType.Outline:
                {
                    xrFeedback.onTouchEnter.AddListener((XRController controller) => 
                    { 
                        outline.OutlineColor = xrUIColors.touchColor; 
                    });

                    xrFeedback.onTouchExit.AddListener((XRController controller) =>
                    {
                        outline.OutlineColor = runOnNear ? xrUIColors.nearColor : xrUIColors.normalColor;
                    });
                    break;
                }
            default:
                break;
        }
    }

    private void SetSelectFeedbackListenersByType()
    {
        switch (feedbackType)
        {
            case VisualFeedbackType.MeshRenderer:
                {
                    xrFeedback.onSelectEnter.AddListener((XRController controller) => 
                    {
                        meshRenderer.material.color = xrUIColors.selectColor;
                    });

                    xrFeedback.onSelectExit.AddListener((XRController controller) =>
                    {
                        if (runOnTouch)
                            meshRenderer.material.color = xrUIColors.touchColor;
                        else if (runOnNear)
                            meshRenderer.material.color = xrUIColors.nearColor;
                        else
                            meshRenderer.material.color = xrUIColors.normalColor;
                    });
                    break;
                }
            case VisualFeedbackType.SpriteRenderer:
                {
                    xrFeedback.onSelectEnter.AddListener((XRController controller) => 
                    { 
                        spriteRenderer.color = xrUIColors.selectColor; 
                    });

                    xrFeedback.onSelectExit.AddListener((XRController controller) =>
                    {
                        if (runOnTouch)
                            spriteRenderer.color = xrUIColors.touchColor;
                        else if (runOnNear)
                            spriteRenderer.color = xrUIColors.nearColor;
                        else
                            spriteRenderer.color = xrUIColors.normalColor;
                    });
                    break;
                }
            case VisualFeedbackType.Outline:
                {
                    xrFeedback.onSelectEnter.AddListener((XRController controller) => 
                    { 
                        outline.OutlineColor = xrUIColors.selectColor; 
                    });

                    xrFeedback.onSelectExit.AddListener((XRController controller) =>
                    {
                        if (runOnTouch)
                            outline.OutlineColor = xrUIColors.touchColor;
                        else if (runOnNear)
                            outline.OutlineColor = xrUIColors.nearColor;
                        else
                            outline.OutlineColor = xrUIColors.normalColor;
                    });
                    break;
                }
            default:
                break;
        }
    }


    protected override void ConfigureNearFeedback()
    {
        SetNearFeedbackListenersByType();
    }

    protected override void ConfigureTouchFeedback()
    {
        SetTouchFeedbackListenersByType();
    }

    protected override void ConfigureSelectFeedback()
    {
        SetSelectFeedbackListenersByType();
    }
}
