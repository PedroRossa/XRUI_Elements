using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.XR.Interaction.Toolkit.XRBaseInteractable;

public class Walk : MonoBehaviour
{
    public short translationSpeed = 250;
    public short rotationSpeed = 50;
    public Transform cameraTransform;
    public Transform[] handsTransforms;

    private Rigidbody rigidbody;
    private XRBaseInteractor[] xrBaseInteractors;
    private Rigidbody[] handsRigidbodies;

    private void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();

        xrBaseInteractors = new XRBaseInteractor[handsTransforms.Length];
        handsRigidbodies = new Rigidbody[handsTransforms.Length];

        for(int i = 0; i < handsTransforms.Length; i++)
        {
            xrBaseInteractors[i] = handsTransforms[i].GetComponentInParent<XRBaseInteractor>();
            handsRigidbodies[i] = handsTransforms[i].GetComponent<Rigidbody>();
        }

    }
    void FixedUpdate()
    {
         rigidbody.velocity = DirectionVector() * Time.deltaTime * translationSpeed *
           OculusInput.LeftHandVerticalAxis;

        transform.eulerAngles += Vector3.up * Time.deltaTime * rotationSpeed *
            OculusInput.LeftHandHorizontalAxis;
    }

    private void Update()
    {
        for (int i = 0; i < handsTransforms.Length; i++)
        {
            if (!xrBaseInteractors[i].isSelectActive) {
                handsTransforms[i].localPosition = new Vector3(0, 0, -0.05f);
                handsRigidbodies[i].isKinematic = false;
            }
            else
                handsRigidbodies[i].isKinematic = true;
        }
    }

    Vector3 DirectionVector()
    {
        return new Vector3(Mathf.Sin(cameraTransform.eulerAngles.y * Mathf.Deg2Rad), 0, 
            Mathf.Cos(cameraTransform.eulerAngles.y * Mathf.Deg2Rad));
    }

    private void OnDisable()
    {
        rigidbody.velocity = Vector3.zero;
    }
}
