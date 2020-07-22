using UnityEngine;

public static class OculusInput
{
    public static float LeftHandVerticalAxis
    {
        get {return Input.GetAxis("Oculus_CrossPlatform_PrimaryThumbstickVertical"); }
    }

    public static float LeftHandHorizontalAxis
    {
        get { return Input.GetAxis("Oculus_CrossPlatform_PrimaryThumbstickHorizontal"); }
    }

    public static float RightHandVerticalAxis
    {
        get { return Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical"); }
    }

    public static float RightHandHorizontalAxis
    {
        get { return Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickHorizontal"); }
    }

    public static KeyCode LeftHandTrigger
    {
        get { return KeyCode.JoystickButton4; }
    }

    public static KeyCode RightHandTrigger
    {
        get { return KeyCode.JoystickButton5; }
    }

    public static KeyCode LeftHandIndexTrigger
    {
        get { return KeyCode.JoystickButton14; }
    }

    public static KeyCode RightHandIndexTrigger
    {
        get { return KeyCode.JoystickButton15; }
    }

    public static KeyCode LeftHandThumbstick
    {
        get { return KeyCode.JoystickButton8; }
    }

    public static KeyCode RightHandThumbstick
    {
        get { return KeyCode.JoystickButton9; }
    }

    public static KeyCode OptionsButton
    {
        get { return KeyCode.JoystickButton6; }
    }

    public static KeyCode ButtonA
    {
        get { return KeyCode.JoystickButton0; }
    }

    public static KeyCode ButtonB
    {
        get { return KeyCode.JoystickButton1; }
    }

    public static KeyCode ButtonX
    {
        get { return KeyCode.JoystickButton2; }
    }

    public static KeyCode ButtonY
    {
        get { return KeyCode.JoystickButton3; }
    }
}
