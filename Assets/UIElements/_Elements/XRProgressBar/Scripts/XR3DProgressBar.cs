using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XR3DProgressBar : XRProgressBarBase
{
    public Material backgroundMaterial;
    public Material totalProgressMaterial;
    public Material progressMaterial;
    public Material progressPointMaterial;

    private MeshRenderer backgroundMesh;
    private MeshRenderer totalProgressMesh;
    private MeshRenderer progressMesh;
    private MeshRenderer progressPointMesh;

    protected override void OnValidate()
    {
        base.OnValidate();
        GetMeshReferences();
    }

    protected override void Awake()
    {
        base.Awake();
        GetMeshReferences();
    }

    protected override void UpdateColors()
    {
        if (backgroundMesh != null)
            backgroundMesh.sharedMaterial.color = backgroundColor;

        if (totalProgressMesh != null)
            totalProgressMesh.sharedMaterial.color = totalProgressColor;

        if (progressMesh != null)
            progressMesh.sharedMaterial.color = isEnabled ? normalColor : disabledColor;

        if (progressPointMesh != null)
            progressPointMesh.sharedMaterial.color = isEnabled ? normalColor : disabledColor;
    }

    private void GetMeshReferences()
    {
        if (backgroundMesh == null && background != null)
        {
            backgroundMesh = background.GetComponent<MeshRenderer>();
            
            if (backgroundMaterial != null && backgroundMesh.sharedMaterial == null)
                backgroundMesh.material = new Material(backgroundMaterial);
        }

        if (totalProgressMesh == null && totalProgress != null)
        {
            totalProgressMesh = totalProgress.GetComponent<MeshRenderer>();
            
            if (totalProgressMaterial != null && totalProgressMesh.sharedMaterial == null)
                totalProgressMesh.material = new Material(totalProgressMaterial);
        }

        if (progressMesh == null && progressElement != null && progressElement.childCount > 0)
        {
            progressMesh = progressElement.GetChild(0).GetComponent<MeshRenderer>();
            
            if (progressMaterial != null && progressMesh.sharedMaterial == null)
                progressMesh.material = new Material(progressMaterial);
        }

        if (progressPointMesh == null && progressPointElement != null && progressPointElement.childCount > 0)
        {
            progressPointMesh = progressPointElement.GetChild(0).GetComponent<MeshRenderer>();
            
            if (progressPointMaterial != null && progressPointMesh.sharedMaterial == null)
                progressPointMesh.material = new Material(progressPointMaterial);
        }
    }

    protected override void WhenTouchStart(XRBaseInteractor interactor)
    {
        base.WhenTouchStart(interactor);
    }

    protected override void WhenTouchEnd(XRBaseInteractor interactor)
    {
        base.WhenTouchEnd(interactor);
    }
}
