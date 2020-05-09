using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class XRButton : MonoBehaviour
{
    private XRHoverFeedback xrHoverFeedback;

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
    public Color clickColor = Color.green;

    [Header("States")]
    [ReadOnly]
    public bool isHover;
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

        wireframeMesh.sharedMaterial = new Material(Shader.Find("Unlit/Vizlab/Wireframe"));

        wireframeMesh.sharedMaterial.SetFloat("_WireframeVal", lineBoxWidth);
        wireframeMesh.sharedMaterial.SetColor("_Color", lineBoxColor);
    }

    void Start()
    {
        onClickDown.AddListener(OnClickDownFucntion);
        onClickPress.AddListener(OnClickPressFunction);
        onClickUp.AddListener(OnClickUpFucntion);

        initialPos = frontPanel.transform.localPosition.z;

        wireframeMesh.sharedMaterial.color = Color.clear;

        xrHoverFeedback = GetComponentInChildren<XRHoverFeedback>();

        if (xrHoverFeedback != null)
        {
            xrHoverFeedback.onHoverEnter.AddListener(OnHoverEnterFuncion);
            xrHoverFeedback.onHoverExit.AddListener(OnHoverExitFuncion);
        }
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
        float normalizedDistance = 1 / initialPos * distance;
        Color c = lineBoxColor;
        c.a = (1 - normalizedDistance) + 0.5f;
        wireframeMesh.sharedMaterial.color = c;
    }

    private void OnHoverExitFuncion()
    {
        Debug.Log("Hover Exit");
        wireframeMesh.sharedMaterial.color = Color.clear;
    }
}
