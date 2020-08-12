using UnityEngine;

public class DisableWalkOnCollision : MonoBehaviour
{
    public Walk walk;
    public GameObject ghostHand;

    private void OnCollisionEnter(Collision collision)
    {
        walk.enabled = false;
        ghostHand.active = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        walk.enabled = true;
        ghostHand.active = false;
    }
}
