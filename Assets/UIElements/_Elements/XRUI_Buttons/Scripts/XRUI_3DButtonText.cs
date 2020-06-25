using NaughtyAttributes;
using UnityEngine;

public class XRUI_3DButtonText : XRUI_ButtonBase
{
    [Header("Internal Properties")]
    public Transform buttonBackground;
    public Transform buttonObject;

    public Material buttonMaterial;

    [ReadOnly]
    public bool isPressed;

    private MeshRenderer buttonMesh;
    private Rigidbody buttonRigidBody;

    protected override void OnValidate()
    {
        base.OnValidate();

        ConfigureButtonMaterial();
    }

    protected override void Awake()
    {
        base.Awake();

        ConfigureButtonMaterial();

        onClickDown.AddListener(() => { buttonMesh.sharedMaterial.color = xrUIColors.selectColor; });
        onClickUp.AddListener(() => { buttonMesh.sharedMaterial.color = xrUIColors.normalColor; });
    }

    private void FixedUpdate()
    {
        XRUI_Helper.ConstraintVelocityLocally(transform, buttonRigidBody, true, true, false);
        XRUI_Helper.ConstraintPositionLocally(transform, buttonRigidBody, true, true, false);
        buttonRigidBody.transform.localRotation = Quaternion.identity;
        buttonObject.localPosition = new Vector3(0, 0, buttonObject.localPosition.z);
    }

    private void ConfigureButtonMaterial()
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
