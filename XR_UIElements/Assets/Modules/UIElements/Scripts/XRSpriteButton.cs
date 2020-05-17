using NaughtyAttributes;
using UnityEngine;

public class XRSpriteButton : XRButton
{
    [Header("Sprite Properties")]
    [ShowAssetPreview(32, 32)]
    public Sprite sprite;
    public Color spriteColor = Color.white;

    private SpriteRenderer buttonSpriteRenderer;

    //Runs only in editor
    private void OnValidate()
    {
        BaseOnValidate();

        if (sprite != null)
        {
            //Get the spriterenderer on child to set the new sprite when changed
            if (buttonSpriteRenderer == null)
                buttonSpriteRenderer = frontPanel.GetComponentsInChildren<SpriteRenderer>()[1];

            if (buttonSpriteRenderer != null)
            {
                buttonSpriteRenderer.sprite = sprite;
                buttonSpriteRenderer.color = spriteColor;
            }
        }
    }

    private void Awake()
    {
        if (buttonSpriteRenderer == null)
            buttonSpriteRenderer = frontPanel.GetComponentsInChildren<SpriteRenderer>()[1];
    }

    public void SetSprite(Sprite sprite)
    {
        if (buttonSpriteRenderer != null)
            buttonSpriteRenderer.sprite = sprite;
    }
}