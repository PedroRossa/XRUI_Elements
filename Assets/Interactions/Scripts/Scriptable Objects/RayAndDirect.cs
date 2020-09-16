using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class RayAndDirect : MonoBehaviour
{
    public RaySO raySo;
    public DirectSO directSo;

    public RaySO cloneRay()
    {
        return raySo.clone();
    }

    public DirectSO cloneDirect()
    {
        return directSo.clone();
    }
}
