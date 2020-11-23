using UnityEngine;

/// <summary>
/// A script that possibilty physical interactions with the xr rig hands
/// </summary>
public class PhysicalHandsController : MonoBehaviour
{
    private bool isColliding;

    private void Update()
    {
        if(!isColliding) {
            transform.localPosition = new Vector3(0, 0, -0.05f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isColliding = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isColliding = false;
    }
}
