using NaughtyAttributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CubeGenerator : MonoBehaviour
{
    private List<string> FeedbackType { get { return new List<string>() { "Outline", "Mesh Color" }; } }


    public Color[] cubeColors = { Color.white, Color.red, Color.blue };
    public Texture2D cubeTexture;

    [Dropdown("FeedbackType")]
    public string feedback;
    public Color feedbackColor = Color.magenta;
    public GameObject cubePrefab;

    private Transform generatedCubes;
    private XRBaseFeedback feedbackElement;

    private void Awake()
    {
        generatedCubes = new GameObject("GeneratedCubes").transform;
        generatedCubes.transform.SetParent(transform);
    }

    public void GenerateCube(int colorId)
    {
        GameObject cube = Instantiate(cubePrefab);
        cube.transform.SetParent(generatedCubes);
        GameObject content = cube.GetComponent<XRManipulable2D>().content.GetChild(0).gameObject;

        if (colorId >= cubeColors.Length || colorId < 0)
            content.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), 1);
        else
            content.GetComponent<MeshRenderer>().sharedMaterial.color = cubeColors[colorId];
       
        AddGrabAndFeedback(content);
    }

    private void AddGrabAndFeedback(GameObject content)
    {
        SphereCollider feedbackCollider = content.AddComponent<SphereCollider>();
        feedbackCollider.radius = 1.0f;
        feedbackCollider.isTrigger = true;

        content.AddComponent<XRGrabInteractable>();

        if (feedback.Equals("Outline"))
            feedbackElement = content.AddComponent<XROutlineFeedback>();
        else
            feedbackElement = content.AddComponent<XRMeshFeedback>();

        feedbackElement.proximityColor = feedbackColor;
        feedbackElement.proximityCollider = feedbackCollider;
    }

    public void ClearGeneratedCubes()
    {
        for (int i = 0; i < generatedCubes.transform.childCount; i++)
        {
            Destroy(generatedCubes.transform.GetChild(i).gameObject);
        }
    }

    public void ColorizeCubes(int colorId)
    {
        if (colorId >= cubeColors.Length || colorId < 0)
            return;

        for (int i = 0; i < generatedCubes.transform.childCount; i++)
        {
            GameObject go = generatedCubes.transform.GetChild(i).GetComponent<XRManipulable2D>().content.GetChild(0).gameObject;
            go.GetComponent<MeshRenderer>().sharedMaterial.color = cubeColors[colorId];
        }
    }

    public void SetFeedbackType(string type)
    {
        if (type.Equals("Outline") || type.Equals("Mesh Color"))
            feedback = type;
    }

    public void ToggleManipulable(bool state)
    {
        if (state)
        {
            for (int i = 0; i < generatedCubes.childCount; i++)
                generatedCubes.GetChild(i).GetComponent<XRManipulable2D>().SetInteractablesVisibility(state);
        }
    }
}
