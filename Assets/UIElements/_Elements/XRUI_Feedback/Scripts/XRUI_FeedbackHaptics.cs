using NaughtyAttributes;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
///Haptics triggered on Oculus controll derived from XRUI_FeedbackBaseType
/// </summary>
public class XRUI_FeedbackHaptics : XRUI_FeedbackBaseType
{
    [Header("Near Haptics")]
    [ShowIf("runOnNear")]
    [Range(0.0f, 1.0f)]
    public float nearEnterHapticAmplitude = 0.1f;
    [ShowIf("runOnNear")]
    [Range(0.0f, 1.0f)]
    public float nearEnterHapticFrequence = 0.1f;
    [ShowIf("runOnNear")]
    [Range(0.0f, 1.0f)]
    public float nearExitHapticAmplitude = 0.1f;
    [ShowIf("runOnNear")]
    [Range(0.0f, 1.0f)]
    public float nearExitHapticFrequence = 0.1f;

    [Header("Touch Haptics")]
    [ShowIf("runOnTouch")]
    [Range(0.0f, 1.0f)]
    public float touchEnterHapticAmplitude = 0.1f;
    [ShowIf("runOnTouch")]
    [Range(0.0f, 1.0f)]
    public float touchEnterHapticFrequence = 0.1f;
    [ShowIf("runOnTouch")]
    [Range(0.0f, 1.0f)]
    public float touchExitHapticAmplitude = 0.1f;
    [ShowIf("runOnTouch")]
    [Range(0.0f, 1.0f)]
    public float touchExitHapticFrequence = 0.1f;

    [Header("Select Haptics")]
    [ShowIf("runOnSelect")]
    [Range(0.0f, 1.0f)]
    public float selectEnterHapticAmplitude = 0.1f;
    [ShowIf("runOnSelect")]
    [Range(0.0f, 1.0f)]
    public float selectEnterHapticFrequence = 0.1f;
    [ShowIf("runOnSelect")]
    [Range(0.0f, 1.0f)]
    public float selectExitHapticAmplitude = 0.1f;
    [ShowIf("runOnSelect")]
    [Range(0.0f, 1.0f)]
    public float selectExitHapticFrequence = 0.1f;


    protected override void OnNearEnterFeedbackFunction(XRController controller)
    {
        _ = controller.SendHapticImpulse(nearEnterHapticAmplitude, nearEnterHapticFrequence);
    }

    protected override void OnNearExitFeedbackFunction(XRController controller)
    {
        _ = controller.SendHapticImpulse(nearExitHapticAmplitude, nearExitHapticFrequence);
    }

    protected override void OnTouchEnterFeedbackFunction(XRController controller)
    {
        _ = controller.SendHapticImpulse(touchEnterHapticAmplitude, touchEnterHapticFrequence);
    }

    protected override void OnTouchExitFeedbackFunction(XRController controller)
    {
        _ = controller.SendHapticImpulse(touchExitHapticAmplitude, touchExitHapticFrequence);
    }

    protected override void OnSelectEnterFeedbackFunction(XRController controller)
    {
        _ = controller.SendHapticImpulse(selectEnterHapticAmplitude, selectEnterHapticFrequence);
    }

    protected override void OnSelectExitFeedbackFunction(XRController controller)
    {
        _ = controller.SendHapticImpulse(selectExitHapticAmplitude, selectExitHapticFrequence);
    }

}
