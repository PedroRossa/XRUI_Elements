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
    [HideInInspector]
    public Color memoryColor;

    private bool isOutLineType = true;
    private bool nearEnter;

    [ShowIf("isOutLineType")]
    [ValidateInput("ValideteOutLine")]
    public Outline outline;

    private void OnValidate()
    {
        if (xrFeedback == null)
            xrFeedback = GetComponent<XRUI_Feedback>();

        LookForXRUIColor();
        CheckTargetType();
        RefreshElementColor();
    }
    private bool ValideteOutLine(Outline _outline)
    {
        return (!isOutLineType || _outline != null);
    }

    protected override void Awake()
    {
        base.Awake();

        if (xrFeedback == null)
            xrFeedback = GetComponent<XRUI_Feedback>();

        LookForXRUIColor();
        RefreshElementColor();
        CheckTargetType();
        LookForOutline();
    }

    private void LookForXRUIColor()
    {
        if (xrUIColors != null)
            return;

        xrUIColors = GetComponentInChildren<XRUI_Colors>(); //try to find a XRUIColors on object or any child element

        if (xrUIColors == null)
            throw new System.Exception("XR Feedback Color needs a XRUIColor reference to work properly.");
    }

    private void LookForOutline()
    {
        if (outline != null || !isOutLineType)
            return;

        outline = GetComponentInChildren<Outline>();
    }
    private void CheckTargetType()
    {
        if (xrUIColors == null)
            return;

        isOutLineType = false;
        if (feedbackType == VisualFeedbackType.Outline)
        {
            isOutLineType = true;
            if (outline == null && xrUIColors != null)
                outline = xrUIColors.target.GetComponent<Outline>();

            return;
        }

        meshRenderer = xrUIColors.target.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            feedbackType = VisualFeedbackType.MeshRenderer;
            return;
        }

        spriteRenderer = xrUIColors.target.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            feedbackType = VisualFeedbackType.SpriteRenderer;
            return;
        }
        feedbackType = VisualFeedbackType.MeshRenderer;
    }

    public void RefreshElementColor()
    {
        //its here because of running on editor its not updated on awake
        if (xrFeedback == null)
            xrFeedback = GetComponent<XRUI_Feedback>();

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
                if (outline != null)
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

    [Button]
    public void CreateOutline()
    {
        if (!isOutLineType || outline != null)
            return;
        if (outline == null && xrUIColors != null)
            outline = xrUIColors.target.gameObject.AddComponent<Outline>();
    }
    protected override void OnNearEnterFeedbackFunction(XRController controller)
    {
        switch (feedbackType)
        {
            case VisualFeedbackType.MeshRenderer:
                if (!nearEnter)
                    memoryColor = meshRenderer.material.color;
                meshRenderer.material.color = xrUIColors.nearColor;
                break;
            case VisualFeedbackType.SpriteRenderer:
                if (!nearEnter)
                    memoryColor = spriteRenderer.color;
                spriteRenderer.color = xrUIColors.nearColor;
                break;
            case VisualFeedbackType.Outline:
                if (outline != null && xrUIColors != null)
                {
                    if (!nearEnter)
                        memoryColor = outline.OutlineColor;
                    outline.OutlineColor = xrUIColors.nearColor;
                }
                break;
            default:
                break;
        }
        nearEnter = true;
    }

    protected override void OnNearExitFeedbackFunction(XRController controller)
    {
        nearEnter = false;
        switch (feedbackType)
        {
            case VisualFeedbackType.MeshRenderer:
                meshRenderer.material.color = memoryColor;
                break;
            case VisualFeedbackType.SpriteRenderer:
                spriteRenderer.color = memoryColor;
                break;
            case VisualFeedbackType.Outline:
                if (outline != null && xrUIColors != null)
                    outline.OutlineColor = memoryColor;
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
                if (outline != null && xrUIColors != null)
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
                    meshRenderer.material.color = memoryColor;

                break;
            case VisualFeedbackType.SpriteRenderer:

                if (runOnNear && xrFeedback.IsNear)
                    spriteRenderer.color = xrUIColors.nearColor;
                else
                    spriteRenderer.color = memoryColor;

                break;
            case VisualFeedbackType.Outline:

                if (outline != null && xrUIColors != null)
                {
                    if (runOnNear && xrFeedback.IsNear)
                        outline.OutlineColor = xrUIColors.nearColor;
                    else
                        outline.OutlineColor = memoryColor;
                }
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
                if (outline != null && xrUIColors != null)
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
                    meshRenderer.material.color = memoryColor;

                break;
            case VisualFeedbackType.SpriteRenderer:

                if (runOnTouch && xrFeedback.IsTouching)
                    spriteRenderer.color = xrUIColors.touchColor;
                else if (runOnNear && xrFeedback.IsNear)
                    spriteRenderer.color = xrUIColors.nearColor;
                else
                    spriteRenderer.color = memoryColor;

                break;
            case VisualFeedbackType.Outline:

                if (outline != null && xrUIColors != null)
                {
                    if (runOnTouch && xrFeedback.IsTouching)
                        outline.OutlineColor = xrUIColors.touchColor;
                    else if (runOnNear && xrFeedback.IsNear)
                        outline.OutlineColor = xrUIColors.nearColor;
                    else
                        outline.OutlineColor = memoryColor;
                }
                break;
            default:
                break;
        }
    }
}
