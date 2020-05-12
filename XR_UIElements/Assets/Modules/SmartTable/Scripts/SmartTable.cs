using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SmartTable : MonoBehaviour
{
    public Transform tableTransform;
    public Transform baseTransform;

    public XRRig xrRig;

    public bool isRightHanded = true;

    private XRController leftController;
    private XRController rightController;

    private bool isCalibrationStarted = false;
    private bool isDownLeftCornerSet = false;
    private bool isUpRightCornerSet = false;

    private Vector3 downLeftCorner;
    private Vector3 upRightCorner;

    private bool rTriggerDown = false;

    private UnityAction onRightTriggerDown;
    private UnityAction onRightTriggerUp;

    void Awake()
    {
        GetControllers();

        onRightTriggerDown += RightTriggerDown;
        onRightTriggerUp += RightTriggerUp;
    }

    void Update()
    {
        if (xrRig == null)
            return;

        if(Input.GetKeyUp(KeyCode.Space))
        {
            isCalibrationStarted = true;
        }

        DetectRightTrigger();
    }

    private void GetControllers()
    {
        if(xrRig == null)
        {
            Debug.LogError("You need to set an xrRig referece!");
            return;
        }

        XRController[] controllers = xrRig.GetComponentsInChildren<XRController>();

        foreach (XRController controller in controllers)
        {
            if (controller.controllerNode == XRNode.LeftHand)
                leftController = controller;
            else if (controller.controllerNode == XRNode.RightHand)
                rightController = controller;
        }
    }

    void RightTriggerDown()
    {
    }

    void RightTriggerUp()
    {
        if (isCalibrationStarted)
        {
            if (!isDownLeftCornerSet)
            {
                SetDownLeftCorner();
                isDownLeftCornerSet = true;
            }
            else if (isDownLeftCornerSet && !isUpRightCornerSet)
            {
                SetUpRightCorner();

                isUpRightCornerSet = true;
                isCalibrationStarted = false;
            }
            else if (isDownLeftCornerSet && isUpRightCornerSet)
            {
                isCalibrationStarted = false;
                isDownLeftCornerSet = false;
                isUpRightCornerSet = false;
            }
        }
    }

    private void DetectRightTrigger()
    {
        bool rightTriggerIsPressed;
        if (rightController.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out rightTriggerIsPressed))
        {
            if (!rTriggerDown && rightTriggerIsPressed)
            {
                rTriggerDown = true;
                onRightTriggerDown.Invoke();
            }
            else if (rTriggerDown && !rightTriggerIsPressed)
            {
                rTriggerDown = false;
                onRightTriggerUp.Invoke();
            }
        }
    }

    private void SetDownLeftCorner()
    {
        downLeftCorner = leftController.modelTransform.position;
        Vector3 newTableHeight = new Vector3(1, downLeftCorner.y, 1);

        tableTransform.position = downLeftCorner;
        baseTransform.localScale = newTableHeight;
    }

    private void SetUpRightCorner()
    {
        upRightCorner = leftController.modelTransform.position;

        Vector3 distanceBetweenControllers = upRightCorner - downLeftCorner;
        Debug.Log("Distance:" + distanceBetweenControllers);

        tableTransform.localScale = new Vector3(distanceBetweenControllers.x, 1, distanceBetweenControllers.z);
    }

}
