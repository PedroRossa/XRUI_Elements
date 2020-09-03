using UnityEngine;

/// <summary>
/// Class used only for buttons don't exceed their position
/// </summary>
public class FixButtonPosition : MonoBehaviour
{
    /// <summary>
    /// Maximum button position
    /// </summary>
    public float maxValue;
    /// <summary>
    /// Minimum button position
    /// </summary>
    private float minValue;

    /// <summary>
    /// Z axis, because button just translate in "Z"
    /// </summary>
    private float z_Axis;
    /// <summary>
    /// Is the button at North at the player?
    /// </summary>
    private bool isButtonAtNorth;

    private void Start()
    {
        //Its initial position
        minValue = gameObject.transform.position.z;

        isButtonAtNorth = maxValue < 0;
        maxValue = gameObject.transform.position.z + maxValue;
    }
    private void FixedUpdate()
    {
        if(isButtonAtNorth)
            z_Axis = Mathf.Clamp(gameObject.transform.position.z, maxValue, minValue);
        else
            z_Axis = Mathf.Clamp(gameObject.transform.position.z, minValue, maxValue);
        print(z_Axis);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, z_Axis);
    }
}
