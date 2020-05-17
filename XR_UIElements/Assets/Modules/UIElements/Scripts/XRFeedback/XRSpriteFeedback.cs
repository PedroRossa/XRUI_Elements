using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(SpriteRenderer))]
public class XRSpriteFeedback : XRBaseFeedback
{
    private SpriteRenderer elementSprite;

    protected override void Awake()
    {
        base.Awake();
        elementSprite = GetComponent<SpriteRenderer>();

        originalColor = elementSprite.color;
    }


    protected override void SetColor(Color color)
    {
        elementSprite.color = color;
    }
}
