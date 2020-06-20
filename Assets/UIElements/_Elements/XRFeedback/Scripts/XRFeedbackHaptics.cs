using NaughtyAttributes;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRFeedbackHaptics : XRFeedbackBaseType
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


    protected override void ConfigureNearFeedback()
    {
        xrFeedback.onNearEnter.AddListener((XRController controller) => 
        {
            controller.SendHapticImpulse(nearEnterHapticAmplitude, nearEnterHapticFrequence); 
        });
        xrFeedback.onNearExit.AddListener((XRController controller) => 
        {
            controller.SendHapticImpulse(nearExitHapticAmplitude, nearExitHapticFrequence);
        });
    }

    protected override void ConfigureSelectFeedback()
    {
        xrFeedback.onTouchEnter.AddListener((XRController controller) =>
        {
            controller.SendHapticImpulse(touchEnterHapticAmplitude, touchEnterHapticFrequence);
        });
        xrFeedback.onTouchExit.AddListener((XRController controller) =>
        {
            controller.SendHapticImpulse(touchExitHapticAmplitude, touchExitHapticFrequence);
        });
    }

    protected override void ConfigureTouchFeedback()
    {
        xrFeedback.onSelectEnter.AddListener((XRController controller) =>
        {
            controller.SendHapticImpulse(selectEnterHapticAmplitude, selectEnterHapticFrequence);
        });
        xrFeedback.onSelectExit.AddListener((XRController controller) =>
        {
            controller.SendHapticImpulse(selectExitHapticAmplitude, selectExitHapticFrequence);
        });
    }
}
