using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace InputManager
{
    /// <summary>
    /// Base class for input handlers
    /// </summary>
    public class InputHandler : ScriptableObject
    {
        public virtual void HandleState(XRController controller)
        {
            // Empty
        }
    }
}