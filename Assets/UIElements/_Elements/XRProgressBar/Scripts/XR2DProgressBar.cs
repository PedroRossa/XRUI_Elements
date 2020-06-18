using System.Dynamic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XR2DProgressBar : XRProgressBarBase
{
    private SpriteRenderer backgroundSprite;
    private SpriteRenderer totalProgressSprite;
    private SpriteRenderer progressSprite;
    private SpriteRenderer progressPointSprite;

    protected override void OnValidate()
    {
        base.OnValidate();
        GetSpriteReferences();
    }

    protected override void Awake()
    {
        base.Awake();
        GetSpriteReferences();
    }

    protected override void UpdateColors()
    {
        if (backgroundSprite != null)
            backgroundSprite.color = backgroundColor;

        if (totalProgressSprite != null)
            totalProgressSprite.color = totalProgressColor;

        if (progressSprite != null)
            progressSprite.color = isEnabled ? normalColor : disabledColor;

        if (progressPointSprite != null)
            progressPointSprite.color = isEnabled ? normalColor : disabledColor;
    }

    private void GetSpriteReferences()
    {
        if (background != null)
            backgroundSprite = background.GetComponent<SpriteRenderer>();

        if (totalProgress != null)
            totalProgressSprite = totalProgress.GetComponent<SpriteRenderer>();

        if (progressElement != null && progressElement.childCount > 0)
            progressSprite = progressElement.GetChild(0).GetComponent<SpriteRenderer>();

        if (progressPointElement != null && progressPointElement.childCount > 0)
            progressPointSprite = progressPointElement.GetChild(0).GetComponent<SpriteRenderer>();
    }

    protected override void WhenTouchStart(XRBaseInteractor interactor)
    {
        base.WhenTouchStart(interactor);
    }

    protected override void WhenTouchEnd(XRBaseInteractor interactor)
    {
        base.WhenTouchEnd(interactor);
    }
}
