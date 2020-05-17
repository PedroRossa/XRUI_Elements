using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class XR2DWindow : MonoBehaviour
{
    private XR2DManipulableContent manipulableContent;
    private bool minimizeState = false;

    [Header("Internal References")]
    public GameObject headerPanel;
    public GameObject bodyPanel;
    public TextMeshPro tmpTitle;

    [ShowIf("showLeftButton")]
    public XRButton btnLeft;
    [ShowIf("showMiddleButton")]
    public XRButton btnMiddle;
    [ShowIf("showRightButton")]
    public XRButton btnRight;

    public bool showLeftButton;
    public bool showMiddleButton;
    public bool showRightButton;

    public bool showDraggableElements;

    [Header("Color properties")]
    public Color headerColor = Color.gray;
    public Color bodyColor = Color.gray;

    public string windowTitle;

    [ShowIf("showLeftButton")]
    public UnityEvent onLeftButtonClickDown;
    [ShowIf("showMiddleButton")]
    public UnityEvent onMiddleButtonClickDown;
    [ShowIf("showRightButton")]
    public UnityEvent onRightButtonClickDown;

    [Button]
    public void UpdateVisual()
    {
        if (headerPanel != null)
        {
            MeshRenderer headerMesh = headerPanel.GetComponent<MeshRenderer>();

            if (headerMesh.sharedMaterial == null)
                headerMesh.sharedMaterial = new Material(Shader.Find("Unlit/TransparentColor"));

            headerMesh.sharedMaterial.color = headerColor;
        }

        if (bodyPanel != null)
        {
            MeshRenderer bodyMesh = bodyPanel.GetComponent<MeshRenderer>();

            if (bodyMesh.sharedMaterial == null)
                bodyMesh.sharedMaterial = new Material(Shader.Find("Unlit/TransparentColor"));

            bodyMesh.sharedMaterial.color = bodyColor;
        }

        if (manipulableContent == null)
            manipulableContent = GetComponentInChildren<XR2DManipulableContent>();

        manipulableContent.manipulationIsActive = showDraggableElements;
        manipulableContent.UpdateVisual();

        btnLeft.gameObject.SetActive(showLeftButton);
        btnMiddle.gameObject.SetActive(showMiddleButton);
        btnRight.gameObject.SetActive(showRightButton);
    }
    
    //Runs only in editor
    private void OnValidate()
    {
        if (tmpTitle != null) 
            tmpTitle.text = windowTitle;

        UpdateVisual();
    }


    private void Awake()
    {
        UpdateVisual();

        if (manipulableContent == null)
            manipulableContent = GetComponentInChildren<XR2DManipulableContent>();

        manipulableContent.manipulationIsActive = showDraggableElements;

        if (btnLeft != null)
        {
            if (onLeftButtonClickDown != null)
                btnLeft.onClickDown.AddListener(onLeftButtonClickDown.Invoke);

            btnLeft.gameObject.SetActive(showLeftButton);
        }

        if (btnMiddle != null)
        {
            if (onMiddleButtonClickDown != null)
                btnMiddle.onClickDown.AddListener(onMiddleButtonClickDown.Invoke);

            btnMiddle.gameObject.SetActive(showMiddleButton);
        }

        if (btnRight != null)
        {
            if (onRightButtonClickDown != null)
                btnRight.onClickDown.AddListener(onRightButtonClickDown.Invoke);

            btnRight.gameObject.SetActive(showRightButton);
        }
    }

    public void ToggleWindowState()
    {
        minimizeState = !minimizeState;
        bodyPanel.transform.parent.gameObject.SetActive(minimizeState);
    }
}
