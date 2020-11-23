using UnityEngine;

/// <summary>
/// XRUI 3DButton identified by Text
/// </summary>
public class XRUI_3DButtonText : XRUI_3DButtonBase
{
    protected override void OnValidate()
    {
        base.OnValidate();

        ConfigureButtonMaterial();
    }

    protected override void Awake()
    {
        base.Awake();

        ConfigureButtonMaterial();

        onClickDown.AddListener(() =>
        {
            if (!isEnabled)
                return;

            buttonMesh.sharedMaterial.color = xrUIColors.selectColor;
        }
        );
        onClickUp.AddListener(() =>
        {
            if (!isEnabled)
                return;

            buttonMesh.sharedMaterial.color = xrUIColors.normalColor;
        }
        );
    }


    protected override void ConfigureButtonMaterial()
    {
        buttonMesh = buttonObject.GetComponent<MeshRenderer>();
        buttonRigidBody = buttonObject.GetComponent<Rigidbody>();

        if (buttonMesh == null)
            return;

        buttonMesh.sharedMaterial = new Material(buttonMaterial);

        buttonMaterial.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
        buttonMesh.material = buttonMaterial;

        buttonMesh.sharedMaterial = new Material(buttonMesh.sharedMaterial);

        xrUIColors.target = buttonMesh.transform;
        buttonMesh.sharedMaterial.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
    }
}
