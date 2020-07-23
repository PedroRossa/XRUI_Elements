using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRUI_2DDragInteractable : MonoBehaviour
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

    private Vector3 worldInitPos = Vector3.zero;
    private Vector3 worldEndPos = Vector3.zero;

    private Rigidbody interactableRigidbody;
    private Transform originalParent;

    private void OnValidate()
    {
        originalParent = transform.parent;
        worldInitPos = transform.parent.TransformVector(localInitPos) + transform.parent.position;
        worldEndPos = transform.parent.TransformVector(localEndPos) + transform.parent.position;
    }

    private void Start()
    {
        originalParent = transform.parent;
        interactable = gameObject.GetComponent<XRBaseInteractable>();
        interactableRigidbody = interactable.GetComponent<Rigidbody>();

        interactableRigidbody.isKinematic = true;

        interactable.onHoverEnter.AddListener((XRBaseInteractor) => { isHover = true; });
        interactable.onHoverExit.AddListener((XRBaseInteractor) => { isHover = false; });

        interactable.onSelectEnter.AddListener((XRBaseInteractor) =>
        {
            isSelected = true;
            XRUI_Helper.ConstraintVelocityLocally(transform, interactableRigidbody, false, true, true);
            StartCoroutine(UpdateInfos());
        });
        interactable.onSelectExit.AddListener((XRBaseInteractor) =>
        {
            isSelected = false;
            interactableRigidbody.velocity = Vector3.zero;
            StopCoroutine(UpdateInfos());
        });
    }
    private IEnumerator UpdateInfos()
    {
        UpdateByMovement();
        yield return new WaitForFixedUpdate();
        transform.localPosition = new Vector3(Mathf.Clamp01(transform.localPosition.x), 0, 0);
        StartCoroutine(UpdateInfos());
    }
    private void UpdateByMovement()
    {
        if (transform.parent == null)
            transform.SetParent(originalParent);
        //Get x position that represents the current local slider object position
        float posX = Mathf.Clamp01(transform.localPosition.x);
        normalizedValue = (posX - localInitPos.x) / (localEndPos.x - localInitPos.x);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(worldInitPos, transform.localScale.x);
        Gizmos.DrawLine(worldInitPos, worldEndPos);
        Gizmos.DrawWireSphere(worldEndPos, transform.localScale.x);
    }
}
