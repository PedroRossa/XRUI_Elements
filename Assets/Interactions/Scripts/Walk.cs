﻿using UnityEngine;

public class Walk : MonoBehaviour
{
    public short translationSpeed = 250;
    public short rotationSpeed = 50;
    public Transform cameraTransform;
    // Update is called once per frame

    private Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {
         rigidbody.velocity = DirectionVector() * Time.deltaTime * translationSpeed *
           OculusInput.LeftHandVerticalAxis;

        transform.eulerAngles += Vector3.up * Time.deltaTime * rotationSpeed *
            OculusInput.LeftHandHorizontalAxis;
    }

    Vector3 DirectionVector()
    {
        return new Vector3(Mathf.Sin(cameraTransform.eulerAngles.y * Mathf.Deg2Rad), 0, 
            Mathf.Cos(cameraTransform.eulerAngles.y * Mathf.Deg2Rad));
    }

    private void OnDisable()
    {
        rigidbody.velocity = new Vector3(0,0,0);
    }
}