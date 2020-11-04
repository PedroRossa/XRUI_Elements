using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Base class of XRUI 3DButtons
/// </summary>
public class XRUI_3DButtonBase : XRUI_ButtonBase
{
    [Header("Internal Properties")]
    public Transform buttonBackground;
    public Transform buttonObject;

    public Material buttonMaterial;
    /// <summary>
    /// Others buttons to deselect when this one is selected 
    /// </summary>
    public XRUI_3DButtonBase[] buttonsToDisableOnClickUp;

    [ReadOnly]
    public bool isPressed;

    public bool isClicked = false;
    [HideInInspector]
    public bool isOn;

    [HideInInspector]
    public MeshRenderer buttonMesh;
    protected Rigidbody buttonRigidBody;

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
        xrFeedback.XRInteractable.onSelectEnter.AddListener((XRBaseInteractor interactor) => { SimulateClick(); });
    }

    protected virtual void EventConfiguration()
    {
        onClickDown.AddListener(() => {
            if (canActiveButton)
            {
                buttonMesh.sharedMaterial.color = xrUIColors.selectColor; isClicked = true;
            }
        });
        onClickUp.AddListener(() => {
            if (canActiveButton)
            {
                buttonMesh.sharedMaterial.color = xrUIColors.normalColor; isClicked = false;
            }
        });
    }

    public IEnumerator resetCanActiveButton()
    {
        yield return new WaitForEndOfFrame();
        canActiveButton = false;
        StartCoroutine(TimerTick());
    }

    private void FixedUpdate()
    {
        XRUI_Helper.ConstraintVelocityLocally(transform, buttonRigidBody, true, true, false);
        XRUI_Helper.ConstraintPositionLocally(transform, buttonRigidBody, true, true, false);
        buttonRigidBody.transform.localRotation = Quaternion.identity;
    }

    protected virtual void ConfigureButtonMaterial()
    {
        //Empty
    }

    [Button]
    public void SimulateClick()
    {
        StartCoroutine(MoveButtonToCollider());
    }
    private IEnumerator MoveButtonToCollider()
    {
        float seconds = 0.2f;
        float elapsedTime = 0;
        Vector3 startingPos = buttonObject.transform.position;
        Vector3 direction = (buttonObject.transform.position - buttonBackground.transform.position).normalized;
        Vector3 end = buttonObject.transform.position - (direction * (Vector3.Distance(buttonObject.transform.position, buttonBackground.transform.position) - (buttonMesh.bounds.size.z * 0.35f)));

        while (elapsedTime < seconds && !isClicked)
        {
            buttonObject.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
