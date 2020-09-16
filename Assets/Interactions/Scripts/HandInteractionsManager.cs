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
    /// The objects' name that spam when swap interaction
    /// </summary>
    public string[] spamObjectsName;


    /// <summary>
    /// Main setup
    /// </summary>
    private void Start()
    {
        eventConfiguration();
        machineStates = nextFrameCommands.nothing;
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
                case nextFrameCommands.addDirect:
                    addDirect();
                    break;

                case nextFrameCommands.addRay:
                    addRayAndLineVisual();                
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
        var prefab = gameObject.GetComponent<RayAndDirect>().cloneRay();
        var ray = gameObject.AddComponent<XRRayInteractor>();
        ray = prefab.ray;

        var line = gameObject.AddComponent<XRInteractorLineVisual>();
        line = prefab.line;

        GetComponent<SphereCollider>().enabled = false;

        foreach(var spam in spamObjectsName) {
            Destroy(GameObject.Find(spam));
        }
    }

    /// <summary>
    /// Add a direct interactor
    /// </summary>
    private void addDirect()
    {
        var prefab = gameObject.GetComponent<RayAndDirect>().cloneDirect();
        var direct = gameObject.AddComponent<XRDirectInteractor>();
        direct = prefab.direct;

        GetComponent<SphereCollider>().enabled = true;
    }
}
