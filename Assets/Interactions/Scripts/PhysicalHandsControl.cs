using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.XR.Interaction.Toolkit.XRBaseInteractable;
using UnityEngine;

public class PhysicalHandsControl : MonoBehaviour
{
    public Transform[] handsTransforms;

    private Rigidbody rigidbody;
    private XRBaseInteractor[] xrBaseInteractors;
    private Rigidbody[] handsRigidbodies;

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
