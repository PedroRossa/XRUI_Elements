using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// ONLY TEST CLASS... MY FIRST PLAY IN VR - GUILHERME
/// </summary>
public class ChangeColor : MonoBehaviour
{
    XRGrabInteractable grabInteractable;
    Color initialColor;
    Material rendererMaterial;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.onActivate.AddListener(SetColor);

        grabInteractable.onHoverExit.AddListener(ResetColor);

        initialColor = GetComponent<MeshRenderer>().material.color;

        rendererMaterial = GetComponent<MeshRenderer>().material;
    }

    private void OnDestroy()
    {
        grabInteractable.onActivate.RemoveListener(SetColor);
    }

    public void SetColor(XRBaseInteractor interactor)
    {
        /*if (interactor.GetComponent<XRController>().controllerNode != UnityEngine.XR.XRNode.RightHand)
            return;*/

        int rnd;

        if(interactor.GetComponent<XRController>().controllerNode == UnityEngine.XR.XRNode.RightHand)
        {
            if (rendererMaterial.color.Equals(Color.green))
            {
                rnd = new System.Random().Next(1, 4);
            }
            else
            {
                System.Collections.Generic.List<int> lb = new System.Collections.Generic.List<int> { 0, 1, 2, 3, 4 };
                if (rendererMaterial.color.Equals(Color.yellow))
                {
                    lb.Remove(lb[1]);
                    rnd = lb[new System.Random().Next(4)];
                }
                else if (rendererMaterial.color.Equals(Color.blue))
                {
                    lb.Remove(lb[2]);
                    rnd = lb[new System.Random().Next(4)];
                }
                else if (rendererMaterial.color.Equals(Color.red))
                {
                    lb.Remove(lb[3]);
                    rnd = lb[new System.Random().Next(4)];
                }
                else
                {
                    lb.Remove(lb[4]);
                    rnd = lb[new System.Random().Next(4)];
                }
            }

            switch (rnd)
            {
                case 0:
                    rendererMaterial.color = Color.green;
                    break;
                case 1:
                    rendererMaterial.color = Color.yellow;
                    break;
                case 2:
                    rendererMaterial.color = Color.blue;
                    break;
                case 3:
                    rendererMaterial.color = Color.red;
                    break;
                default:
                    rendererMaterial.color = Color.black;
                    break;
            }
        }
        else
        {
            if (rendererMaterial.color.Equals(Color.cyan))
            {
                rnd = new System.Random().Next(1, 4);
            }
            else
            {
                System.Collections.Generic.List<int> lb = new System.Collections.Generic.List<int> { 0, 1, 2, 3, 4 };
                if (rendererMaterial.color.Equals(Color.gray))
                {
                    lb.Remove(lb[1]);
                    rnd = lb[new System.Random().Next(4)];
                }
                else if (rendererMaterial.color.Equals(Color.magenta))
                {
                    lb.Remove(lb[2]);
                    rnd = lb[new System.Random().Next(4)];
                }
                else if (rendererMaterial.color.Equals(Color.clear))
                {
                    lb.Remove(lb[3]);
                    rnd = lb[new System.Random().Next(4)];
                }
                else
                {
                    lb.Remove(lb[4]);
                    rnd = lb[new System.Random().Next(4)];
                }
            }

            switch (rnd)
            {
                case 0:
                    rendererMaterial.color = Color.cyan;
                    break;
                case 1:
                    rendererMaterial.color = Color.gray;
                    break;
                case 2:
                    rendererMaterial.color = Color.magenta;
                    break;
                case 3:
                    rendererMaterial.color = Color.clear;
                    break;
                default:
                    rendererMaterial.color = new Color(0.7f,0.2f,0.5f);
                    break;
            }
        }
    }

    public void ResetColor(XRBaseInteractor interactor)
    {
        rendererMaterial.color = initialColor;
    }
}
