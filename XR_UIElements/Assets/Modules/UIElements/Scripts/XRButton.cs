using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class XRButton : MonoBehaviour
{
    protected XRButtonHover xrButtonHover;

    [Header("Internal Properties")]
    public SpriteRenderer frontPanel;
    public MeshRenderer wireframeMesh;

    [Header("Properties")]
    [Range(0.0f, 1.0f)]
    public float hoverDistance = 0.1f;
    [Range(0.0f, 1.0f)]
    public float lineBoxWidth = 0.013f;
    public Color lineBoxColor = Color.white;

    [Header("Color Event Properties")]
    public Color normalColor = Color.white;
    public Color hoverColor = new Color(0.25f, 0.25f, 0.25f);
    public Color clickColor = Color.green;

    [Header("States")]
    [ReadOnly]
    public bool isHover;
    [ReadOnly]
    public bool isPressed;

    [Header("Events")]

    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;
    public UnityEvent onClickDown;
    public UnityEvent onClickPress;
    public UnityEvent onClickUp;

    private float initialPos;
    private float distance = 1;

    //This function need's to be called on child classes
    protected void BaseOnValidate()
    {
        if (xrButtonHover == null)
        {
            xrButtonHover = GetComponentInChildren<XRButtonHover>();
            xrButtonHover.xrButton = this;
        }

        xrButtonHover.SetHoverDistanceCollider();

        frontPanel.color = normalColor;

        wireframeMesh.sharedMaterial = new Material(Shader.Find("Unlit/Vizlab/Wireframe"));

        wireframeMesh.sharedMaterial.SetFloat("_WireframeVal", lineBoxWidth);
        wireframeMesh.sharedMaterial.SetColor("_Color", lineBoxColor);
    }

    private void Awake()
    {
        if (xrButtonHover == null)
            xrButtonHover = GetComponentInChildren<XRButtonHover>();

        xrButtonHover.xrButton = this;
        xrButtonHover.SetHoverDistanceCollider();
    }

    void Start()
    {
        onHoverEnter.AddListener(OnHoverEnterFuncion);
        onHoverExit.AddListener(OnHoverExitFuncion);
        onClickDown.AddListener(OnClickDownFucntion);
        onClickPress.AddListener(OnClickPressFunction);
        onClickUp.AddListener(OnClickUpFucntion);

        initialPos = frontPanel.transform.localPosition.z;

        wireframeMesh.sharedMaterial.color = Color.clear;
    }

    void Update()
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
            if (isPressed)
            {
                isPressed = false;
                onClickUp?.Invoke();
            }
        }
    }

    private void OnClickDownFucntion()
    {
        Debug.Log("Start Click");
        isPressed = true;
        frontPanel.color = clickColor;
    }

    private void OnClickPressFunction()
    {
        Debug.Log("Clicking");
    }

    private void OnClickUpFucntion()
    {
        Debug.Log("Stop Clicking");
        isPressed = false;
        frontPanel.color = normalColor;
    }

    private void OnHoverEnterFuncion()
    {
        Debug.Log("Hover Enter");
        frontPanel.color = hoverColor;

        float normalizedDistance = 1 / initialPos * distance;
        Color c = lineBoxColor;
        c.a = (1 - normalizedDistance) + 0.5f;
        wireframeMesh.sharedMaterial.color = c;
    }

    private void OnHoverExitFuncion()
    {
        Debug.Log("Hover Exit");
        frontPanel.color = normalColor;
        wireframeMesh.sharedMaterial.color = Color.clear;
    }
}
