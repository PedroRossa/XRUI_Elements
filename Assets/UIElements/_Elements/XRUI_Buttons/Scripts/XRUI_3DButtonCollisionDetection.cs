using UnityEngine;

public class XRUI_3DButtonCollisionDetection : MonoBehaviour
{
    public bool isColliding;
    public Collider buttonCollider;

    public XRUI_ButtonBase xrUIButton;

    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(buttonCollider))
        {
            isColliding = true;
            xrUIButton.onClickDown?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.Equals(buttonCollider) && isColliding)
        {
            isColliding = false;
            xrUIButton.onClickUp?.Invoke();
        }
    }
}
