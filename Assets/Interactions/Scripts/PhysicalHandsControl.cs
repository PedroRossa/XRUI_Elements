using UnityEngine;

/// <summary>
/// A script that possibilty physical interactions with the xr rig hands
/// </summary>
public class PhysicalHandsControl : MonoBehaviour
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
        /*this.transform.localPosition = new Vector3(
                    Mathf.Clamp(transform.localPosition.x,
                        -0.1f,
                        0.1f)
                        ,
                    Mathf.Clamp(transform.localPosition.y,
                        -0.1f,
                        0.1f)
                        ,
                    Mathf.Clamp(transform.localPosition.z,
                        -0.15f,
                        0.15f)
                        );*/
    }

    private void OnCollisionExit(Collision collision)
    {
        isColliding = false;
    }
}
