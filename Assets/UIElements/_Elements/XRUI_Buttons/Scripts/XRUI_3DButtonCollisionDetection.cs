using UnityEngine;

/// <summary>
/// Class to manage collisions with XRUI_3DButtonBase instances
/// </summary>
[RequireComponent(typeof(XRUI_3DButtonBase))]
public class XRUI_3DButtonCollisionDetection : MonoBehaviour
{
    public bool isColliding;
    public Collider buttonCollider;

    public XRUI_3DButtonBase xrUIButton;

    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(buttonCollider) && xrUIButton.canActiveButton)
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

            try
            {
                StartCoroutine((xrUIButton as XRUI_3DButtonBase).resetCanActiveButton());
            }
            catch (System.Exception e) {
                Debug.LogError(e);
            }
        }
    }
}
