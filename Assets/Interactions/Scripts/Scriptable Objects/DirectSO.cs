using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

[CreateAssetMenu]
public class DirectSO : ScriptableObject
{
    public XRDirectInteractor direct;

    public DirectSO clone()
    {
        return (DirectSO) MemberwiseClone();
    }
}
