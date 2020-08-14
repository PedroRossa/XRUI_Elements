using UnityEngine;

public class DisableWalkOnCollision : MonoBehaviour
{
    public Walk walk;
    public PhysicalHandsControl physicalHandsControl;
    public GameObject ghostHand;

    private void OnCollisionEnter(Collision collision)
    {
        walk.enabled = physicalHandsControl.enabled = false;
        ghostHand.active = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        walk.enabled = physicalHandsControl.enabled = true;
        ghostHand.active = false;
    }
}
