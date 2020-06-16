using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class XRToggle : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject toggleObject;
    public GameObject toggleBody;

    [Header("Prefab Color Properties")]
    public Color bodyColor = new Color(1, 1, 1, 0.25f);
    public Color selectedColor = Color.green;
    public Color unselectedColor = Color.gray;


    [Header("General Properties")]
    public float animationSpeed = 0.1f;

    [SerializeField]
    private bool isSelected;
    public bool IsSelected
    {
        get => isSelected;
        set => SetToggleValue(value);
    }

    public XRUIElements_Helper.UnityBooleanEvent onToggleChange;
    public UnityEvent onToggleSelect;
    public UnityEvent onToggleUnselect;

    private Vector3 unSelectPos = new Vector3(0, 0.5f, 0);
    private Vector3 selectPos = new Vector3(0, -0.5f, 0);


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
    }

    private void OnValidate()
    {
        if (toggleBody != null)
            toggleBody.GetComponent<MeshRenderer>().sharedMaterial.color = bodyColor;
        if (toggleObject != null)
            toggleObject.GetComponent<MeshRenderer>().sharedMaterial.color = isSelected ? selectedColor : unselectedColor;


        Vector3 newPos = isSelected ? selectPos : unSelectPos;
        toggleObject.transform.localPosition = newPos;
    }

    [Button]
    public void Toggle()
    {
        IsSelected = !IsSelected;
    }

    private void Awake()
    {
        XRBaseInteractable interactable = gameObject.GetComponentInChildren<XRBaseInteractable>();
        interactable.onSelectEnter.AddListener((XRBaseInteractor) => { SetToggleValue(!isSelected); });
    }

    private void SetTogglePosition()
    {
        if (toggleObject == null)
            return;

        StopAllCoroutines();
        StartCoroutine(SetToggleCoroutine());
    }

    IEnumerator SetToggleCoroutine()
    {
        float t = 0;
        Vector3 newPos = isSelected ? selectPos : unSelectPos;
        MeshRenderer toggleMeshRenderer = toggleObject.GetComponent<MeshRenderer>();

        while (t < 1)
        {
            t += Time.deltaTime / animationSpeed;
            toggleObject.transform.localPosition = Vector3.Lerp(toggleObject.transform.localPosition, newPos, t);

            //Lerp color. For some reason the speed to execute this lerp need's to be smaller
            if (isSelected)
                toggleMeshRenderer.sharedMaterial.color = Color.Lerp(unselectedColor, selectedColor, t * 10);
            else
                toggleMeshRenderer.sharedMaterial.color = Color.Lerp(selectedColor, unselectedColor, t * 10);

            yield return null;
        }
    }
}
