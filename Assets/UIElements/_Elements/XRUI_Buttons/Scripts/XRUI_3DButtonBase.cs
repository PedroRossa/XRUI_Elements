﻿using NaughtyAttributes;
using System.Collections;
using System.Threading;
using UnityEngine;

public class XRUI_3DButtonBase : XRUI_ButtonBase
{
    [Header("Internal Properties")]
    public Transform buttonBackground;
    public Transform buttonObject;

    public Material buttonMaterial;

    [ReadOnly]
    public bool isPressed;

    public bool isClicked = false;

    protected MeshRenderer buttonMesh;
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

        onClickDown.AddListener(() => { buttonMesh.sharedMaterial.color = xrUIColors.selectColor; isClicked = true; });
        onClickUp.AddListener(() => { buttonMesh.sharedMaterial.color = xrUIColors.normalColor; isClicked = false; });
    }

    private void FixedUpdate()
    {
        XRUI_Helper.ConstraintVelocityLocally(transform, buttonRigidBody, true, true, false);
        XRUI_Helper.ConstraintPositionLocally(transform, buttonRigidBody, true, true, false);
        buttonRigidBody.transform.localRotation = Quaternion.identity;
        buttonObject.localPosition = new Vector3(0, 0, buttonObject.localPosition.z);
    }

    protected virtual void ConfigureButtonMaterial()
    {

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

        Vector3 end = buttonObject.transform.position - (direction * (Vector3.Distance(buttonObject.transform.position,buttonBackground.transform.position) - (buttonMesh.bounds.size.z * 0.4f)));


        while (elapsedTime < seconds && !isClicked)
        {
            //Debug.Log(isClicked);
            buttonObject.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //Debug.Log(isClicked);
        yield return null;
    }

}