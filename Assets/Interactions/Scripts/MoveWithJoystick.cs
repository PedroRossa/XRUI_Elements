using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.XR.Interaction.Toolkit.XRBaseInteractable;

/// <summary>
/// Class used to move a XRGrabInteractable with the joysticks
/// </summary>
public class MoveWithJoystick : MonoBehaviour
{
    /// <summary>
    /// vertical movement speed
    /// </summary>
    public float speedVertical = 10f;
    /// <summary>
    /// horizontal movement speed
    /// </summary>
    public float speedHorizontal = 10f;
    /// <summary>
    /// rotation object speed
    /// </summary>
    public float rotationSpeed = 10f;
    /// <summary>
    /// scaling object speed
    /// </summary>
    public float scalingSpeed = 10f;
    /// <summary>
    /// XRRig snap turn provider reference
    /// </summary>
    public SnapTurnProvider snap;
    /// <summary>
    /// XRRig snap turn provider reference
    /// </summary>
    public Walk walk;
    /// <summary>
    /// The minimum distance to attach the object to controller
    /// </summary>
    public float minDistanceToAttach = 0.1f;
    /// <summary>
    /// The minimum possible scale of the object
    /// </summary>
    public float minScale = 0.5f;
    /// <summary>
    /// The maximum possible scale of the object
    /// </summary>
    public float maxScale = 5f;
    
    /// <summary>
    /// Interaction states of the object enum
    /// </summary>
    [flags]
    public enum machineStates{
        translading = 0,
        rotating = 1,
        scaling = 2,
    };
    /// <summary>
    /// Interaction states of the object enum instance
    /// </summary>
    private machineStates states = 0;

    /// <summary>
    /// Proper XRGrabInteractable of the object
    /// </summary>
    private XRGrabInteractable grabInteractable;
    /// <summary>
    /// The joystick transform which interact with the object
    /// </summary>
    private Transform controllerTransform;
    /// <summary>
    /// Is the left joystick which interact with the object?
    /// </summary>
    private bool isInputLeft;
    /// <summary>
    /// The vertical axis of the controller
    /// </summary>
    private float verticalAxis;
    /// <summary>
    /// The horizontal axis of the controller
    /// </summary>
    private float horizontalAxis;
    /// <summary>
    /// Is the interactor a ray interactor?
    /// </summary>
    private bool isRayInteractor;
    /// <summary>
    /// The current movement type of the object
    /// </summary>
    private MovementType movementType;
    /// <summary>
    /// A memory variable to take the line length of a xr ray interactor
    /// </summary>
    private float lineLength;
    /// <summary>
    /// Starts the object's position tracked?
    /// </summary>
    private bool startsTrackedPosition;
    /// <summary>
    /// The starter scale of the object
    /// </summary>
    private Vector3 starterScale;
    /// <summary>
    /// A directional vector where goes the object
    /// </summary>
    private Vector3 vectorDirection;
    /// <summary>
    /// The controller position at the moment the object is selected
    /// </summary>
    private Vector3 controllerPositionOnSelect;
    /// <summary>
    /// The object position at the moment it is selected
    /// </summary>
    private Vector3 transformPositionOnSelect;
    /// <summary>
    /// The proper object's rigidbody
    /// </summary>
    private Rigidbody rigidbody;

    /// <summary>
    /// Setup of the components and callbacks
    /// </summary>
    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        movementType = grabInteractable.movementType;

        startsTrackedPosition = grabInteractable.trackPosition;
        starterScale = gameObject.transform.localScale;

        rigidbody = GetComponent<Rigidbody>();

        grabInteractable.onSelectEnter.AddListener((interactor) => {
            controllerTransform = interactor.transform;
            controllerPositionOnSelect = controllerTransform.position;
            transformPositionOnSelect = transform.position;

            isRayInteractor = interactor as XRRayInteractor;

            grabInteractable.movementType = (isRayInteractor ? movementType : MovementType.Kinematic);

            isInputLeft = interactor.GetComponent<XRController>().controllerNode == UnityEngine.XR.XRNode.LeftHand;

            if(isRayInteractor)
            {
                if(!startsTrackedPosition)
                    SetMovements(false);

                var ray = interactor.GetComponent<XRRayInteractor>();
                lineLength = ray.maxRaycastDistance;

                ray.maxRaycastDistance = 0;
            }
        });

        grabInteractable.onSelectExit.AddListener((interactor) => {
            rigidbody.velocity = Vector3.zero;
            grabInteractable.trackPosition = startsTrackedPosition;

            if (isRayInteractor)
            {
                interactor.GetComponent<XRRayInteractor>().maxRaycastDistance = lineLength;
            }

            SetMovements(true);
        });
    }

    /// <summary>
    /// Interpolate the object position when it needs
    /// </summary>
    private void FixedUpdate()
    {
        if(grabInteractable.isSelected && isRayInteractor)
        {
            InterpolatePosition();
        }
    }

    /// <summary>
    /// Determines the object's interaction 
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(OculusInput.RightHandThumbstick))
        {
            states = (states == machineStates.rotating ? machineStates.translading : machineStates.rotating);
        }

        else if (Input.GetKeyDown(OculusInput.LeftHandThumbstick))
        {
            states = (states == machineStates.scaling ? machineStates.translading : machineStates.scaling);
        }
    }

    /// <summary>
    /// Make the interactions and adjust the movements
    /// </summary>
    void LateUpdate() {

        if (grabInteractable.isSelected) {

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

            if(states == machineStates.translading)
            {
                if(isRayInteractor)
                    TransladingInteraction();
            }

            else if(states == machineStates.rotating)
            {

                RotatingInteraction();
            }

            else
            {
                ScaleInteraction();
            }

            AdjustMovements();
        }
        
    }

    /// <summary>
    /// Translading object interaction
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
    /// Adjust the controller movements
    /// </summary>
    private void AdjustMovements()
    {
        if (!startsTrackedPosition && !grabInteractable.trackPosition)
        {
            grabInteractable.trackPosition = (Vector3.Distance(gameObject.transform.position, controllerTransform.position) <= minDistanceToAttach);
        }

        SetMovements(grabInteractable.trackPosition && states == machineStates.translading);
    }

    /// <summary>
    /// Rotating object interaction
    /// </summary>
    private void RotatingInteraction()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,
            transform.eulerAngles.y + rotationSpeed * horizontalAxis,
            transform.eulerAngles.z +  rotationSpeed * verticalAxis);
    } 

    /// <summary>
    /// Scale object interaction
    /// </summary>
    private void ScaleInteraction()
    {
        if(verticalAxis < 0)
        {
            if (transform.localScale.magnitude > minScale && transform.localScale.x > 0 &&
                transform.localScale.y > 0 &&
                transform.localScale.z > 0) {

                transform.localScale += new Vector3(1,1,1) * verticalAxis * Time.deltaTime * scalingSpeed;
            }

            else
            {
                transform.localScale = starterScale * (minScale / starterScale.magnitude);
            }
        }
        else
        {
            if(transform.localScale.magnitude < maxScale) {
                transform.localScale += new Vector3(1, 1, 1) * verticalAxis * Time.deltaTime * scalingSpeed;
            }
        }
    }

    /// <summary>
    /// Set the XR rig movements
    /// </summary>
    /// <param name="state"></param>
    private void SetMovements(bool state)
    {
        if(isInputLeft)
        {
            walk.enabled = state;
        }
        else
        {
            snap.enabled = state;
        }
    }

    /// <summary>
    /// Set the object velocity direction in relation to the controller position direction
    /// </summary>
    void InterpolatePosition()
    {
        //Calculando vetor direção
        vectorDirection = controllerTransform.position - controllerPositionOnSelect;


        //Transladando o objeto na direção do vetor
        //Soma a velocidade se estiver transladando, se não, seta diretamente
        if (states == machineStates.translading)
            rigidbody.velocity += vectorDirection *  walk.translationSpeed/2 * Time.deltaTime;
        else
            rigidbody.velocity = vectorDirection * walk.translationSpeed/2 * Time.deltaTime;
    }
}
