using NaughtyAttributes;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRUI_FeedbackColor : XRUI_FeedbackBaseType
{
    public enum VisualFeedbackType
    {
        MeshRenderer = 1,
        SpriteRenderer = 2,
        Outline = 3
    }

    public VisualFeedbackType feedbackType = VisualFeedbackType.Outline;

    public XRUI_Colors xrUIColors;

    private MeshRenderer meshRenderer;
    private SpriteRenderer spriteRenderer;
    private Outline outline;

    private void OnValidate()
    {
        LookForXRUIColor();
        CheckComponentsByType();
        RefreshElementColor();
    }

    protected override void Awake()
    {
        base.Awake();

        LookForXRUIColor();
        RefreshElementColor();
        InitializeByType();
    }

    private void LookForXRUIColor()
    {
        if (xrUIColors != null)
            return;

        xrUIColors = GetComponentInChildren<XRUI_Colors>(); //try to find a XRUIColors on object or any child element

        if (xrUIColors == null)
            throw new System.Exception("XR Feedback Color needs a XRUIColor reference to work properly.");
    }

    private void CheckComponentsByType()
    {
        if (xrFeedback == null)
            xrFeedback = GetComponent<XRUI_Feedback>();

        if(meshRenderer == null)
            meshRenderer = xrUIColors.target.GetComponent<MeshRenderer>();
        
        if(spriteRenderer == null)
            spriteRenderer = xrUIColors.target.GetComponent<SpriteRenderer>();

        if (outline == null)
            outline = xrUIColors.target.GetComponent<Outline>();

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
            outline = xrUIColors.target.gameObject.AddComponent<Outline>();
    }

    public void RefreshElementColor()
    {
        switch (feedbackType)
        {
            case VisualFeedbackType.MeshRenderer:
                if (meshRenderer != null && meshRenderer.sharedMaterial != null)
                    meshRenderer.sharedMaterial.color = xrFeedback.isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
                
                break;
            case VisualFeedbackType.SpriteRenderer:
                if (spriteRenderer != null)
                    spriteRenderer.color = xrFeedback.isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
               
                break;
            case VisualFeedbackType.Outline:
                if(outline!= null)
                    outline.OutlineColor = xrFeedback.isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;

                break;
            default:
                break;
        }
    }

    [Button]
    public void CreateXRUIColors()
    {
        LookForXRUIColor();

        if (xrUIColors == null)
            xrUIColors = gameObject.AddComponent<XRUI_Colors>();
        else
            Debug.Log("You already have a XRUIColors on your game object or a child of it");
    }

    protected override void OnNearEnterFeedbackFunction(XRController controller)
    {
        switch (feedbackType)
        {
            case VisualFeedbackType.MeshRenderer:
                meshRenderer.material.color = xrUIColors.nearColor;
                break;
            case VisualFeedbackType.SpriteRenderer:
                spriteRenderer.color = xrUIColors.nearColor;
                break;
            case VisualFeedbackType.Outline:
                outline.OutlineColor = xrUIColors.nearColor;
                break;
            default:
                break;
        }
    }

    protected override void OnNearExitFeedbackFunction(XRController controller)
    {
        switch (feedbackType)
        {
            case VisualFeedbackType.MeshRenderer:
                meshRenderer.material.color = xrUIColors.normalColor;
                break;
            case VisualFeedbackType.SpriteRenderer:
                spriteRenderer.color = xrUIColors.normalColor;
                break;
            case VisualFeedbackType.Outline:
                outline.OutlineColor = xrUIColors.normalColor;
                break;
            default:
                break;
        }
    }

    protected override void OnTouchEnterFeedbackFunction(XRController controller)
    {
        switch (feedbackType)
        {
            case VisualFeedbackType.MeshRenderer:
                meshRenderer.material.color = xrUIColors.touchColor;
                break;
            case VisualFeedbackType.SpriteRenderer:
                spriteRenderer.color = xrUIColors.touchColor;
                break;
            case VisualFeedbackType.Outline:
                outline.OutlineColor = xrUIColors.touchColor;
                break;
            default:
                break;
        }
    }

    protected override void OnTouchExitFeedbackFunction(XRController controller)
    {
        switch (feedbackType)
        {
            case VisualFeedbackType.MeshRenderer:

                if (runOnNear && xrFeedback.IsNear)
                    meshRenderer.material.color = xrUIColors.nearColor;
                else
                    meshRenderer.material.color = xrUIColors.normalColor;

                break;
            case VisualFeedbackType.SpriteRenderer:

                if (runOnNear && xrFeedback.IsNear)
                    spriteRenderer.color = xrUIColors.nearColor;
                else
                    spriteRenderer.color = xrUIColors.normalColor;

                break;
            case VisualFeedbackType.Outline:

                if (runOnNear && xrFeedback.IsNear)
                    outline.OutlineColor = xrUIColors.nearColor;
                else
                    outline.OutlineColor = xrUIColors.normalColor;

                break;
            default:
                break;
        }
    }

    protected override void OnSelectEnterFeedbackFunction(XRController controller)
    {
        switch (feedbackType)
        {
            case VisualFeedbackType.MeshRenderer:
                meshRenderer.material.color = xrUIColors.selectColor;
                break;
            case VisualFeedbackType.SpriteRenderer:
                spriteRenderer.color = xrUIColors.selectColor;
                break;
            case VisualFeedbackType.Outline:
                outline.OutlineColor = xrUIColors.selectColor;
                break;
            default:
                break;
        }
    }

    protected override void OnSelectExitFeedbackFunction(XRController controller)
    {
        switch (feedbackType)
        {
            case VisualFeedbackType.MeshRenderer:

                if (runOnTouch && xrFeedback.IsTouching)
                    meshRenderer.material.color = xrUIColors.touchColor;
                else if (runOnNear && xrFeedback.IsNear)
                    meshRenderer.material.color = xrUIColors.nearColor;
                else
                    meshRenderer.material.color = xrUIColors.normalColor;

                break;
            case VisualFeedbackType.SpriteRenderer:

                if (runOnTouch && xrFeedback.IsTouching)
                    spriteRenderer.color = xrUIColors.touchColor;
                else if (runOnNear && xrFeedback.IsNear)
                    spriteRenderer.color = xrUIColors.nearColor;
                else
                    spriteRenderer.color = xrUIColors.normalColor;

                break;
            case VisualFeedbackType.Outline:

                if (runOnTouch && xrFeedback.IsTouching)
                    outline.OutlineColor = xrUIColors.touchColor;
                else if (runOnNear && xrFeedback.IsNear)
                    outline.OutlineColor = xrUIColors.nearColor;
                else
                    outline.OutlineColor = xrUIColors.normalColor;

                break;
            default:
                break;
        }
    }
}
