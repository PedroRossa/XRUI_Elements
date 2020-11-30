using UnityEngine;

/// <summary>
/// Base class for Walk Systems implementations
/// </summary>
public class WalkSystemBase : MonoBehaviour
{
    /// <summary>
    /// Speed for translation movement
    /// </summary>
    public short translationSpeed = 250;
    /// <summary>
    /// Speed for rotation movement
    /// </summary>
    public short rotationSpeed = 50;
    /// <summary>
    /// The transform of the xrRig camera
    /// </summary>
    public Transform cameraTransform;
    /// <summary>
    /// Do you wanna use left thumbstick? If it is false, use right thumbstick
    /// </summary>
    public bool useLeftThumbstick;
}
