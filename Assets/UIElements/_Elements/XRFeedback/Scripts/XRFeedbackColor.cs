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
                meshRenderer.material.color = runOnNear ? xrUIColors.nearColor : xrUIColors.normalColor;
                break;
            case VisualFeedbackType.SpriteRenderer:
                spriteRenderer.color = runOnNear ? xrUIColors.nearColor : xrUIColors.normalColor;
                break;
            case VisualFeedbackType.Outline:
                outline.OutlineColor = runOnNear ? xrUIColors.nearColor : xrUIColors.normalColor;
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

                if (runOnTouch)
                    meshRenderer.material.color = xrUIColors.touchColor;
                else if (runOnNear)
                    meshRenderer.material.color = xrUIColors.nearColor;
                else
                    meshRenderer.material.color = xrUIColors.normalColor;

                break;
            case VisualFeedbackType.SpriteRenderer:

                if (runOnTouch)
                    spriteRenderer.color = xrUIColors.touchColor;
                else if (runOnNear)
                    spriteRenderer.color = xrUIColors.nearColor;
                else
                    spriteRenderer.color = xrUIColors.normalColor;

                break;
            case VisualFeedbackType.Outline:

                if (runOnTouch)
                    outline.OutlineColor = xrUIColors.touchColor;
                else if (runOnNear)
                    outline.OutlineColor = xrUIColors.nearColor;
                else
                    outline.OutlineColor = xrUIColors.normalColor;

                break;
            default:
                break;
        }
    }
}
