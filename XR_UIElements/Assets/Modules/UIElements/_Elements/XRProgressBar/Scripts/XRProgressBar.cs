using TMPro;
using UnityEngine;

public class XRProgressBar : MonoBehaviour
{
    [Header("Prefab References")]
    public Transform progressElement;
    public TextMeshPro tmpProgress;

    public GameObject backgroundObject;
    public GameObject totalProgressObject;
    public GameObject currentProgressObject;

    [Header("Color Properties")]
    public Color backgroundColor = Color.white;
    public Color32 totalProgressColor = new Color32(27,140,175,255);
    public Color32 currentProgressColor = new Color32(10,198,242,255);

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float progress = 0.75f; 
    public float Progress
    {
        get => progress;
        set => SetProgress(value);
    }

    public XRUIElements_Helper.UnityFloatEvent onProgressChange;

    private void OnValidate()
    {
        UpdateProgressPrefab();
        UpdateColors();
    }

    private void SetProgress(float value)
    {
        if (progress == value) return;

        //limit the value between 0 and 1
        progress = Mathf.Clamp01(value);

        UpdateProgressPrefab();
        onProgressChange?.Invoke(progress);
    }

    private void UpdateProgressPrefab()
    {
        if (progressElement == null || tmpProgress == null)
            return;

        progressElement.localScale = new Vector3(Progress, 1, 1);

        int currentProgress = (int)(Progress * 100);
        tmpProgress.text = currentProgress.ToString() + " %";
    }

    private void UpdateColors()
    {
        if (backgroundObject != null)
            backgroundObject.GetComponent<MeshRenderer>().sharedMaterial.color = backgroundColor;

        if (totalProgressObject != null)
            totalProgressObject.GetComponent<MeshRenderer>().sharedMaterial.color = totalProgressColor;

        if (currentProgressObject != null)
            currentProgressObject.GetComponent<MeshRenderer>().sharedMaterial.color = currentProgressColor;
    }

}
