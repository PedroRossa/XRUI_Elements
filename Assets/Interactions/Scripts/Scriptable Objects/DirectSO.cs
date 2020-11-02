using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

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
