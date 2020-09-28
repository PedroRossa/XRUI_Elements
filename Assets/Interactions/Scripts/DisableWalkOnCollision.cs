using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Class used for disable a Walk script when a hand collide with a collider
/// </summary>
public class DisableWalkOnCollision : MonoBehaviour
{
    /// <summary>
    /// The XR rig walk instance
    /// </summary>
    public Walk walk;
    /// <summary>
    /// A ghost hand game object
    /// </summary>
    public GameObject ghostHand;

    private XRBaseInteractor grabInteractor;

    private void Start()
    {
        grabInteractor = gameObject.GetComponentInParent<XRBaseInteractor>();
    }
    /// <summary>
    /// Active the ghost hand and deactive the walk
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(!grabInteractor.isSelectActive) { 
            walk.enabled = false;
            ghostHand.SetActive(true);
        }
    }
}
