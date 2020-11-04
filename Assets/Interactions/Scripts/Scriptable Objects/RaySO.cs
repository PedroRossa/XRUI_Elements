using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

/// <summary>
/// Scriptable Object to instance XRRayInteractors containing XRInteractorLineVisuals
/// </summary>
[CreateAssetMenu]
public class RaySO : ScriptableObject
{
    public XRInteractorLineVisual line;
    public XRRayInteractor ray;

    /// <summary>
    /// Return a copy of the component
    /// </summary>
    /// <returns></returns>
    public RaySO clone()
    {
        return (RaySO) MemberwiseClone();
    }
}
