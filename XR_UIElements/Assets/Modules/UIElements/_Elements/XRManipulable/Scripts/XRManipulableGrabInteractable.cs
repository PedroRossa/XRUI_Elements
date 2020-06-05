using NaughtyAttributes;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(MeshRenderer))]
public class XRManipulableGrabInteractable : XRGrabInteractable
{
    [Header("ManipulableProperties")]
    public bool isScaleElement;
    public bool isRotationElement;

    public bool moveParent;

    [ShowIf("moveParent")]
    public Transform parentToMove;

    private Material originalMaterial;
    private MeshRenderer meshRenderer;

    private bool moving = false;
    private Vector3 offset;

    protected override void Awake()
    {
        base.Awake();
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer.material == null)
            meshRenderer.material = new Material(Shader.Find("Unlit/TransparentColor"));

        originalMaterial = meshRenderer.material;

        onSelectEnter.AddListener(OnSelectEnterFunction);
        onSelectExit.AddListener(OnSelectExitFunction);
    }

    private void Update()
    {
        if (parentToMove == null)
            return;

        if(moveParent && moving)
            MoveParent();
    }

    private void OnSelectEnterFunction(XRBaseInteractor interactor)
    {
        moving = true;

        if(parentToMove != null)
            offset = (transform.position - parentToMove.position);
    }

    private void OnSelectExitFunction(XRBaseInteractor interactor)
    {
        moving = false;
        meshRenderer.material = originalMaterial;
    }

    private void MoveParent()
    {
        parentToMove.position = transform.position - offset;
    }

    public void StopMoving()
    {
        moving = false;
    }
}
