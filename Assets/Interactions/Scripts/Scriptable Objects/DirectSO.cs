using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

/// <summary>
/// Scriptable object to instance XRDirectInteractors
/// </summary>
[CreateAssetMenu]
public class DirectSO : ScriptableObject
{
    public XRDirectInteractor direct;

    /// <summary>
    /// Return a copy of the component
    /// </summary>
    /// <returns></returns>
    public DirectSO clone()
    {
        return (DirectSO) MemberwiseClone();
    }
}
