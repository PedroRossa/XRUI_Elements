using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class XRMeshFeedback : XRBaseFeedback
{
    private MeshRenderer elementMesh;


    protected override void Awake()
    {
        base.Awake();

        elementMesh = GetComponent<MeshRenderer>();

        if (elementMesh.sharedMaterial == null)
            elementMesh.sharedMaterial = new Material(Shader.Find("Unlit/TransparentColor"));
        else //Copy the material to don't change every meshFeedbacks
            elementMesh.sharedMaterial = new Material(elementMesh.sharedMaterial);

        originalColor = elementMesh.sharedMaterial.color;
    }

    protected override void SetColor(Color color)
    {
        elementMesh.sharedMaterial.color = color;
    }
}
