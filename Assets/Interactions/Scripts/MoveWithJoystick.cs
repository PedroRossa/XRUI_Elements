using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.XR.Interaction.Toolkit.XRBaseInteractable;

/// <summary>
/// Class that permits a bunch of interactions with a joystick in an object
/// </summary>
[RequireComponent(typeof(XRGrabInteractable))]
public class MoveWithJoystick : MonoBehaviour
{
    private enum AxisToRotate
    {
        x,
        y,
        z
    };
    private AxisToRotate axisToRotate = AxisToRotate.x;

    /// <summary>
    /// Speed of vertical translation
    /// </summary>
    public float speedVertical = 10f;
    /// <summary>
    /// Speed of horizontal translation
    /// </summary>
    public float speedHorizontal = 10f;
    /// <summary>
    /// Speed of rotation
    /// </summary>
    public float rotationSpeed = 10f;
    /// <summary>
    /// Speed to scale
    /// </summary>
    public float scalingSpeed = 10f;
    /// <summary>
    /// A SnapTurnProvider reference from XRRig
    /// </summary>
    public SnapTurnProvider snap;
    /// <summary>
    /// A Walk reference from XRRig
    /// </summary>
    public WalkSystemBase walk;
    /// <summary>
    /// The minimum distance to attach an object in a controller
    /// </summary>
    public float minDistanceToAttach = 0.1f;
    /// <summary>
    /// The minimum scale magnitude of the object
    /// </summary>
    public float minScaleMagnitude = 0.5f;
    /// <summary>
    /// The maximum scale magnitude of the object 
    /// </summary>
    public float maxScaleMagnitude = 5f;

    /// <summary>
    /// Enum of possible interactions types
    /// </summary>
    public enum machineStates
    {
        translading,
        rotating,
        scaling,
    };
    private machineStates states = 0;

    /// <summary>
    /// The controller XRGrabInteractable reference
    /// </summary>
    private XRGrabInteractable grabInteractable;
    /// <summary>
    /// The controller transform
    /// </summary>
    private Transform controllerTransform;
    /// <summary>
    /// Is the controller the left hand input?
    /// </summary>
    private bool isInputLeft;
    /// <summary>
    /// The vertical axis reference
    /// </summary>
    private float verticalAxis;
    /// <summary>
    /// The horizontal axis reference
    /// </summary>
    private float horizontalAxis;
    /// <summary>
    /// Is the controller interactor a ray interactor?
    /// </summary>
    private bool isRayInteractor;
    /// <summary>
    /// MovementType of the controller
    /// </summary>
    private MovementType movementType;
    /// <summary>
    /// Line length of a ray interactor
    /// </summary>
    private float lineLength;
    /// <summary>
    /// The object has started with track position?
    /// </summary>
    private bool startsTrackedPosition;
    /// <summary>
    /// The starter scale of the object
    /// </summary>
    private Vector3 starterScale;
    /// <summary>
    /// The vector direction to object translate
    /// </summary>
    private Vector3 vectorDirection;
    /// <summary>
    /// The position of the controller at the start of frame
    /// </summary>
    private Vector3 controllerPositionOnStartOfFrame;
    /// <summary>
    /// The position of the object when it is selected
    /// </summary>
    private Vector3 transformPositionOnSelect;
    /// <summary>
    /// The proper object's rigidbody
    /// </summary>
    private new Rigidbody rigidbody;
    private bool isInterpolatingPosition;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        movementType = grabInteractable.movementType;

        startsTrackedPosition = grabInteractable.trackPosition;
        starterScale = gameObject.transform.localScale;

        rigidbody = GetComponent<Rigidbody>();

        grabInteractable.onSelectEnter.AddListener((interactor) => {
            controllerTransform = interactor.transform;
            transformPositionOnSelect = transform.position;

            isRayInteractor = interactor as XRRayInteractor;

            grabInteractable.movementType = (isRayInteractor ? movementType : MovementType.Kinematic);

            isInputLeft = interactor.GetComponent<XRController>().controllerNode == UnityEngine.XR.XRNode.LeftHand;

            if (isRayInteractor)
            {
                if (!startsTrackedPosition)
                    SetInteractions(false);

                var ray = interactor.GetComponent<XRRayInteractor>();
                lineLength = ray.maxRaycastDistance;

                ray.maxRaycastDistance = 0;
            }
        });

        grabInteractable.onSelectExit.AddListener((interactor) => {

            grabInteractable.trackPosition = startsTrackedPosition;

            if (isRayInteractor)
            {
                interactor.GetComponent<XRRayInteractor>().maxRaycastDistance = lineLength;
            }

            SetInteractions(true);
        });
    }

    private void FixedUpdate()
    {
        if(controllerTransform != null && !isInterpolatingPosition)
            controllerPositionOnStartOfFrame = controllerTransform.position;

        if (grabInteractable.isSelected && isRayInteractor && !isInterpolatingPosition)
        {
            StartCoroutine(InterpolatePosition());
        }
    }

    void Update()
    {
        if (!grabInteractable.isSelected)
            return;

        if (Input.GetKeyDown(OculusInput.RightHandThumbstick))
        {
            states = (states == machineStates.rotating ? machineStates.translading : machineStates.rotating);
        }

        else if (Input.GetKeyDown(OculusInput.LeftHandThumbstick))
        {
            states = (states == machineStates.scaling ? machineStates.translading : machineStates.scaling);
        }
    }

    void LateUpdate()
    {

        if (grabInteractable.isSelected)
        {

            if (isInputLeft)
            {
                verticalAxis = OculusInput.LeftHandVerticalAxis;
                horizontalAxis = OculusInput.LeftHandHorizontalAxis;
            }
            else
            {
                verticalAxis = OculusInput.RightHandVerticalAxis;
                horizontalAxis = OculusInput.RightHandHorizontalAxis;
            }

            if (states == machineStates.translading)
            {
                if (isRayInteractor)
                    TransladingInteraction();
            }

            else if (states == machineStates.rotating)
            {

                RotatingInteraction();
            }

            else
            {
                ScaleInteraction();
            }

            AdjustMovements();
        }

        else
            rigidbody.velocity = Vector3.zero;
    }

    /// <summary>
    /// Set rigidbody's velocity
    /// </summary>
    private void TransladingInteraction()
    {
        float xrRigY = controllerTransform.eulerAngles.y * Mathf.Deg2Rad;

        rigidbody.velocity = new Vector3(Mathf.Sin(xrRigY), -Mathf.Sin(controllerTransform.eulerAngles.x * Mathf.Deg2Rad),
            Mathf.Cos(xrRigY)) * verticalAxis * speedVertical;

        xrRigY += 90;
        rigidbody.velocity += new Vector3(Mathf.Sin(xrRigY), 0,
            Mathf.Cos(xrRigY)) * horizontalAxis * speedHorizontal;
    }

    /// <summary>
    /// Set grabInteractable.trackPosition then call SetInteraction(bool state) with a proper parameter
    /// </summary>
    private void AdjustMovements()
    {
        if (!startsTrackedPosition && !grabInteractable.trackPosition)
        {
            grabInteractable.trackPosition = (Vector3.Distance(gameObject.transform.position, controllerTransform.position) <= minDistanceToAttach);
        }

        SetInteractions(grabInteractable.trackPosition && states == machineStates.translading);
    }

    /// <summary>
    /// Set transform's euler angles
    /// </summary>
    private void RotatingInteraction()
    {
        if (!grabInteractable.isSelected)
            return;

        switch (axisToRotate)
        {
            case AxisToRotate.x:
                transform.eulerAngles += new Vector3(rotationSpeed * horizontalAxis, 0, 0);
                break;
            case AxisToRotate.y:
                transform.eulerAngles += new Vector3(0, rotationSpeed * horizontalAxis, 0);
                break;
            case AxisToRotate.z:
                transform.eulerAngles += new Vector3(0, 0, rotationSpeed * horizontalAxis);
                break;
        }

        if (Input.GetKeyDown(OculusInput.ButtonA))
        {
            if (axisToRotate == AxisToRotate.z)
                axisToRotate = AxisToRotate.x;
            else
                axisToRotate++;
        }
    }

    /// <summary>
    /// Scale the object with the joystick
    /// </summary>
    private void ScaleInteraction()
    {
        if (verticalAxis < 0)
        {
            if (transform.localScale.magnitude > minScaleMagnitude && transform.localScale.x > 0 &&
                transform.localScale.y > 0 &&
                transform.localScale.z > 0)
            {

                transform.localScale += new Vector3(1, 1, 1) * verticalAxis * Time.deltaTime * scalingSpeed;
            }

            else
            {
                transform.localScale = starterScale * (minScaleMagnitude / starterScale.magnitude);
            }
        }
        else
        {
            if (transform.localScale.magnitude < maxScaleMagnitude)
            {
                transform.localScale += new Vector3(1, 1, 1) * verticalAxis * Time.deltaTime * scalingSpeed;
            }
        }
    }

    /// <summary>
    /// Enable/Disable Walk and SnapTurnProviderComponent
    /// </summary>
    /// <param name="state">Do you want to enable(True) or disable(False) a component?</param>
    private void SetInteractions(bool state)
    {
        if (isInputLeft)
        {
            if (walk)
            {
                if (walk.useLeftThumbstick)
                    walk.enabled = state;
                else if (snap)
                    snap.enabled = state;
            }
            else if (snap)
            {
                snap.enabled = state;
            }
        }
        else
        {
            if (walk)
            {
                if (walk.useLeftThumbstick && snap != null)
                    snap.enabled = state;
                else
                    walk.enabled = state;
            }
            else if (snap)
            {
                snap.enabled = state;
            }
        }
    }

    /// <summary>
    /// Make the object mirror the controller movements
    /// </summary>
    private IEnumerator InterpolatePosition()
    {
        isInterpolatingPosition = true;
        yield return new WaitForEndOfFrame();

        //Calculating vector direction
        vectorDirection = controllerTransform.position - controllerPositionOnStartOfFrame;

        transform.position += vectorDirection * 1.5f;

        isInterpolatingPosition = false;
    }
}
