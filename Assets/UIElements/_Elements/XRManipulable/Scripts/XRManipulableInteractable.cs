using NaughtyAttributes;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(MeshRenderer))]
public class XRManipulableInteractable : XRGrabInteractable
{
    [Header("ManipulableProperties")]
    public bool isScaleElement;
    public bool isRotationElement;
}
