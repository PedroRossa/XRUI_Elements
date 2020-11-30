using UnityEngine;

/// <summary>
/// Manager of XRUI 3D buttons collisions
/// </summary>
public class XRUI_3DButtonCollisionDetection : MonoBehaviour
{
    /// <summary>
    /// Is the button colliding with the collisor?
    /// </summary>
    public bool isColliding;
    /// <summary>
    /// The collider of the button
    /// </summary>
    public Collider buttonCollider;

    /// <summary>
    /// The XRUI_3DButtonBase reference component
    /// </summary>
    public XRUI_3DButtonBase xrUIButton;

    /// <summary>
    /// Callback called when the collider enter in trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(buttonCollider) && xrUIButton.canActiveButton)
        {
            isColliding = true;
            xrUIButton.onClickDown?.Invoke();
        }
    }

    /// <summary>
    /// Callback called when the collider exit trigger state
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.Equals(buttonCollider) && isColliding && xrUIButton.canActiveButton)
        {
            isColliding = false;
            xrUIButton.onClickUp?.Invoke();

            try
            {
                StartCoroutine(xrUIButton.ResetCanActiveButton());
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
