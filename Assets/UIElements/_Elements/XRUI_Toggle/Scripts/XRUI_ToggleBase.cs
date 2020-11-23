using NaughtyAttributes;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Abstract and base class of XRUI toggle components
/// </summary>
public abstract class XRUI_ToggleBase : XRUI_Base
{
    [Header("Prefab References")]
    public GameObject bodyObject;
    public GameObject selectObject;

    [Header("Prefab Color Properties")]
    public Color32 selectedColor = Color.green;
    public Color32 unselectedColor = Color.gray;

    [Header("Prefab Background Color Properties")]
    public bool changeBGColor = false;
    [ShowIf("changeBGColor")]
    public Color32 selectedBGColor = Color.green;
    [ShowIf("changeBGColor")]
    public Color32 unselectedBGColor = Color.gray;

    [Header("General Properties")]
    public float animationSpeed = 0.1f;

    [SerializeField]
    private bool isSelected;
    public bool IsSelected
    {
        get => isSelected;
        set => SetToggleValue(value);
    }

    public Vector3 unselectPos = new Vector3(0, 0.5f, 0);
    public Vector3 selectPos = new Vector3(0, -0.5f, 0);

    public XRUI_Helper.UnityBooleanEvent onToggleChange;
    public UnityEvent onToggleSelect;
    public UnityEvent onToggleUnselect;

    private Vector3 worldUnselectPos;
    private Vector3 worldSelectPos;

    private void SetToggleValue(bool value)
    {
        if (isSelected == value) return;

        isSelected = value;
        SetTogglePosition();
        onToggleChange?.Invoke(value);
        if (IsSelected)
            onToggleSelect?.Invoke();
        else
            onToggleUnselect?.Invoke();

        UpdateColors();
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        if (selectObject != null)
        {
            worldUnselectPos = bodyObject.transform.TransformVector(selectPos) + bodyObject.transform.position;
            worldSelectPos = bodyObject.transform.TransformVector(unselectPos) + bodyObject.transform.position;
        }

        SetRenderers();
        UpdateColors();

        Vector3 newPos = isSelected ? selectPos : unselectPos;
        selectObject.transform.localPosition = newPos;
    }

    protected override void Awake()
    {
        base.Awake();

        SetRenderers();
        UpdateColors();

        if (xrFeedback.XRInteractable != null)
            xrFeedback.XRInteractable.onSelectEnter.AddListener(
                (XRBaseInteractor) =>
                {
                    if (!isEnabled)
                        return;

                    SetToggleValue(!isSelected);
                }
            );
    }

    private void SetTogglePosition()
    {
        if (selectObject == null)
            return;

        StopAllCoroutines();
        StartCoroutine(SetToggleCoroutine());
    }

    IEnumerator SetToggleCoroutine()
    {
        float t = 0;
        Vector3 newPos = isSelected ? selectPos : unselectPos;

        while (t < 1)
        {
            t += Time.deltaTime / animationSpeed;
            selectObject.transform.localPosition = Vector3.Lerp(selectObject.transform.localPosition, newPos, t);
            yield return null;
        }
        if (changeBGColor)
        {
            yield return new WaitForSeconds(0.3f);
            UpdateColors();
        }
    }

    protected abstract void SetRenderers();
    protected abstract void UpdateColors();

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
            return;
#endif
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(worldUnselectPos, 0.01f);
        Gizmos.DrawLine(worldUnselectPos, worldSelectPos);
        Gizmos.DrawWireSphere(worldSelectPos, 0.01f);
    }
}