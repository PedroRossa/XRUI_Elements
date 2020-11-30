using UnityEngine;

/// <summary>
/// 3D Slider controller
/// </summary>
public class XRUI_3DSlider : XRUI_ProgressBarBase
{
    private MeshRenderer backgroundMesh;
    private MeshRenderer sliderElementMesh;

    public Material backgroundMaterial;
    public Material elementMaterial;

    protected override void UpdateColors()
    {
        if (backgroundMesh != null)
        {
            backgroundMesh.sharedMaterial = new Material(backgroundMaterial);
            backgroundMesh.sharedMaterial.color = backgroundColor;
        }
        if (sliderElementMesh != null)
        {
            sliderElementMesh.sharedMaterial = new Material(elementMaterial);
            sliderElementMesh.sharedMaterial.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
        }
    }

    protected override void SetElementReferences()
    {
        if (backgroundMesh == null && background != null)
            backgroundMesh = background.GetComponent<MeshRenderer>();

        if (sliderElementMesh == null && progressPointElement != null && progressPointElement.childCount > 0)
            sliderElementMesh = progressPointElement.GetChild(0).GetComponent<MeshRenderer>();

        backgroundMaterial.color = backgroundColor;
        elementMaterial.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;

        backgroundMesh.material = backgroundMaterial;
        sliderElementMesh.material = elementMaterial;
    }
}
