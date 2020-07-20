using NaughtyAttributes;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class XRUI_Feedback : MonoBehaviour
{
    public bool isEnabled = true;

    public bool useNearEvents = false;
    public bool useTouchEvents = false;
    [Tooltip("To use the Select Events, it's necessary to have a XRInteractable attached to gameobject.")]
    public bool useSelectEvents = false;
    public bool allowDistanceEvents = false;
    #region Unity Events

    [BoxGroup("NearEvents")]
    [ShowIf("useNearEvents")]
    public XRUI_Helper.UnityXRControllerEvent onNearEnter = new XRUI_Helper.UnityXRControllerEvent();
    [BoxGroup("NearEvents")]
    [ShowIf("useNearEvents")]
    public XRUI_Helper.UnityXRControllerEvent onNearExit = new XRUI_Helper.UnityXRControllerEvent();

    [BoxGroup("TouchEvents")]
    [ShowIf("useTouchEvents")]
    public XRUI_Helper.UnityXRControllerEvent onTouchEnter = new XRUI_Helper.UnityXRControllerEvent();
    [BoxGroup("TouchEvents")]
    [ShowIf("useTouchEvents")]
    public XRUI_Helper.UnityXRControllerEvent onTouchExit = new XRUI_Helper.UnityXRControllerEvent();

    [BoxGroup("SelectEvents")]
    [ShowIf("useSelectEvents")]
    public XRUI_Helper.UnityXRControllerEvent onSelectEnter = new XRUI_Helper.UnityXRControllerEvent();
    [BoxGroup("SelectEvents")]
    [ShowIf("useSelectEvents")]
    public XRUI_Helper.UnityXRControllerEvent onSelectExit = new XRUI_Helper.UnityXRControllerEvent();

    #endregion

    public enum NearColliderType
    {
        Box = 0,
        Sphere = 1
    }

    private bool IsNearBoxCollider() { return nearColliderType == NearColliderType.Box ? true : false; }
    private bool IsNearSphereCollider() { return nearColliderType == NearColliderType.Sphere ? true : false; }

    [Header("Near Collider")]
    public NearColliderType nearColliderType;

    [ShowIf("IsNearBoxCollider")]
    public Vector3 nearColliderScale = Vector3.one * 1.5f;

    [ShowIf("IsNearSphereCollider")]
    [Range(0.01f, 50.0f)]
    public float nearColliderRadius = 1.5f;

    public Vector3 nearColliderOffset = Vector3.zero;

    private Vector3 worldNearColliderOffset;
    private Collider nearCollider;
    private XRUI_NearDetectionLocked.XRUI_NearDetection xrNearDetection;
    private XRBaseInteractable xrInteractable;

    private bool isTouching;
    private bool isSelected;

    public bool IsNear { get => xrNearDetection.IsNear; }
    public bool IsTouching { get => isTouching; }
    public bool IsSelected { get => isSelected; }

    private void OnValidate()
    {
        worldNearColliderOffset = transform.TransformDirection(nearColliderOffset) + transform.position;
        ManageEvents();

        //Check if the uicolor is used with a Feedback Color script. If it's, refresh the normal color on target
        XRUI_FeedbackColor feedbackColor = GetComponentInChildren<XRUI_FeedbackColor>();
        if (feedbackColor != null)
            feedbackColor.RefreshElementColor();
    }

    protected virtual void Awake()
    {
        CreateNearCollider();

        xrInteractable = transform.GetComponent<XRBaseInteractable>();

        if (xrInteractable != null)
            ConfigureSelectListeners();

    }

    private void ManageEvents()
    {
        if (!useNearEvents)
        {
            onNearEnter = new XRUI_Helper.UnityXRControllerEvent();
            onNearExit = new XRUI_Helper.UnityXRControllerEvent();
        }

        if (!useTouchEvents)
        {
            onTouchEnter = new XRUI_Helper.UnityXRControllerEvent();
            onTouchExit = new XRUI_Helper.UnityXRControllerEvent();
        }

        if (!useSelectEvents)
        {
            onSelectEnter = new XRUI_Helper.UnityXRControllerEvent();
            onSelectExit = new XRUI_Helper.UnityXRControllerEvent();
        }
    }

    private void CreateNearCollider()
    {
        GameObject nearColliderObject = new GameObject("NearColliderObject");
        nearColliderObject.transform.SetParent(transform);

        nearColliderObject.transform.localPosition = Vector3.zero;
        nearColliderObject.transform.localRotation = Quaternion.identity;
        /// its fix scale on rotation if some axis is diffent 

        switch (nearColliderType)
        {
            case NearColliderType.Box:
                {

                    nearColliderObject.transform.localScale = new Vector3(1f / transform.localScale.x, 1f / transform.localScale.y, 1f / transform.localScale.z);
                    Vector3 boxScale = transform.localScale;
                    boxScale.Scale(nearColliderScale);

                    nearCollider = nearColliderObject.AddComponent<BoxCollider>();
                    ((BoxCollider)nearCollider).center = new Vector3(nearColliderOffset.x * transform.localScale.x, nearColliderOffset.y * transform.localScale.y, nearColliderOffset.z * transform.localScale.z);
                    ((BoxCollider)nearCollider).size = boxScale;
                }
                break;
            case NearColliderType.Sphere:
                {
                    nearCollider = nearColliderObject.AddComponent<SphereCollider>();
                    ((SphereCollider)nearCollider).center = nearColliderOffset;
                    ((SphereCollider)nearCollider).radius = nearColliderRadius * transform.localScale.x;
                    break;
                }
            default:
                throw new Exception("Unknow NearColliderType selected");
        }
        nearCollider.isTrigger = true;
        xrNearDetection = nearColliderObject.AddComponent<XRUI_NearDetectionLocked.XRUI_NearDetection>();

        if (allowDistanceEvents)
        {
            XRBaseInteractable interactable = gameObject.GetComponent<XRBaseInteractable>();
            if (interactable == null)
                interactable = gameObject.GetComponentInChildren<XRBaseInteractable>();

            if (interactable == null)
                interactable = gameObject.AddComponent<XRSimpleInteractable>();

            if (interactable != null)
            {
                interactable.onHoverEnter.AddListener((XRBaseInteractor) => { xrNearDetection.OnEnterAction(XRBaseInteractor); });
                interactable.onHoverExit.AddListener((XRBaseInteractor) => { xrNearDetection.OnExitAction(XRBaseInteractor); });
            }
        }
    }

    private void ConfigureSelectListeners()
    {
        xrInteractable.onSelectEnter.AddListener((XRBaseInteractor interactor) =>
        {
            OnEnterInteraction(interactor);
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
    private void OnEnterInteraction(XRBaseInteractor interactor)
    {
        XRController controller = interactor.GetComponent<XRController>();
        if (controller != null)
        {
            isSelected = true;
            onSelectEnter?.Invoke(controller);
            //nearCollider.gameObject.SetActive(false);
            //GetComponent<Collider>().enabled = false;
            XRUI_3DButtonBase button3d = GetComponentInChildren<XRUI_3DButtonBase>();
            if (button3d == null)
                button3d = GetComponentInParent<XRUI_3DButtonBase>();

            if (button3d != null)
                button3d.SimulateClick();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //if the near trigger is not activated yet, isn't the touch trigger enter yet
        if (!xrNearDetection.IsNear)
            return;

        XRController controller = other.GetComponent<XRController>();

        if (controller == null)
            controller = other.GetComponentInParent<XRController>();

        if (controller != null && !isTouching)
        {
            isTouching = true;
            onTouchEnter?.Invoke(controller);
        }
    }

    void OnTriggerExit(Collider other)
    {
        XRController controller = other.GetComponent<XRController>();
        if (controller == null)
            controller = other.GetComponentInParent<XRController>();

        if (controller != null && isTouching)
        {
            isTouching = false;
            onTouchExit?.Invoke(controller);
        }
    }


    private void OnDrawGizmosSelected()
    {
        //#if UNITY_EDITOR
        //        if (EditorApplication.isPlaying)
        //            return;
        //#endif

        Gizmos.color = Color.red;

        switch (nearColliderType)
        {
            case NearColliderType.Box:

                Vector3 boxScale = transform.localScale;
                //scale for all parents 
                boxScale = ScaleOfParents(transform.parent, boxScale);
                boxScale.Scale(nearColliderScale);
                //its set a limit of bounds of gizmos on center of object... buts its not happens on collider without fix some issues
                //Vector3 nnearOff = new Vector3(nearColliderOffset.x != 0 ? (nearColliderOffset.x / (2f * (Mathf.Abs(nearColliderOffset.x) / boxScale.x))) : 0,
                //                               nearColliderOffset.y != 0 ? (nearColliderOffset.y / (2f * (Mathf.Abs(nearColliderOffset.y) / boxScale.y))) : 0,
                //                               nearColliderOffset.z != 0 ? (nearColliderOffset.z / (2f * (Mathf.Abs(nearColliderOffset.z) / boxScale.z))) : 0);
                Vector3 nnearOff = new Vector3((nearColliderOffset.x * boxScale.x) / nearColliderScale.x, (nearColliderOffset.y * boxScale.y) / nearColliderScale.y, (nearColliderOffset.z * boxScale.z) / nearColliderScale.z);
                Vector3 nworldNearColliderOffset = transform.TransformDirection(nnearOff) + transform.position;

                Matrix4x4 rotationMatrix = Matrix4x4.TRS(nworldNearColliderOffset, transform.rotation, boxScale);
                Gizmos.matrix = rotationMatrix;
                Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

                break;
            case NearColliderType.Sphere:

                Gizmos.DrawWireSphere(worldNearColliderOffset, transform.localScale.x * nearColliderRadius);
                break;
            default:
                break;
        }
    }

    private Vector3 ScaleOfParents(Transform parent, Vector3 scale)
    {

        Vector3 scaleOfParents = scale;
        if (parent != null)
        {
            scaleOfParents.Scale(parent.localScale);
            scaleOfParents = ScaleOfParents(parent.parent, scaleOfParents);
        }
        return scaleOfParents;
    }
}
