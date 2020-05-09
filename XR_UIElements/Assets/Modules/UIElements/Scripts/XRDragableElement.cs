using NaughtyAttributes;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRDragableElement : MonoBehaviour
{
    public Vector3 dragColliderSize = Vector3.one;
    public Transform parentToDrag;

    [ReadOnly]
    public bool isAttached = false;

    private Vector3 parentOffset;

    private void Awake()
    {
        parentOffset = parentToDrag.position - transform.position;
    }

    //Runs only in editor
    private void OnValidate()
    {
        BoxCollider bxCollider = gameObject.GetComponent<BoxCollider>();
        bxCollider.size = dragColliderSize;
    }

    private void OnDrawGizmos()
    {
        //DragAreaGizmos();
    }

    //TODO: ver pra alinhar com a box do collider
    private void DragAreaGizmos()
    {
        Vector3 scaledSize = new Vector3
        (
            dragColliderSize.x * transform.localScale.x,
            dragColliderSize.y * transform.localScale.y,
            dragColliderSize.z * transform.localScale.z
         );

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, scaledSize);
    }

    private void DragByController(Collider collider)
    {
        XRController xrController = collider.GetComponentInChildren<XRController>();
        if (xrController == null)
        {
            xrController = collider.GetComponentInParent<XRController>();
            if (xrController == null)
                return;
        }

        if (xrController.inputDevice != null)
        {
            bool isPressed;
            if (xrController.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out isPressed) && isPressed)
            {
                parentToDrag.position = collider.transform.position + parentOffset;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            isAttached = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isAttached)
        {
            DragByController(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            isAttached = false;
        }
    }
}