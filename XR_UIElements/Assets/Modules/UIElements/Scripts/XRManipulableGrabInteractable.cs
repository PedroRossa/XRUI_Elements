using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(MeshRenderer))]
public class XRManipulableGrabInteractable : XRGrabInteractable
{
    public bool isScaleElement;
    public bool isRotationElement;

    public Material selectMaterial;

    private Material originalMaterial;
    private MeshRenderer meshRenderer;

    protected override void Awake()
    {
        base.Awake();
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer.material == null)
            meshRenderer.material = new Material(Shader.Find("Unlit/TransparentColor"));

        originalMaterial = meshRenderer.material;

        if(selectMaterial == null)
        {
            selectMaterial = new Material(Shader.Find("Unlit/TransparentColor"));
            selectMaterial.color = Color.magenta;
        }

        onSelectEnter.AddListener(OnSelectEnterFunction);
        onSelectExit.AddListener(OnSelectExitFunction);
    }

    private void OnSelectEnterFunction(XRBaseInteractor interactor)
    {
        meshRenderer.material = selectMaterial;
    }

    private void OnSelectExitFunction(XRBaseInteractor interactor)
    {
        meshRenderer.material = originalMaterial;
    }
}
