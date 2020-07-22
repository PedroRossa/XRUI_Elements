using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandInteractionsManager : MonoBehaviour
{
    private XRInteractorLineVisual destroyedLine;

    public enum nextFrameCommands
    {
        nothing = 0,
        addRay = 1,
        addDirect = 2
    };

    nextFrameCommands machineStates;
    public static short selectedObjetcs;

    private void Start()
    {
        eventConfiguration();
    }
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
                    addRay();                
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

    private void addRay()
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

    private void addDirect()
    {
        gameObject.AddComponent<XRDirectInteractor>();

        GetComponent<SphereCollider>().enabled = true;
    }
}
