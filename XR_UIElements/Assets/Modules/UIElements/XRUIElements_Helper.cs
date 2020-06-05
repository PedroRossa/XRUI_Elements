using System;
using UnityEngine.Events;

public class XRUIElements_Helper
{
    [Serializable]
    public class UnityBooleanEvent : UnityEvent<bool> { }

    [Serializable]
    public class UnityFloatEvent : UnityEvent<float> { }
}
