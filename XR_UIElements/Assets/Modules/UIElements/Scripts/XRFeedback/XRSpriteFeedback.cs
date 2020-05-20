using UnityEngine;

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


    public override void SetColor(Color color)
    {
        if (elementSprite != null)
            elementSprite.color = color;
    }
}
