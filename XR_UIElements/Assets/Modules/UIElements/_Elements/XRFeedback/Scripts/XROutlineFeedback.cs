using UnityEngine;

[RequireComponent(typeof(Outline))]
public class XROutlineFeedback : XRBaseFeedback
{
    private Outline outline;

    protected override void Awake()
    {
        base.Awake();

        outline = GetComponent<Outline>();
        outline.enabled = false;

        onProximityAreaEnter.AddListener(() => outline.enabled = true);
        onProximityAreaExit.AddListener(() => outline.enabled = false);
    }

    public override void SetColor(Color color)
    {
        if (outline != null)
            outline.OutlineColor = color;
    }
}
