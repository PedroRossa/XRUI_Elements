using UnityEngine;

/// <summary>
/// Class used only for buttons don't exceed their position
/// </summary>
public class FixButtonPosition : MonoBehaviour
{
    /// <summary>
    /// The initial position locally
    /// </summary>
    private Vector3 initialLocalPosition;

    /// <summary>
    /// Time in seconds to wait till verify position again
    /// </summary>
    private const float waitSeconds = 0.1f;
    /// <summary>
    /// The tolerance range of button position. Has this value because this is the distance in 'Z' between the mesh and the background;
    /// </summary>
    private const float backgroundLocalPosition = 0.0274f;
    private const float toleranceRange = backgroundLocalPosition + 0.050f;

    private void Start()
    {
        initialLocalPosition = gameObject.transform.localPosition;
    }

    private void Update()
    {
        gameObject.transform.localPosition = initialLocalPosition + new Vector3(0, 0,
            Mathf.Clamp(gameObject.transform.localPosition.z,
            -toleranceRange,
            toleranceRange)
            );
    }
}
