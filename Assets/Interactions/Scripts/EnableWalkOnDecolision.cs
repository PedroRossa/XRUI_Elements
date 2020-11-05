using UnityEngine;

/// <summary>
/// Class used for disable a Walk script when a hand collide with a collider
/// </summary>
public class EnableWalkOnDecolision : MonoBehaviour
{
    /// <summary>
    /// The XR rig walk instance
    /// </summary>
    public XRRigRigidbodyWalk walk;

    private void OnCollisionExit(Collision collision)
    {
        walk.enabled = true;
        gameObject.SetActive(false);
    }
}
