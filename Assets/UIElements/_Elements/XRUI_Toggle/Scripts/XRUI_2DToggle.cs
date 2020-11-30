﻿using UnityEngine;

/// <summary>
/// 2D toggle XRUI component
/// </summary>
public class XRUI_2DToggle : XRUI_ToggleBase
{
    private SpriteRenderer bodySprite;
    private SpriteRenderer selectSprite;

    protected override void SetRenderers()
    {
        if (bodySprite == null)
            bodySprite = bodyObject.GetComponent<SpriteRenderer>();

        if (selectSprite == null)
            selectSprite = selectObject.GetComponent<SpriteRenderer>();
    }

    protected override void UpdateColors()
    {
        if (bodySprite != null)
        {
            if (changeBGColor)
            {
                xrUIColors.normalColor = IsSelected ? selectedBGColor : unselectedBGColor;
                bodySprite.color = IsSelected ? selectedBGColor : unselectedBGColor;
            }
            else
                bodySprite.color = xrUIColors.normalColor;
        }

        if (selectSprite != null)
        {
            if (IsSelected)
                selectSprite.color = isEnabled ? selectedColor : xrUIColors.disabledColor;
            else
                selectSprite.color = isEnabled ? unselectedColor : xrUIColors.disabledColor;
        }
    }
}
