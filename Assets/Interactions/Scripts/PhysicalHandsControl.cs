using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.XR.Interaction.Toolkit.XRBaseInteractable;
using UnityEngine;

/// <summary>
/// A script that possibilty physical interactions with the xr rig hands
/// </summary>
public class PhysicalHandsControl : MonoBehaviour
{
    /// <summary>
    /// The transforms of the hands
    /// </summary>
    public Transform[] handsTransforms;

    /// <summary>
    /// The xr base interactors of the hands
    /// </summary>
    private XRBaseInteractor[] xrBaseInteractors;
    /// <summary>
    /// The hands' rigidbodies
    /// </summary>
    private Rigidbody[] handsRigidbodies;

    /// <summary>
    /// Main setup
    /// </summary>
    void Start()
    {
        xrBaseInteractors = new XRBaseInteractor[handsTransforms.Length];
        handsRigidbodies = new Rigidbody[handsTransforms.Length];

        for (int i = 0; i < handsTransforms.Length; i++)
        {
            xrBaseInteractors[i] = handsTransforms[i].GetComponentInParent<XRBaseInteractor>();
            handsRigidbodies[i] = handsTransforms[i].GetComponent<Rigidbody>();
        }
    }

    /// <summary>
    /// If a hand is selecting something, control its position and turns non-kinematic, else turn its kinematic
    /// </summary>
    private void Update()
    {
        for (int i = 0; i < handsTransforms.Length; i++)
        {
            if (!xrBaseInteractors[i].isSelectActive)
            {
                handsTransforms[i].localPosition = new Vector3(0, 0, -0.05f);
                handsRigidbodies[i].isKinematic = false;
            }
            else
                handsRigidbodies[i].isKinematic = true;
        }
    }
}
