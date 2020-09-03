using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Class used to manage the interactions used in hands
/// </summary>
public class HandInteractionsManager : MonoBehaviour
{   /// <summary>
    /// A memory variable to keep a destroyed line visual 
    /// </summary>
    private XRInteractorLineVisual destroyedLine;

    /// <summary>
    /// Possible commands to do in next frame enum
    /// </summary>
    public enum nextFrameCommands
    {
        nothing = 0,
        addRay = 1,
        addDirect = 2
    };

    /// <summary>
    /// Possible commands to do in next frame enum instance
    /// </summary>
    nextFrameCommands machineStates;
    /// <summary>
    /// Selected xr grab interactables quantity
    /// </summary>
    public static short selectedObjetcs;

    /// <summary>
    /// Main setup
    /// </summary>
    private void Start()
    {
        eventConfiguration();
    }
    /// <summary>
    /// Function to swap both hands xr base interactors
    /// </summary>
    void SwapInteraction()
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
                case nextFrameCommands.nothing:
                    break;

                case nextFrameCommands.addRay:
                    addRayAndLineVisual();                
                    break;

                default:
                    addDirect();
                    break;

            }
            machineStates = nextFrameCommands.nothing;
            eventConfiguration();
        }
        
        if (Input.GetKeyDown(OculusInput.ButtonA) && selectedObjetcs == 0) {
            SwapInteraction();
        }
    }

    /// <summary>
    /// Callbacks setup
    /// </summary>
    private void eventConfiguration()
    {
        GetComponent<XRBaseInteractor>().onSelectEnter.AddListener(i =>
        {
            ++selectedObjetcs;
        });

        GetComponent<XRBaseInteractor>().onSelectExit.AddListener(i =>
        {
            --selectedObjetcs;
        });
    }

    /// <summary>
    /// Add a ray interactor and a line visual
    /// </summary>
    private void addRayAndLineVisual()
    {
        gameObject.AddComponent<XRRayInteractor>();

        gameObject.AddComponent<XRInteractorLineVisual>();
        XRInteractorLineVisual line = gameObject.GetComponent<RayAndDirect>().raySo.line;

        XRInteractorLineVisual goLine = gameObject.GetComponent<XRInteractorLineVisual>();
        goLine.validColorGradient = line.validColorGradient;
        goLine.invalidColorGradient = line.invalidColorGradient;
        goLine.smoothMovement = line.smoothMovement;

        GetComponent<SphereCollider>().enabled = false;
    }

    /// <summary>
    /// Add a direct interactor
    /// </summary>
    private void addDirect()
    {
        gameObject.AddComponent<XRDirectInteractor>();

        GetComponent<SphereCollider>().enabled = true;
    }
}
