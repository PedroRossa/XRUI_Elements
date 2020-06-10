using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class XRButton : MonoBehaviour
{
    [Header("Internal Properties")]
    public Transform backgroundPanel;
    public Transform buttonTransform;
    public AudioClip clickSound;

    [Header("Color Event Properties")]
    public Color backgroundColor = Color.gray;
    public Color proximityColor = Color.blue;
    public Color normalColor = Color.white;
    public Color clickColor = Color.green;

    [Header("States")]
    [ReadOnly]
    public bool isPressed;

    [Header("Events")]
    public UnityEvent onClickDown;
    public UnityEvent onClickPress;
    public UnityEvent onClickUp;

    private AudioSource audioSource;
    private Rigidbody buttonRigidBody;
    private XRMeshFeedback meshFeedback;

    private float initialPos;
    private float distance = 1;

    //This function need's to be called on child classes
    protected void BaseOnValidate()
    {
        if (meshFeedback == null)
            meshFeedback = GetComponentInChildren<XRMeshFeedback>();

        if (meshFeedback != null)
            meshFeedback.proximityColor = proximityColor;
        
        backgroundPanel.GetComponent<SpriteRenderer>().color = backgroundColor;

        MeshRenderer mr = buttonTransform.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            mr.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
            mr.sharedMaterial.color = normalColor;
        }
    }

    void Start()
    {
        onClickDown.AddListener(OnClickDownFucntion);
        onClickPress.AddListener(OnClickPressFunction);
        onClickUp.AddListener(OnClickUpFucntion);

        initialPos = buttonTransform.localPosition.z;

        ConfigureAudioSource();

        if (meshFeedback == null)
            meshFeedback = GetComponentInChildren<XRMeshFeedback>();

        meshFeedback.onProximityAreaStay.AddListener(OnProximityStayFunction);
        meshFeedback.onProximityAreaExit.AddListener(OnProximityExitFuncion);

        buttonRigidBody = buttonTransform.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ConstraintLocally();
    }

    void Update()
    {
        ButtonLoop();
    }

    private void ButtonLoop()
    {
           distance = buttonTransform.localPosition.z;

        if (distance <= 0)
        {
            buttonTransform.localPosition = Vector3.zero;

            if (!isPressed)
            {
                isPressed = true;
                onClickDown?.Invoke();
            }
            else
            {
                onClickPress?.Invoke();
            }
        }
        else
        {
            if (distance > initialPos)
                buttonTransform.localPosition = new Vector3(0, 0, initialPos);
            else
                buttonTransform.localPosition = new Vector3(0, 0, buttonTransform.localPosition.z);

            if (isPressed)
            {
                isPressed = false;
                onClickUp?.Invoke();
            }
        }
    }

    private void ConstraintLocally()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(buttonRigidBody.velocity);
        localVelocity.x = 0;
        localVelocity.y = 0;

        Vector3 localPosition = transform.InverseTransformPoint(buttonRigidBody.position);
        localPosition.x = 0;
        localPosition.y = 0;

        buttonRigidBody.velocity = transform.TransformDirection(localVelocity);
        buttonRigidBody.position = transform.TransformPoint(localPosition);

        //fix local rotation to zero
        buttonRigidBody.transform.localRotation = Quaternion.identity;
    }

    private void ConfigureAudioSource()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            return;

        audioSource.playOnAwake = false;

        if (clickSound != null)
            audioSource.clip = clickSound;
    }


    private void OnClickDownFucntion()
    {
        isPressed = true;
        buttonTransform.GetComponent<MeshRenderer>().sharedMaterial.color = clickColor;

        if (audioSource != null)
            audioSource.Play();

        meshFeedback.enabled = false;
    }

    private void OnClickPressFunction()
    {
    }

    private void OnClickUpFucntion()
    {
        meshFeedback.enabled = true;

        isPressed = false;
        buttonTransform.GetComponent<MeshRenderer>().sharedMaterial.color = normalColor;
    }


    private void OnProximityStayFunction()
    {
    }

    private void OnProximityExitFuncion()
    {
    }

    public void SetNormalColor(Color color)
    {
        normalColor = color;
    }

    public void SetClickColor(Color color)
    {
        clickColor = color;
    }
}
