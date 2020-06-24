using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRUI_3DProgressBar : XRUI_ProgressBarBase
{
    private MeshRenderer backgroundMesh;
    private MeshRenderer totalProgressMesh;
    private MeshRenderer progressMesh;
    private MeshRenderer progressPointMesh;

    public Material backgroundMaterial;
    public Material totalProgressMaterial;
    public Material progressMaterial;
    public Material progressPointMaterial;

    protected override void Awake()
    {
        base.Awake();

        xrFeedback.onTouchEnter.AddListener((XRController xrController) =>
        {
            progressPointMesh.transform.localScale *= 1.1f; //increase the scale in 10%
        });

        xrFeedback.onTouchExit.AddListener((XRController xrController) =>
        {
            progressPointMesh.transform.localScale /= 1.1f; //decrease the scale in 10%
        });
    }

    protected override void UpdateColors()
    {
        if (backgroundMesh != null)
        {
            backgroundMesh.sharedMaterial = new Material(backgroundMaterial);
            backgroundMesh.sharedMaterial.color = backgroundColor;
        }

        if (totalProgressMesh != null)
        {
            totalProgressMesh.sharedMaterial = new Material(totalProgressMaterial);
            totalProgressMesh.sharedMaterial.color = totalProgressColor;
        }

        if (progressMesh != null)
        {
            progressMesh.sharedMaterial = new Material(progressMaterial);
            progressMesh.sharedMaterial.color = isEnabled ? progressColor : xrUIColors.disabledColor;
        }

        if (progressPointMesh != null)
            progressPointMesh.sharedMaterial = new Material(progressPointMaterial);
    }

    protected override void SetElementReferences()
    {
        if (backgroundMesh == null && background != null)
            backgroundMesh = background.GetComponent<MeshRenderer>();

        if (totalProgressMesh == null && totalProgress != null)
            totalProgressMesh = totalProgress.GetComponent<MeshRenderer>();

        if (progressMesh == null && progressElement != null && progressElement.childCount > 0)
            progressMesh = progressElement.GetChild(0).GetComponent<MeshRenderer>();

        if (progressPointMesh == null && progressPointElement != null && progressPointElement.childCount > 0)
            progressPointMesh = progressPointElement.GetChild(0).GetComponent<MeshRenderer>();

        backgroundMaterial.color = backgroundColor;
        totalProgressMaterial.color = totalProgressColor;
        progressMaterial.color = isEnabled ? progressColor : xrUIColors.disabledColor;
        progressPointMaterial.color = isEnabled ? xrUIColors.normalColor : xrUIColors.disabledColor;

        backgroundMesh.material = backgroundMaterial;
        totalProgressMesh.material = totalProgressMaterial;
        progressMesh.material = progressMaterial;
        progressPointMesh.material = progressPointMaterial;
    }
}
