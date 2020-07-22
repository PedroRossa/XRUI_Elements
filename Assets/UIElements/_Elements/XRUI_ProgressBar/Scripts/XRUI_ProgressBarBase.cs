using NaughtyAttributes;
using System.Collections;
using TMPro;
using UnityEngine;

public abstract class XRUI_ProgressBarBase : XRUI_Base
{
    [Header("Prefab References")]
    public Transform background;
    public Transform totalProgress;
    public Transform progressElement;
    public Transform progressPointElement;
    public TextMeshPro tmpProgress;

    public bool canPush = false;

    [Header("Color Properties")]
    public Color backgroundColor = Color.white;
    public Color32 totalProgressColor = new Color32(27, 140, 175, 255);
    public Color32 progressColor = new Color32(27, 100, 175, 255);

    [Header("Visualization Properties")]
    public bool showBackground = true;
    public bool showProgressPoint = true;
    public bool showProgressText = true;

    [Header("Progress")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float progress = 0.75f;
    public float Progress
    {
        get => progress;
        set => SetProgress(value);
    }

    public float minRange = 0f;
    public float maxRange = 100f;
    public string symbol = "%";
    public int decimalPlaces = 0;

    public XRUI_Helper.UnityFloatEvent onProgressChange;

    private XRUI_2DDragInteractable dragInteractable;

    protected override void OnValidate()
    {
        base.OnValidate();

        SetElementReferences();
        SetElementsVisibility();
        UpdateProgress();
        UpdateColors();
    }

    private void Start()
    {
        SetElementReferences();
        SetElementsVisibility();
        UpdateColors();

        dragInteractable = GetComponentInChildren<XRUI_2DDragInteractable>();
        if (!isEnabled)
            dragInteractable.GetComponent<Collider>().enabled = false;
        else if (dragInteractable != null)
        {
            dragInteractable.interactable.onSelectEnter.AddListener((XRBaseInteractor) => { StartCoroutine(UpdateInfos()); });
            dragInteractable.interactable.onSelectEnter.AddListener((XRBaseInteractor) => { StopCoroutine(UpdateInfos()); });
        }
    }
    //protected virtual void Update()
    //{
    //    if (!isEnabled)
    //        return;

    //    if (dragInteractable.isSelected || (canPush && xrFeedback.IsTouching))
    //    {
    //        SetProgress(dragInteractable.normalizedValue);
    //        UpdateProgress();
    //    }
    //}
    private IEnumerator UpdateInfos()
    {
        SelectedEvent();
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(UpdateInfos());
    }

    private void SelectedEvent()
    {
        SetProgress(dragInteractable.normalizedValue);
        UpdateProgress();
    }

    private void SetElementsVisibility()
    {
        if (background != null)
            background.gameObject.SetActive(showBackground);

        if (progressPointElement != null)
            progressPointElement.gameObject.SetActive(showProgressPoint);

        if (tmpProgress != null)
            tmpProgress.gameObject.SetActive(showProgressText);
    }

    private void SetProgress(float value)
    {
        if (progress == value) return;

        //limit the value between 0 and 1
        progress = Mathf.Clamp01(value);

        UpdateProgress();
        onProgressChange?.Invoke(progress);
    }

    private void UpdateProgress()
    {
        if (progressElement != null)
            progressElement.localScale = new Vector3(Progress, 1, 1);

        if (tmpProgress != null)
        {
            float currentProgress = (float)(minRange + (Progress * (maxRange - minRange)));

            tmpProgress.text = currentProgress.ToString("F" + decimalPlaces) + symbol;
        }

        //Get child object that is containered between 0 - 1
        if (progressPointElement != null && progressPointElement.childCount > 0)
            progressPointElement.GetChild(0).transform.localPosition = new Vector3(Progress, 0, 0);
    }

    protected abstract void UpdateColors();
    protected abstract void SetElementReferences();
}
