using NaughtyAttributes;
using UnityEngine;

public class XRSpriteButton : XRButton
{
    [Header("Sprite Properties")]
    [ShowAssetPreview(32, 32)]
    public Sprite sprite;
    public Color spriteColor = Color.white;

    //Runs only in editor
    private void OnValidate()
    {
        BaseOnValidate();
        
        if (sprite != null)
        {
            //Get the spriterenderer on child to set the new sprite when changed
            SpriteRenderer buttonIcon = frontPanel.GetComponentsInChildren<SpriteRenderer>()[1];
            buttonIcon.sprite = sprite;
            buttonIcon.color = spriteColor;
        }
    }
}