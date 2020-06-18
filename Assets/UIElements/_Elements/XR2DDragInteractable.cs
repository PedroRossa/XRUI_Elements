using NaughtyAttributes;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XR2DDragInteractable : MonoBehaviour
{
    [ReadOnly]
    public bool isHover = false;
    [ReadOnly]
    public bool isSelected = false;
    [HideInInspector]
    public float normalizedValue;
    [HideInInspector]
    public XRBaseInteractable interactable;

    public Vector3 localInitPos = Vector3.zero;
    public Vector3 localEndPos = Vector3.right;

    private Rigidbody interactableRigidbody;
    private Transform originalParent;

    private void Awake()
    {
        interactable = gameObject.GetComponent<XRBaseInteractable>();
        interactableRigidbody = interactable.GetComponent<Rigidbody>();

        originalParent = transform.parent;
    }

    private void FixedUpdate()
    {
        ConstraintLocally();
    }

    private void Update()
    {
        if (isHover || isSelected)
        {
            UpdateByMovement();
        }
    }

    public void Setup(bool canPush)
    {
        interactableRigidbody.isKinematic = !canPush;

        if (canPush)
        {
            interactable.onFirstHoverEnter.AddListener((XRBaseInteractor) => { isHover = true; });
            interactable.onLastHoverExit.AddListener((XRBaseInteractor) => { isHover = false; });
        }

        interactable.onSelectEnter.AddListener((XRBaseInteractor) => { isSelected = true; });
        interactable.onSelectExit.AddListener((XRBaseInteractor) =>
        {
            isSelected = false;
            interactableRigidbody.velocity = Vector3.zero;
        });
    }

    private void UpdateByMovement()
    {
        if (transform.parent == null)
            transform.SetParent(originalParent);

        //Get x position that represents the current local slider object position
        float posX = transform.localPosition.x;
        posX = posX > 1 ? 1 : posX;
        posX = posX < 0 ? 0 : posX;

        transform.localPosition = new Vector3(posX, 0, 0);

        normalizedValue = (posX - localInitPos.x) / (localEndPos.x - localInitPos.x);
    }

    private void ConstraintLocally()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(interactableRigidbody.velocity);
        localVelocity.y = 0;
        localVelocity.z = 0;

        interactableRigidbody.velocity = transform.TransformDirection(localVelocity);
        transform.localPosition = new Vector3(transform.localPosition.x, 0, 0);
    }
}
