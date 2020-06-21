using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class XRButton : XRUIBase
{
    [Header("Internal Properties")]
    public Transform backgroundPanel;
    public Transform buttonTransform;

    public Color backgroundColor = Color.white;
    public Color clickColor = Color.green;

    [ReadOnly]
    public bool isPressed;

    [Header("Events")]
    public UnityEvent onClickDown;
    public UnityEvent onClickPress;
    public UnityEvent onClickUp;

    private Rigidbody buttonRigidBody;

    private float initialPos;
    private float distance = 1;

    protected override void OnValidate()
    {
        base.OnValidate();
        backgroundPanel.GetComponent<SpriteRenderer>().color = backgroundColor;

        MeshRenderer mr = buttonTransform.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            mr.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
            mr.sharedMaterial.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;
        }
    }

    void Start()
    {
        onClickDown.AddListener(OnClickDownFucntion);
        onClickPress.AddListener(OnClickPressFunction);
        onClickUp.AddListener(OnClickUpFucntion);

        initialPos = buttonTransform.localPosition.z;

        buttonRigidBody = buttonTransform.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ConstraintLocally();
    }

    void Update()
    {
        ButtonLoop();
    }

    private void ButtonLoop()
    {
           distance = buttonTransform.localPosition.z;

        if (distance <= 0)
        {
            buttonTransform.localPosition = Vector3.zero;

            if (!isPressed)
            {
                isPressed = true;
                onClickDown?.Invoke();
            }
            else
            {
                onClickPress?.Invoke();
            }
        }
        else
        {
            if (distance > initialPos)
                buttonTransform.localPosition = new Vector3(0, 0, initialPos);
            else
                buttonTransform.localPosition = new Vector3(0, 0, buttonTransform.localPosition.z);

            if (isPressed)
            {
                isPressed = false;
                onClickUp?.Invoke();
            }
        }
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


    private void OnClickDownFucntion()
    {
        isPressed = true;
        buttonTransform.GetComponent<MeshRenderer>().sharedMaterial.color = clickColor;
    }

    private void OnClickPressFunction()
    {
    }

    private void OnClickUpFucntion()
    {
        isPressed = false;
        buttonTransform.GetComponent<MeshRenderer>().sharedMaterial.color = xrUIColors.normalColor;
    }
}
