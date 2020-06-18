using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSlider : XRProgressBarBase
{
    public Material backgroundMaterial;
    public Material sliderElementMaterial;

    private MeshRenderer backgroundMesh;
    private MeshRenderer sliderElementMesh;

    protected override void OnValidate()
    {
        base.OnValidate();
        GetMeshReferences();
    }

    protected override void Awake()
    {
        base.Awake();
        GetMeshReferences();
    }

    protected override void UpdateColors()
    {
        if (backgroundMesh != null && backgroundMesh.sharedMaterial != null)
            backgroundMesh.sharedMaterial.color = backgroundColor;

        if (sliderElementMesh != null && sliderElementMesh.sharedMaterial != null)
            sliderElementMesh.sharedMaterial.color = isEnabled ? normalColor : disabledColor;
    }

    private void GetMeshReferences()
    {
        if (backgroundMesh == null && background != null)
        {
            backgroundMesh = background.GetComponent<MeshRenderer>();

            if (backgroundMaterial != null && backgroundMesh.sharedMaterial == null)
                backgroundMesh.sharedMaterial = new Material(backgroundMaterial);
        }

        if (sliderElementMesh == null && progressPointElement != null && progressPointElement.childCount > 0)
        {
            sliderElementMesh = progressPointElement.GetChild(0).GetComponent<MeshRenderer>();

            if (sliderElementMaterial != null && sliderElementMesh.sharedMaterial == null)
                sliderElementMesh.sharedMaterial = new Material(sliderElementMaterial);
        }
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
