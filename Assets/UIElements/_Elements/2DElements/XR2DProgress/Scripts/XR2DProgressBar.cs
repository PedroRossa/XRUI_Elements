using TMPro;
using UnityEngine;

public class XR2DProgressBar : MonoBehaviour
{
    [Header("Prefab References")]
    public Transform progressElement;

    public SpriteRenderer backgroundSprite;
    public SpriteRenderer totalProgressSprite;
    public SpriteRenderer currentProgressSprite;
    public SpriteRenderer progressPointSprite;

    public TextMeshPro tmpProgress;

    [Header("Color Properties")]
    public Color backgroundColor = Color.white;
    public Color32 totalProgressColor = new Color32(27, 140, 175, 255);
    public Color32 currentProgressColor = new Color32(10, 198, 242, 255);

    public bool showBackground = true;
    public bool showProgressPoint = true;
    public bool showProgressText = true;

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
        if (progressElement != null)
            progressElement.localScale = new Vector3(Progress, 1, 1);

        if (backgroundSprite != null)
            backgroundSprite.enabled = showBackground;

        if (tmpProgress != null)
        {
            int currentProgress = (int)(Progress * 100);
            tmpProgress.text = currentProgress.ToString() + " %";
            tmpProgress.enabled = showProgressText;
        }

        if (progressPointSprite != null)
        {
            progressPointSprite.transform.localPosition = new Vector3(Progress, 0, 0);
            progressPointSprite.enabled = showProgressPoint;
        }
    }

    private void UpdateColors()
    {
        if (backgroundSprite != null)
            backgroundSprite.color = backgroundColor;

        if (totalProgressSprite != null)
            totalProgressSprite.color = totalProgressColor;

        if (currentProgressSprite != null)
            currentProgressSprite.color = currentProgressColor;

        if (progressPointSprite != null)
            progressPointSprite.color = currentProgressColor;
    }

}
