﻿using UnityEngine;

/// <summary>
/// Class that permits a locomotion system with one of the joysticks thumbstick through xr rig rigidbody's speed
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class XRRigRigidbodyWalk : WalkSystemBase
{
    /// <summary>
    /// The xrRig rigidbody
    /// </summary>
    private new Rigidbody rigidbody;
    public Vector3 RigidbodyVelocity()
    {
        return rigidbody.velocity;
    }

    private void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (useLeftThumbstick)
        {
            rigidbody.velocity = CalculateVelocityMultiplicationFactor() * OculusInput.LeftHandVerticalAxis;

            transform.eulerAngles += CalculateMaxAngleToMoveInFrame() * OculusInput.LeftHandHorizontalAxis;
        }
        else
        {
            rigidbody.velocity = CalculateVelocityMultiplicationFactor() * OculusInput.RightHandVerticalAxis;

            transform.eulerAngles += CalculateMaxAngleToMoveInFrame() * OculusInput.RightHandHorizontalAxis;
        }
    }

    /// <summary>
    /// The vector direction pointed according to y euler angle of the camera transform
    /// </summary>
    /// <returns></returns>
    private Vector3 DirectionVector()
    {
        return new Vector3(Mathf.Sin(cameraTransform.eulerAngles.y * Mathf.Deg2Rad), 0,
            Mathf.Cos(cameraTransform.eulerAngles.y * Mathf.Deg2Rad));
    }

    private void OnDisable()
    {
        rigidbody.velocity = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// Calculate the factor to multiplicate with rigidbody's velocity
    /// </summary>
    /// <returns> The factor calculated </returns>
    private Vector3 CalculateVelocityMultiplicationFactor()
    {
        return DirectionVector() * Time.deltaTime * translationSpeed;
    }

    /// <summary>
    /// Calculate the max angle that a single frame can rotate
    /// </summary>
    /// <returns> The angle calculated </returns>
    private Vector3 CalculateMaxAngleToMoveInFrame()
    {
        return Vector3.up * Time.deltaTime * rotationSpeed;
    }
}
