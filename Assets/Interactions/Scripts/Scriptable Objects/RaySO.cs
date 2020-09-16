using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

[CreateAssetMenu]
public class RaySO : ScriptableObject
{
    public XRInteractorLineVisual line;
    public XRRayInteractor ray;

    public RaySO clone()
    {
        return (RaySO) MemberwiseClone();
    }
}
