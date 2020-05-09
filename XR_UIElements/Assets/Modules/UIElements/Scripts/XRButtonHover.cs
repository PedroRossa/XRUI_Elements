using UnityEngine;

public class XRButtonHover : MonoBehaviour
{
    [HideInInspector]
    public XRButton xrButton;

    public void SetHoverDistanceCollider()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        //Multiply the hoverDistance of XRButton by 10, bacause the element 'backpanel' has an scale of 0.1 .
        float colliderZSize = xrButton.hoverDistance * 10;

        boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, colliderZSize);
        boxCollider.center = new Vector3(0, 0, boxCollider.size.z / 2);
    }

    private void OnDrawGizmos()
    {
        if (xrButton == null)
            return;

        HoverDistanceGizmo();
    }

    private void HoverDistanceGizmo()
    {
        Vector3 buttonPos = xrButton.transform.position;
        Vector3 hoverArea = new Vector3(buttonPos.x, buttonPos.y, buttonPos.z - xrButton.hoverDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(buttonPos, hoverArea);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            if (xrButton != null)
            {
                xrButton.onHoverEnter?.Invoke();
                xrButton.isHover = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("interactable"))
        {
            if (xrButton != null)
            {
                xrButton.onHoverExit?.Invoke();
                xrButton.isHover = false;
            }
        }

    }
}
