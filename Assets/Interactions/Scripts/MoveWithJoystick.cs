using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.XR.Interaction.Toolkit.XRBaseInteractable;

public class MoveWithJoystick : MonoBehaviour
{
    public float speedVertical = 10f;
    public float speedHorizontal = 10f;
    public float rotationSpeed = 10f;
    public float scalingSpeed = 10f;
    public SnapTurnProvider snap;
    public Walk walk;
    public float minDistanceToAttach = 0.1f;
    public float minScale = 0.5f;
    public float maxScale = 5f;
    
    [flags]
    public enum machineStates{
        translading = 0,
        rotating = 1,
        scaling = 2,
    };
    private machineStates states = 0;

    private XRGrabInteractable grabInteractable;
    private Transform controllerTransform;
    private bool isInputLeft;
    private float verticalAxis;
    private float horizontalAxis;
    private bool isRayInteractor;
    private MovementType movementType;
    private float lineLength;
    private bool startsTrackedPosition;
    private Vector3 starterScale;
    private Vector3 vectorDirection;
    private Vector3 controllerPositionOnSelect;
    private Vector3 transformPositionOnSelect;
    private Rigidbody rigidbody;

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
                    SetInteractions(false);

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

            SetInteractions(true);
        });
    }

    private void FixedUpdate()
    {
        if(grabInteractable.isSelected && isRayInteractor)
        {
            InterpolatePosition();
        }
    }

    void Update()
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

    private void TransladingInteraction()
    {
        float xrRigY = controllerTransform.eulerAngles.y * Mathf.Deg2Rad;

        rigidbody.velocity = new Vector3(Mathf.Sin(xrRigY), -Mathf.Sin(controllerTransform.eulerAngles.x * Mathf.Deg2Rad),
            Mathf.Cos(xrRigY)) * verticalAxis * speedVertical;

        xrRigY += 90;
        rigidbody.velocity += new Vector3(Mathf.Sin(xrRigY), 0,
            Mathf.Cos(xrRigY)) * horizontalAxis * speedHorizontal;
    }

    private void AdjustMovements()
    {
        if (!startsTrackedPosition && !grabInteractable.trackPosition)
        {
            grabInteractable.trackPosition = (Vector3.Distance(gameObject.transform.position, controllerTransform.position) <= minDistanceToAttach);
        }

        SetInteractions(grabInteractable.trackPosition && states == machineStates.translading);
    }

    private void RotatingInteraction()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,
            transform.eulerAngles.y + rotationSpeed * horizontalAxis,
            transform.eulerAngles.z +  rotationSpeed * verticalAxis);
    } 

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

    private void SetInteractions(bool state)
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
