using UnityEngine;

public class XRUI_3DToggle : XRUI_ToggleBase
{
    public Material bodyMaterial;
    public Material selectMaterial;

    private MeshRenderer bodyMesh;
    private MeshRenderer selectMesh;

    protected override void SetRenderers()
    {
        if (bodyMesh == null)
            bodyMesh = bodyObject.GetComponent<MeshRenderer>();

        if (selectMesh == null)
            selectMesh = selectObject.GetComponent<MeshRenderer>();
    }

    protected override void UpdateColors()
    {
        bodyMaterial.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;

        if (IsSelected)
            selectMaterial.color = isEnabled ? selectedColor : xrUIColors.disabledColor;
        else
            selectMaterial.color = isEnabled ? unselectedColor : xrUIColors.disabledColor;

        if (bodyMesh != null)
            bodyMesh.sharedMaterial = bodyMaterial;
        if (selectMesh != null)
            selectMesh.sharedMaterial = selectMaterial;
    }
}
