using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

/// <summary>
/// Class to reference a RaySO and a DirectSO
/// </summary>
public class RayAndDirect : MonoBehaviour
{
    public RaySO raySo;
    public DirectSO directSo;

    /// <summary>
    /// Return a copy of the Ray component
    /// </summary>
    /// <returns></returns>
    public RaySO cloneRay()
    {
        return raySo.clone();
    }
    /// <summary>
    /// Return a copy of the Direct component
    /// </summary>
    /// <returns></returns>
    public DirectSO cloneDirect()
    {
        return directSo.clone();
    }
}
