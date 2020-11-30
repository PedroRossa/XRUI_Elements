using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// 2D Progress Bar controller
/// </summary>
public class XRUI_2DProgressBar : XRUI_ProgressBarBase
{
    private SpriteRenderer backgroundSprite;
    private SpriteRenderer totalProgressSprite;
    private SpriteRenderer progressSprite;
    private SpriteRenderer progressPointSprite;
    private SpriteRenderer currentProgressSprite;

    protected override void Awake()
    {
        base.Awake();

        xrFeedback.onTouchEnter.AddListener((XRController xrController) =>
        {
            progressPointSprite.transform.localScale *= 1.1f; //increase the scale in 10%
        });

        xrFeedback.onTouchExit.AddListener((XRController xrController) =>
        {
            progressPointSprite.transform.localScale /= 1.1f; //decrease the scale in 10%
        });
    }

    protected override void UpdateColors()
    {
        if (backgroundSprite != null)
            backgroundSprite.color = backgroundColor;

        if (totalProgressSprite != null)
            totalProgressSprite.color = totalProgressColor;

        if (currentProgressSprite != null)
            currentProgressSprite.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
    }

    protected override void SetElementReferences()
    {
        if (background != null)
            backgroundSprite = background.GetComponent<SpriteRenderer>();

        if (totalProgress != null)
            totalProgressSprite = totalProgress.GetComponent<SpriteRenderer>();

        if (progressElement != null && progressElement.childCount > 0)
            progressSprite = progressElement.GetChild(0).GetComponent<SpriteRenderer>();

        if (progressPointElement != null && progressPointElement.childCount > 0)
            progressPointSprite = progressPointElement.GetChild(0).GetComponent<SpriteRenderer>();

        if (currentProgressSprite == null)
            currentProgressSprite = progressElement.GetComponentInChildren<SpriteRenderer>();
    }
}
