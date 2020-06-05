using NaughtyAttributes;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSlider : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject lineObject;
    public GameObject sliderObject;
    public GameObject textObject;
    public Transform sliderInit;
    public Transform sliderEnd;

    [Header("Prefab Color Properties")]
    public Color lineColor = Color.white;
    public Color32 sliderObjectColor = new Color32(27, 140, 175, 255);

    [Header("General Properties")]

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float sliderValue = 0.75f;
    public float SliderValue
    {
        get => sliderValue;
        set => SetSliderValue(value);
    }

    public bool canPush = true;
    public bool showText = true;

    [ReadOnly]
    public bool isHover = false;
    [ReadOnly]
    public bool isSelected = false;

    public XRUIElements_Helper.UnityFloatEvent onSliderChange;

    private Rigidbody interactableRigidbody;

    private TextMeshPro tmpSliderText;
    private Vector3 initPos;
    private Vector3 endPos;

    private void SetSliderValue(float value)
    {
        if (sliderValue == value) return;

        //limit the value between 0 and 1
        sliderValue = Mathf.Clamp01(value);

        UpdateSliderPosition();
        UpdateSliderText();
        onSliderChange?.Invoke(sliderValue);
    }

    private void ConfigureInteractable()
    {
        XRBaseInteractable interactable = sliderObject.GetComponentInChildren<XRBaseInteractable>();

        if (interactable == null)
            throw new SystemException("The slider object needs a XRInteractable to work.");

        interactableRigidbody = interactable.GetComponent<Rigidbody>();

        if (!canPush)
            interactableRigidbody.isKinematic = true;

        //Configure select listeners
        interactable.onSelectEnter.RemoveAllListeners();
        interactable.onSelectEnter.AddListener((XRBaseInteractor) => { isSelected = true; });

        interactable.onSelectExit.RemoveAllListeners();
        interactable.onSelectExit.AddListener((XRBaseInteractor) =>
        {
            isSelected = false;
            interactableRigidbody.velocity = Vector3.zero;
        });

        //Configure hover listeners
        interactable.onFirstHoverEnter.RemoveAllListeners();
        interactable.onFirstHoverEnter.AddListener((XRBaseInteractor) =>
        {
            isHover = true;
            if (canPush)
                interactableRigidbody.isKinematic = false;
        });

        interactable.onLastHoverExit.RemoveAllListeners();
        interactable.onLastHoverExit.AddListener((XRBaseInteractor) =>
        {
            isHover = false;
            interactableRigidbody.isKinematic = true;
            interactableRigidbody.velocity = Vector3.zero;
        });
    }

    private void RefreshConfigurations()
    {
        if (lineObject != null)
            lineObject.GetComponent<MeshRenderer>().sharedMaterial.color = lineColor;

        if (sliderObject != null)
            sliderObject.GetComponent<MeshRenderer>().sharedMaterial.color = sliderObjectColor;

        if (textObject != null)
        {
            if (tmpSliderText == null)
                tmpSliderText = textObject.GetComponentInChildren<TextMeshPro>();

            textObject.SetActive(showText);
        }

        if (sliderInit == null || sliderEnd == null)
            throw new SystemException("It's necessary set slider init and end references.");

        initPos = sliderInit.transform.localPosition;
        endPos = sliderEnd.transform.localPosition;
    }


    private void OnValidate()
    {
        RefreshConfigurations();
        UpdateSliderPosition();
        UpdateSliderText();
    }

    private void Awake()
    {
        ConfigureInteractable();
        RefreshConfigurations();
        UpdateSliderPosition();
    }

    private void Update()
    {
        if (isHover || isSelected)
        {
            UpdateSliderByMovement();
            ConstraintLocally();
        }
    }

    private void UpdateSliderByMovement()
    {
        if (isSelected)
        {
            if (sliderObject.transform.parent == null)
                sliderObject.transform.SetParent(transform);
        }

        SliderValue = NormalizedSliderPosition();
    }

    private float NormalizedSliderPosition()
    {
        if (sliderObject == null)
            throw new SystemException("The component don't have the sliderObject set. It's necessary this reference to calculate the slider value.");

        //Get x position that represents the current local slider object position
        float posX = sliderObject.transform.localPosition.x;

        sliderObject.transform.localPosition = new Vector3(posX, 0, 0);

        float normalized = (posX - initPos.x) / (endPos.x - initPos.x);
        return normalized;
    }

    private void ConstraintLocally()
    {
        Vector3 localVelocity = sliderObject.transform.InverseTransformDirection(interactableRigidbody.velocity);
        localVelocity.y = 0;
        localVelocity.z = 0;

        interactableRigidbody.velocity = sliderObject.transform.TransformDirection(localVelocity);
    }

    private void UpdateSliderPosition()
    {
        if (sliderObject == null)
            return;

        //n1 + ((n2 - n1) * percent)
        Vector3 pos = initPos + ((endPos - initPos) * SliderValue);

        sliderObject.transform.localPosition = pos;
    }

    private void UpdateSliderText()
    {
        if (tmpSliderText != null)
            tmpSliderText.text = SliderValue.ToString("f2");
    }
}
