using NaughtyAttributes;
using UnityEngine;

public class XRUI_3DButtonSprite : XRUI_2DButtonBase
{
    [Header("Internal Properties")]
    public Transform buttonBackground;
    public Transform buttonObject;

    [Header("Feedback Properties")]
    public bool backgroundFeedback = true;

    [HideIf("backgroundFeedback")]
    public Color buttonColor = Color.white;
    [HideIf("backgroundFeedback")]
    public Color buttonDisabledColor = Color.white;

    [Header("Icon Properties")]
    public SpriteRenderer iconSprite;
    [ShowIf("backgroundFeedback")]
    public Color iconColor = Color.white;
    [ShowIf("backgroundFeedback")]
    public Color iconDisabledColor = Color.white;
    [ShowAssetPreview(32, 32)]
    public Sprite icon;

    [ShowIf("HasIconSet")]
    [Range(0.001f, 1.0f)]
    public float iconScale = 0.1f;

    public bool HasIconSet() { return icon != null ? true : false; }

    [ReadOnly]
    public bool isPressed;

    private MeshRenderer buttonMesh;
    private Rigidbody buttonRigidBody;

    private float initialPos;
    private float distance = 1;
    private Vector3 initialPosition;

    protected override void OnValidate()
    {
        base.OnValidate();

        if (buttonMesh == null)
            buttonMesh = buttonObject.GetComponent<MeshRenderer>();

        buttonRigidBody = buttonObject.GetComponent<Rigidbody>();

        SetSpriteColors();
    }

    protected override void Awake()
    {
        base.Awake();

        buttonMesh = buttonObject.GetComponent<MeshRenderer>();
        buttonRigidBody = buttonObject.GetComponent<Rigidbody>();

        initialPosition = buttonObject.transform.localPosition;

        SetSpriteColors();
    }

    private void FixedUpdate()
    {
        ConstraintLocally();
    }


    void Update()
    {
        //ButtonLoop();
    }


    private void ConstraintLocally()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(buttonRigidBody.velocity);
        localVelocity.x = 0;
        localVelocity.y = 0;

        Vector3 localPosition = transform.InverseTransformPoint(buttonRigidBody.position);
        localPosition.x = 0;
        localPosition.y = 0;

        buttonRigidBody.velocity = transform.TransformDirection(localVelocity);
        buttonRigidBody.position = transform.TransformPoint(localPosition);

        //fix local rotation to zero
        buttonRigidBody.transform.localRotation = Quaternion.identity;
    }

    private void ButtonLoop()
    {
        distance = buttonObject.localPosition.z;

        if (distance <= 0)
        {
            buttonObject.localPosition = Vector3.zero;

            if (!isPressed)
            {
                isPressed = true;
                //onClickDown?.Invoke();
            }
        }
        else
        {
            if (distance > initialPos)
                buttonObject.localPosition = new Vector3(0, 0, initialPos);
            else
                buttonObject.localPosition = new Vector3(0, 0, buttonObject.localPosition.z);

            if (isPressed)
            {
                isPressed = false;
                //onClickUp?.Invoke();
            }
        }
    }

    private void RefreshSprite()
    {
        if (iconSprite == null || icon == null)
            return;

        iconSprite.transform.localScale = Vector3.one * iconScale;
        iconSprite.sprite = icon;
    }

    private void SetSpriteColors()
    {
        RefreshSprite();

        if (buttonMesh != null)
            buttonMesh.sharedMaterial = new Material(buttonMesh.sharedMaterial);

            if (backgroundFeedback)
        {
            xrUIColors.target = buttonMesh.transform;
            iconSprite.color = isEnabled ? iconColor : iconDisabledColor;
            buttonMesh.sharedMaterial.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
        }
        else
        {
            xrUIColors.target = iconSprite.transform;
            iconSprite.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
            buttonMesh.sharedMaterial.color = isEnabled ? buttonColor : buttonDisabledColor;
        }
    }
}
