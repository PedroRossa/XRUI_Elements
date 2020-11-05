using NaughtyAttributes;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Interactable that can be manipulated by Oculus controlls
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class XRManipulableInteractable : XRGrabInteractable
{
    [Header("ManipulableProperties")]
    public bool isScaleElement;
    public bool isRotationElement;

    [ShowIf("isRotationElement")]
    public Vector3 rotationAxis;
}
