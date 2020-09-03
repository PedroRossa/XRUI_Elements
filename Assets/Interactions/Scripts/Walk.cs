using UnityEngine;
/// <summary>
/// Class used for locomotion with the left joystick
/// </summary>
public class Walk : MonoBehaviour
{
    /// <summary>
    /// Translation speed movement
    /// </summary>
    public short translationSpeed = 250;
    /// <summary>
    /// Rotation speed movement
    /// </summary>
    public short rotationSpeed = 50;
    /// <summary>
    /// The camera transform from direction reference
    /// </summary>
    public Transform cameraTransform;

    /// <summary>
    /// the proper rigidbody
    /// </summary>
    private Rigidbody rigidbody;

    /// <summary>
    /// Get the rigidbody component
    /// </summary>
    private void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }
    /// <summary>
    /// Calculate rigidbody's velocity and rotate transform direction
    /// </summary>
    private void FixedUpdate()
    {
        CalculateVelocity();
        RotateDirection();
    }

    /// <summary>
    /// Function used to calculate rigidbody's velocity
    /// </summary>
    private void CalculateVelocity()
    {
        rigidbody.velocity = DirectionVector() * Time.deltaTime * translationSpeed *
            OculusInput.LeftHandVerticalAxis;
    }

    /// <summary>
    /// Function used to rotate transform direction with the left joystick horizontal axis
    /// </summary>
    private void RotateDirection()
    {
        transform.eulerAngles += Vector3.up * Time.deltaTime * rotationSpeed *
            OculusInput.LeftHandHorizontalAxis;
    }

    /// <summary>
    /// Function to return a directional vector acording to cameraTransform which every vector member has a value to 0 from 1
    /// </summary>
    /// <returns></returns>
    private Vector3 DirectionVector()
    {
        return new Vector3(Mathf.Sin(cameraTransform.eulerAngles.y * Mathf.Deg2Rad), 0, 
            Mathf.Cos(cameraTransform.eulerAngles.y * Mathf.Deg2Rad));
    }

    /// <summary>
    /// Set the rigidbody's velocity to 0 when the script is disabled
    /// </summary>
    private void OnDisable()
    {
        rigidbody.velocity = Vector3.zero;
    }
}
