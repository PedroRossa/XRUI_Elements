using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Class that permits the swap from a XRDirectInteractor in to a XRRayInteractor of a hand
/// </summary>
[RequireComponent(typeof(RayAndDirect))]
public class HandInteractionsManager : MonoBehaviour
{
    /// <summary>
    /// Is the object the left hand?
    /// </summary>
    public bool isLeftHand;
    /// <summary>
    /// Enum of possible commands to execute in the next frame
    /// </summary>
    public enum nextFrameCommands
    {
        nothing = 0,
        addRay = 1,
        addDirect = 2
    };

    /// <summary>
    /// The point transform to attach the ray
    /// </summary>
    public Transform attachRayTransform;
    /// <summary>
    /// The objects' name that spam when swap interaction
    /// </summary>
    public string[] spamObjectsName;

    /// <summary>
    /// The instance of nextFrameCommands enum
    /// </summary>
    private nextFrameCommands machineStates;
    /// <summary>
    /// Is the hand selecting an object?
    /// </summary>
    private bool isSelectingAnObject;

    private void Start()
    {
        eventConfiguration();
    }

    /// <summary>
    /// The setup to swap hand interaction type. Destroy current components of XRBaseInteractor
    /// </summary>
    private void SwapInteraction()
    {
        XRRayInteractor ray = gameObject.GetComponent<XRRayInteractor>();

        if (ray != null)
        {
            Destroy(ray);
            Destroy(gameObject.GetComponent<XRInteractorLineVisual>());
            machineStates = nextFrameCommands.addDirect;
        }

        else
        {
            Destroy(gameObject.GetComponent<XRDirectInteractor>());
            machineStates = nextFrameCommands.addRay;
        }
    }

    void LateUpdate()
    {

        if (machineStates != nextFrameCommands.nothing)
        {
            switch (machineStates)
            {
                case nextFrameCommands.addDirect:
                    addDirect();
                    break;

                case nextFrameCommands.addRay:
                    addRay();
                    break;
            }

            machineStates = nextFrameCommands.nothing;
            eventConfiguration();

            foreach (var spam in spamObjectsName)
            {
                Destroy(GameObject.Find(spam));
            }
        }

        if (isLeftHand)
        {
            if (Input.GetKeyDown(OculusInput.ButtonX) && !isSelectingAnObject)
            {
                SwapInteraction();
            }
        }
        else
        {
            if (Input.GetKeyDown(OculusInput.ButtonA) && !isSelectingAnObject)
            {
                SwapInteraction();
            }
        }
    }

    /// <summary>
    /// Callbacks configuration to XRBaseInteractors
    /// </summary>
    private void eventConfiguration()
    {
        GetComponent<XRBaseInteractor>().onSelectEnter.AddListener(interactable =>
        {
            isSelectingAnObject = true;
        });

        GetComponent<XRBaseInteractor>().onSelectExit.AddListener(interactable =>
        {
            isSelectingAnObject = false;
        });
    }

    /// <summary>
    /// Setup to add a XRRayInteraction and a XRLineInteraction component
    /// </summary>
    private void addRay()
    {
        XRRayInteractor ray = gameObject.AddComponent<XRRayInteractor>();
        gameObject.AddComponent<XRInteractorLineVisual>();
        XRInteractorLineVisual line = gameObject.GetComponent<RayAndDirect>().raySo.line;
        XRInteractorLineVisual goLine = gameObject.GetComponent<XRInteractorLineVisual>();
        goLine.validColorGradient = line.validColorGradient;
        goLine.invalidColorGradient = line.invalidColorGradient;
        goLine.smoothMovement = line.smoothMovement;
        GetComponent<SphereCollider>().enabled = false;

        if (attachRayTransform != null)
        {
            ray.attachTransform.position = attachRayTransform.position;
            ray.attachTransform.rotation = attachRayTransform.rotation;
        }
    }

    /// <summary>
    /// Setup to add a XRDirectInteraction component
    /// </summary>
    private void addDirect()
    {
        gameObject.AddComponent<XRDirectInteractor>();

        GetComponent<SphereCollider>().enabled = true;
    }
}
