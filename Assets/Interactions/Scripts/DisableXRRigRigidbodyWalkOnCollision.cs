using NaughtyAttributes;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Class used for disable a XRRigRigidbodyWalk script when a hand collide with a collider
/// </summary>
public class DisableXRRigRigidbodyWalkOnCollision : MonoBehaviour
{
    /// <summary>
    /// The XR rig walk instance
    /// </summary>
    [Required]
    public XRRigRigidbodyWalk walk;
    /// <summary>
    /// A ghost hand game object
    /// </summary>
    [Required]
    public GameObject ghostHand;

    private XRBaseInteractor grabInteractor;

    private void Start()
    {
        grabInteractor = gameObject.GetComponentInParent<XRBaseInteractor>();
    }
    /// <summary>
    /// Active the ghost hand and deactive XRRigRigidbodyWalk script
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        walk.enabled = false;
        ghostHand.SetActive(true);
    }
    /// <summary>
    /// Deactive the ghost hand and active XRRigRigidbodyWalk script
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        walk.enabled = true;
        ghostHand.SetActive(false);
    }
}
