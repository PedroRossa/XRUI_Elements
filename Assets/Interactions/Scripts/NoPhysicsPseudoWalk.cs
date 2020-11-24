using UnityEngine;

/// <summary>
/// Class that permits a locomotion system with one of the joysticks thumbstick through changing xr rig transform.position
/// </summary>
public class NoPhysicsPseudoWalk : WalkSystemBase
{
   

    void Update()
    {
        if (useLeftThumbstick)
        {
            transform.position += CalculateVelocityMultiplicationFactor() * OculusInput.LeftHandVerticalAxis;

            transform.eulerAngles += CalculateMaxAngleToMoveInFrame() * OculusInput.LeftHandHorizontalAxis;
        }
        else
        {
            transform.position += CalculateVelocityMultiplicationFactor() * OculusInput.RightHandVerticalAxis;

            transform.eulerAngles += CalculateMaxAngleToMoveInFrame() * OculusInput.RightHandHorizontalAxis;
        }
    }

    /// <summary>
    /// The vector direction pointed according to y euler angle of the camera transform
    /// </summary>
    /// <returns></returns>
    Vector3 DirectionVector()
    {
        return new Vector3(Mathf.Sin(cameraTransform.eulerAngles.y * Mathf.Deg2Rad), 0,
            Mathf.Cos(cameraTransform.eulerAngles.y * Mathf.Deg2Rad));
    }

    /// <summary>
    /// Calculate the factor to multiplicate with rigidbody's velocity
    /// </summary>
    /// <returns></returns>
    private Vector3 CalculateVelocityMultiplicationFactor()
    {
        return DirectionVector() * Time.deltaTime * translationSpeed;
    }

    /// <summary>
    /// Calculate the max angle that a single frame can rotate
    /// </summary>
    /// <returns></returns>
    private Vector3 CalculateMaxAngleToMoveInFrame()
    {
        return Vector3.up * Time.deltaTime * rotationSpeed;
    }
}
