using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class XRButton : MonoBehaviour
{
    private XRFeedback xrFeedback;
    private AudioSource audioSource;
    private Rigidbody frontPanelRigidBody;

    [Header("Internal Properties")]
    public SpriteRenderer frontPanel;
    public MeshRenderer wireframeMesh;
    public AudioClip clickSound;

    [Header("Properties")]
    [Range(0.0f, 1.0f)]
    public float lineBoxWidth = 0.013f;
    public Color lineBoxColor = Color.white;

    [Header("Color Event Properties")]
    public Color normalColor = Color.white;
    public Color clickColor = Color.green;

    [Header("States")]
    [ReadOnly]
    public bool isPressed;

    [Header("Events")]
    public UnityEvent onClickDown;
    public UnityEvent onClickPress;
    public UnityEvent onClickUp;

    private float initialPos;
    private float distance = 1;

    //This function need's to be called on child classes
    protected void BaseOnValidate()
    {
        frontPanel.color = normalColor;

        if (wireframeMesh == null)
            return;

        wireframeMesh.sharedMaterial = new Material(Shader.Find("Unlit/Wireframe"));

        wireframeMesh.sharedMaterial.SetFloat("_WireframeVal", lineBoxWidth);
        wireframeMesh.sharedMaterial.SetColor("_Color", lineBoxColor);
    }

    void Start()
    {
        onClickDown.AddListener(OnClickDownFucntion);
        onClickPress.AddListener(OnClickPressFunction);
        onClickUp.AddListener(OnClickUpFucntion);

        initialPos = frontPanel.transform.localPosition.z;

        if (wireframeMesh != null)
            wireframeMesh.sharedMaterial.color = Color.clear;

        ConfigureAudioSource();

        xrFeedback = GetComponentInChildren<XRFeedback>();

        if (xrFeedback != null)
        {
            xrFeedback.onHoverStay.AddListener(OnHoverStayFunction);
            xrFeedback.onHoverExit.AddListener(OnHoverExitFuncion);
            xrFeedback.onTouchStay.AddListener(OnTouchStayFunction);
        }

        frontPanelRigidBody = frontPanel.GetComponent<Rigidbody>();
    }

    void Update()
    {
        ButtonLoop();
    }

    private void ButtonLoop()
    {
        distance = frontPanel.transform.localPosition.z;

        if (distance <= 0)
        {
            frontPanel.transform.localPosition = Vector3.zero;

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
            {
                frontPanel.transform.localPosition = new Vector3(0, 0, initialPos);
            }

            if (isPressed)
            {
                isPressed = false;
                onClickUp?.Invoke();
            }
        }

    }

    private void ConstraintLocally()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(frontPanelRigidBody.velocity);
        localVelocity.x = 0;
        localVelocity.y = 0;

        Vector3 localPosition = transform.InverseTransformPoint(frontPanelRigidBody.position);
        localPosition.x = 0;
        localPosition.y = 0;

        frontPanelRigidBody.velocity = transform.TransformDirection(localVelocity);
        frontPanelRigidBody.position = transform.TransformPoint(localPosition);
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
        frontPanel.color = clickColor;

        if (audioSource != null)
            audioSource.Play();

        xrFeedback.gameObject.SetActive(false);
    }

    private void OnClickPressFunction()
    {
    }

    private void OnClickUpFucntion()
    {
        xrFeedback.gameObject.SetActive(true);

        isPressed = false;
        frontPanel.color = normalColor;

        ConstraintLocally();
    }


    private void OnHoverStayFunction()
    {
        ConstraintLocally();

        if (wireframeMesh == null)
            return;

        Color alphaColor = lineBoxColor;

        alphaColor.a = 0.15f;
        wireframeMesh.sharedMaterial.color = alphaColor;
    }

    private void OnHoverExitFuncion()
    {
        if (wireframeMesh == null)
            return;

        wireframeMesh.sharedMaterial.color = Color.clear;
    }

    private void OnTouchStayFunction()
    {
        if (distance <= 0 || wireframeMesh == null)
            return;

        Color alphaColor = lineBoxColor;
        float normalizedDistance = 1 / initialPos * distance;

        alphaColor.a = (1 - normalizedDistance) + 0.15f;
        wireframeMesh.sharedMaterial.color = alphaColor;

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
