using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Class that permits the swap from a XRDirectInteractor in to a XRRayInteractor of a hand
/// </summary>
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
    /// The current transform where ray is attached
    /// </summary>
    private Transform currAttachTransform = null;

    /// <summary>
    /// The instance of nextFrameCommands enum
    /// </summary>
    private nextFrameCommands machineStates;
    /// <summary>
    /// Is the hand selecting an object?
    /// </summary>
    private bool isSelectingAnObject;

    /// <summary>
    /// The objects' name that spam when swap interaction
    /// </summary>
    public string[] spamObjectsName;

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
            Destroy(ray.attachTransform.gameObject);
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
                    foreach (var spam in spamObjectsName)
                    {
                        Destroy(GameObject.Find(spam));
                    }

                    break;
            }


            machineStates = nextFrameCommands.nothing;
            eventConfiguration();

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
        GetComponent<XRBaseInteractor>().onSelectEnter.AddListener(i =>
        {
            isSelectingAnObject = true;
        });

        GetComponent<XRBaseInteractor>().onSelectExit.AddListener(i =>
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

        if (currAttachTransform != null)
            Destroy(currAttachTransform.gameObject);
        currAttachTransform = ray.attachTransform;
        if (attachRayTransform != null)
        {
            currAttachTransform.position = attachRayTransform.position;
            currAttachTransform.rotation = attachRayTransform.rotation;
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
