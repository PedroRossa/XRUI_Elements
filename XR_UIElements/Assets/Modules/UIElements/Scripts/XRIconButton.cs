using NaughtyAttributes;
using UnityEngine;

public class XRIconButton : XRButton
{
    [Header("Icon Properties")]
    [ShowAssetPreview(32, 32)]
    public Sprite icon;
    public Color iconColor = Color.white;

    //Runs only in editor
    private void OnValidate()
    {
        BaseOnValidate();
        
        if (icon != null)
        {
            //Get the spriterenderer on child to set the new sprite when changed
            SpriteRenderer buttonIcon = frontPanel.GetComponentsInChildren<SpriteRenderer>()[1];
            buttonIcon.sprite = icon;
            buttonIcon.color = iconColor;
        }
    }
}