using UnityEngine;

/// <summary>
/// Static class to have access at the Oculus Input's axis and buttons
/// </summary>
public static class OculusInput
{
    /// <summary>
    /// Left joystick vertical axis
    /// </summary>
    public static float LeftHandVerticalAxis
    {
        get {return Input.GetAxis("Oculus_CrossPlatform_PrimaryThumbstickVertical"); }
    }

    /// <summary>
    /// Left joystick horizontal axis
    /// </summary>
    public static float LeftHandHorizontalAxis
    {
        get { return Input.GetAxis("Oculus_CrossPlatform_PrimaryThumbstickHorizontal"); }
    }

    /// <summary>
    /// Right joystick vertical axis
    /// </summary>
    public static float RightHandVerticalAxis
    {
        get { return Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical"); }
    }

    /// <summary>
    /// Right joystick horizontal axis
    /// </summary>
    public static float RightHandHorizontalAxis
    {
        get { return Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickHorizontal"); }
    }

    /// <summary>
    /// Left joystick middlefinger trigger
    /// </summary>
    public static KeyCode LeftHandTrigger
    {
        get { return KeyCode.JoystickButton4; }
    }

    /// <summary>
    /// Right joystick middlefinger trigger
    /// </summary>
    public static KeyCode RightHandTrigger
    {
        get { return KeyCode.JoystickButton5; }
    }

    /// <summary>
    /// Left joystick index trigger
    /// </summary>
    public static KeyCode LeftHandIndexTrigger
    {
        get { return KeyCode.JoystickButton14; }
    }

    /// <summary>
    /// Right joystick index trigger
    /// </summary>
    public static KeyCode RightHandIndexTrigger
    {
        get { return KeyCode.JoystickButton15; }
    }

    /// <summary>
    /// Left joystick button behind analog
    /// </summary>
    public static KeyCode LeftHandThumbstick
    {
        get { return KeyCode.JoystickButton8; }
    }

    /// <summary>
    /// Right joystick button behind analog
    /// </summary>
    public static KeyCode RightHandThumbstick
    {
        get { return KeyCode.JoystickButton9; }
    }

    /// <summary>
    /// Left joystick options button
    /// </summary>
    public static KeyCode OptionsButton
    {
        get { return KeyCode.JoystickButton6; }
    }

    /// <summary>
    /// Right joystick "A" button
    /// </summary>
    public static KeyCode ButtonA
    {
        get { return KeyCode.JoystickButton0; }
    }

    /// <summary>
    /// Right joystick "B" button
    /// </summary>
    public static KeyCode ButtonB
    {
        get { return KeyCode.JoystickButton1; }
    }

    /// <summary>
    /// Left joystick "X" button
    /// </summary>
    public static KeyCode ButtonX
    {
        get { return KeyCode.JoystickButton2; }
    }

    /// <summary>
    /// Left joystick "Y" button
    /// </summary>
    public static KeyCode ButtonY
    {
        get { return KeyCode.JoystickButton3; }
    }
}
