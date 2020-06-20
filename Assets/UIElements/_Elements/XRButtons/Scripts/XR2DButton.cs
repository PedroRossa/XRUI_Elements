using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(XRTouchManagement))]
public abstract class XR2DButton : XRUIBase
{
    public bool backgroundFeedback;
    public SpriteRenderer backgroundSprite;

    [HideIf("backgroundFeedback")]
    public Color32 backgroundColor = new Color32(255, 255, 255, 255);

    protected XRTouchManagement touchManagement;


    protected override void Awake()
    {
        base.Awake();

        touchManagement = GetComponent<XRTouchManagement>();
        touchManagement.ConfigureSound(audioSource);
    }
}
