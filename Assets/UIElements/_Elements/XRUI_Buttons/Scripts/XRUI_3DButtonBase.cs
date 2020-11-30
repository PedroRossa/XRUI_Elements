using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Base class of XRUI 3DButtons
/// </summary>
public class XRUI_3DButtonBase : XRUI_ButtonBase
{
    /// <summary>
    /// The button background transform
    /// </summary>
    [Header("Internal Properties")]
    public Transform buttonBackground;
    /// <summary>
    /// The transform of the button
    /// </summary>
    public Transform buttonObject;
    /// <summary>
    /// The material of the button
    /// </summary>
    public Material buttonMaterial;

    /// <summary>
    /// Is the button being pressed?
    /// </summary>
    [ReadOnly]
    public bool isPressed;
    /// <summary>
    /// Acts like a falling edge to the button 
    /// </summary>
    public bool isClicked = false;
    /// <summary>
    /// The mesh of the button
    /// </summary>
    [HideInInspector]
    public MeshRenderer buttonMesh;
    /// <summary>
    /// The rigidbody of the button
    /// </summary>
    protected Rigidbody buttonRigidBody;

    /// <summary>
    /// Is the button moving?
    /// </summary>
    [HideInInspector]
    public bool isMoving;

    protected override void OnValidate()
    {
        base.OnValidate();
        ConfigureButtonMaterial();
    }

    protected override void Awake()
    {
        base.Awake();
        ConfigureButtonMaterial();

        EventConfiguration();
        xrFeedback.XRInteractable.onSelectEnter.AddListener(
            (XRBaseInteractor interactor) =>
            {
                if (!isEnabled)
                    return;

                SimulateClick();
            }
        );
    }

    protected virtual void EventConfiguration()
    {
        onClickDown.AddListener(() => {
            if (canActiveButton && isEnabled)
            {
                buttonMesh.sharedMaterial.color = xrUIColors.selectColor;
                isClicked = true;
            }
        });
        onClickUp.AddListener(() => {
            if (canActiveButton && isEnabled)
            {
                buttonMesh.sharedMaterial.color = xrUIColors.normalColor;
                isClicked = false;
            }
        });
    }
    /// <summary>
    /// After the end of the frame, set that the button can't be actived and starts TimerTick() coroutine to can active again in the end
    /// </summary>
    /// <returns></returns>
    public IEnumerator ResetCanActiveButton()
    {
        canActiveButton = false;
        yield return new WaitForEndOfFrame();
        StartCoroutine(TimerTick());
    }

    private void FixedUpdate()
    {
        XRUI_Helper.ConstraintVelocityLocally(transform, buttonRigidBody, true, true, false);
        XRUI_Helper.ConstraintPositionLocally(transform, buttonRigidBody, true, true, false);
        buttonRigidBody.transform.localRotation = Quaternion.identity;
    }
    /// <summary>
    /// Method for setup the material of the button
    /// </summary>
    protected virtual void ConfigureButtonMaterial()
    {
        //Empty
    }

    /// <summary>
    /// Method that can be used on an inspector button to simulate a click in the button
    /// </summary>
    [Button]
    public void SimulateClick()
    {
        StartCoroutine(MoveButtonToColliderThenComeBack());
    }
    /// <summary>
    /// Coroutine to move the button till its collider
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveButtonToColliderThenComeBack()
    {
        if (isMoving)
            yield break;

        isMoving = true;

        Vector3 initialPosition = buttonObject.transform.position;

        //Move button to collider
        float elapsedTime = 0;
        const float duration = 0.2f;
        const float margin = duration + 0.02f;

        while (elapsedTime <= margin)
        {
            elapsedTime += Time.deltaTime;
            buttonObject.transform.position = Vector3.Lerp(buttonObject.transform.position,
                    buttonBackground.transform.position,
                    elapsedTime / duration
                    );
            yield return null;
        }

        elapsedTime = 0;
        Vector3 actualPos = buttonObject.transform.position;

        //Bring the button back
        while (elapsedTime < duration)
        {
            buttonObject.transform.position = Vector3.Lerp(actualPos, initialPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        buttonObject.transform.position = initialPosition;

        isMoving = false;
    }
}
