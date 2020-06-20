using NaughtyAttributes;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class XRFeedback : MonoBehaviour
{
    public bool useNearEvents = false;
    public bool useTouchEvents = false;
    [Tooltip("To use the Select Events, it's necessary to have a XRInteractable attached to gameobject.")]
    public bool useSelectEvents = false;  //TODO: see how to inform that to use select is necessary have a XRInteractable

    #region Unity Events

    [BoxGroup("NearEvents")]
    [ShowIf("useNearEvents")]
    public XRUIElements_Helper.UnityXRControllerEvent onNearEnter = new XRUIElements_Helper.UnityXRControllerEvent();
    [BoxGroup("NearEvents")]
    [ShowIf("useNearEvents")]
    public XRUIElements_Helper.UnityXRControllerEvent onNearExit = new XRUIElements_Helper.UnityXRControllerEvent();

    [BoxGroup("TouchEvents")]
    [ShowIf("useTouchEvents")]
    public XRUIElements_Helper.UnityXRControllerEvent onTouchEnter = new XRUIElements_Helper.UnityXRControllerEvent();
    [BoxGroup("TouchEvents")]
    [ShowIf("useTouchEvents")]
    public XRUIElements_Helper.UnityXRControllerEvent onTouchExit = new XRUIElements_Helper.UnityXRControllerEvent();

    [BoxGroup("SelectEvents")]
    [ShowIf("useSelectEvents")]
    public XRUIElements_Helper.UnityXRControllerEvent onSelectEnter = new XRUIElements_Helper.UnityXRControllerEvent();
    [BoxGroup("SelectEvents")]
    [ShowIf("useSelectEvents")]
    public XRUIElements_Helper.UnityXRControllerEvent onSelectExit = new XRUIElements_Helper.UnityXRControllerEvent();

    #endregion

    protected enum NearColliderType
    {
        Box = 0,
        Sphere = 1
    }

    private bool IsNearBoxCollider() { return nearColliderType == NearColliderType.Box ? true : false; }
    private bool IsNearSphereCollider() { return nearColliderType == NearColliderType.Sphere ? true : false; }

    [Header("Near Collider")]
    [SerializeField]
    private NearColliderType nearColliderType;

    [ShowIf("IsNearBoxCollider")]
    public Vector3 nearColliderScale = Vector3.one * 1.5f;

    [ShowIf("IsNearSphereCollider")]
    [Range(0.01f, 50.0f)]
    public float nearColliderRadius = 1.5f;

    public Vector3 nearColliderOffset = Vector3.zero;

    private Vector3 worldNearColliderOffset;
    private Collider nearCollider;
    private XRNearDetectionLocked.XRNearDetection xrNearDetection;
    private XRBaseInteractable xrInteractable;

    private bool isTouching;
    private bool isSelected;

    private void OnValidate()
    {
        worldNearColliderOffset = transform.TransformDirection(nearColliderOffset) + transform.position;
        ManageEvents();
    }

    protected virtual void Awake()
    {
        CreateNearCollider();

        xrInteractable = GetComponent<XRBaseInteractable>();

        if (xrInteractable != null)
            ConfigureSelectListeners();
    }

    private void ManageEvents()
    {
        if (!useNearEvents)
        {
            onNearEnter = new XRUIElements_Helper.UnityXRControllerEvent();
            onNearExit = new XRUIElements_Helper.UnityXRControllerEvent();
        }

        if (!useTouchEvents)
        {
            onTouchEnter = new XRUIElements_Helper.UnityXRControllerEvent();
            onTouchExit = new XRUIElements_Helper.UnityXRControllerEvent();
        }

        if (!useSelectEvents)
        {
            onSelectEnter = new XRUIElements_Helper.UnityXRControllerEvent();
            onSelectExit = new XRUIElements_Helper.UnityXRControllerEvent();
        }
    }

    private void CreateNearCollider()
    {
        GameObject nearColliderObject = new GameObject("NearColliderObject");
        nearColliderObject.transform.SetParent(transform);
        nearColliderObject.transform.localPosition = Vector3.zero;
        nearColliderObject.transform.localScale = Vector3.one;
        nearColliderObject.transform.localRotation = Quaternion.identity;

        switch (nearColliderType)
        {
            case NearColliderType.Box:
                nearCollider = nearColliderObject.AddComponent<BoxCollider>();
                ((BoxCollider)nearCollider).center = nearColliderOffset;
                ((BoxCollider)nearCollider).size = nearColliderScale;
                break;
            case NearColliderType.Sphere:
                nearCollider = nearColliderObject.AddComponent<SphereCollider>();
                ((SphereCollider)nearCollider).center = nearColliderOffset;
                ((SphereCollider)nearCollider).radius = nearColliderRadius;
                break;
            default:
                throw new Exception("Unknow NearColliderType selected");
        }
        nearCollider.isTrigger = true;

        xrNearDetection = nearColliderObject.AddComponent<XRNearDetectionLocked.XRNearDetection>();
    }

    private void ConfigureSelectListeners()
    {
        xrInteractable.onSelectEnter.AddListener((XRBaseInteractor interactor) =>
        {
            XRController controller = interactor.GetComponent<XRController>();
            if (controller != null)
            {
                isSelected = true;
                nearCollider.gameObject.SetActive(false);
                GetComponent<Collider>().enabled = false;
                onSelectEnter?.Invoke(controller);
            }
        });

        xrInteractable.onSelectExit.AddListener((XRBaseInteractor interactor) =>
        {
            XRController controller = interactor.GetComponent<XRController>();
            if (controller != null)
            {
                isSelected = false;
                nearCollider.gameObject.SetActive(true);
                GetComponent<Collider>().enabled = true;
                onSelectExit?.Invoke(controller);
            }
        });
    }

    void OnTriggerEnter(Collider other)
    {
        //if the near trigger is not activated yet, isn't the touch trigger enter yet
        if (!xrNearDetection.IsNear)
            return;

        XRController controller = other.GetComponent<XRController>();
        if (controller != null && !isTouching)
        {
            isTouching = true;
            onTouchEnter?.Invoke(controller);
        }
    }

    void OnTriggerExit(Collider other)
    {
        XRController controller = other.GetComponent<XRController>();
        if (controller != null && isTouching)
        {
            isTouching = false;
            onTouchExit?.Invoke(controller);
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (EditorApplication.isPlaying)
            return;

        Gizmos.color = Color.red;

        switch (nearColliderType)
        {
            case NearColliderType.Box:
                Gizmos.DrawWireCube(worldNearColliderOffset, transform.localScale * nearColliderRadius);
                break;
            case NearColliderType.Sphere:
                Gizmos.DrawWireSphere(worldNearColliderOffset, transform.localScale.x * nearColliderRadius);
                break;
            default:
                break;
        }
    }
}
