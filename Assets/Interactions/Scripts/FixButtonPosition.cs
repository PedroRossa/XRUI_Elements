using UnityEngine;

/// <summary>
/// Class used only for buttons don't exceed their position
/// </summary>
public class FixButtonPosition : MonoBehaviour
{
    /// <summary>
    /// Value to sum with button position
    /// </summary>
    public float sumMaxValue;
    /// <summary>
    /// Value to subtract with button position
    /// </summary>
    public float subtractMinValue;

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
        subtractMinValue = gameObject.transform.position.z - subtractMinValue;

        isButtonAtNorth = sumMaxValue < 0;
        sumMaxValue = gameObject.transform.position.z + sumMaxValue;
    }
    private void FixedUpdate()
    {
        if (isButtonAtNorth)
            z_Axis = Mathf.Clamp(gameObject.transform.position.z, sumMaxValue, subtractMinValue);
        else
            z_Axis = Mathf.Clamp(gameObject.transform.position.z, subtractMinValue, sumMaxValue);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, z_Axis);
    }
}
