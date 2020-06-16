using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRControllerManager : MonoBehaviour
{
    public bool isRightHand;

    public XRController leftController;
    public XRController rightController;
    public Transform leftAttach;
    public Transform rightAttach;

    public InputHelpers.Button rayButton;

    public void EnableRay(XRController controller)
    {
        XRBaseInteractor baseInteractor = controller.GetComponent<XRBaseInteractor>();
        if (baseInteractor != null)
        {
            if (baseInteractor.GetType() == typeof(XRRayInteractor))
                return;
            
            Destroy(baseInteractor);
        }

        XRRayInteractor rayInteractor = controller.gameObject.AddComponent<XRRayInteractor>();
    }
}
