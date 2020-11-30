using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Controller of Feedback Color from Armor Menu
/// </summary>
public class XRUI_ArmorFeedbackColor : XRUI_FeedbackColor
{
    public XRUI_ArmorFeedbackColor[] otherButtons;

    private void Start()
    {
        RefreshElementColor();
    }
    protected override void OnSelectEnterFeedbackFunction(XRController controller)
    {
        switch (feedbackType)
        {
            case VisualFeedbackType.MeshRenderer:
                meshRenderer.material.color = ReturnNormalOrSelectedColor(meshRenderer.material.color);
                break;
            case VisualFeedbackType.SpriteRenderer:
                spriteRenderer.color = ReturnNormalOrSelectedColor(spriteRenderer.color);
                break;
            case VisualFeedbackType.Outline:
                if (outline != null && xrUIColors != null)
                    outline.OutlineColor = ReturnNormalOrSelectedColor(outline.OutlineColor);
                break;
            default:
                break;
        }
        foreach (var button in otherButtons)
        {
            switch (button.feedbackType)
            {
                case VisualFeedbackType.MeshRenderer:
                    meshRenderer.material.color = xrUIColors.normalColor;
                    break;
                case VisualFeedbackType.SpriteRenderer:
                    spriteRenderer.color = xrUIColors.normalColor;
                    break;
                case VisualFeedbackType.Outline:
                    if (outline != null && xrUIColors != null)
                        outline.OutlineColor = xrUIColors.normalColor;
                    break;
                default:
                    break;
            }
        }
    }
}
