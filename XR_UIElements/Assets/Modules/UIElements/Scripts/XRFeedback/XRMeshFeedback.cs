using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class XRMeshFeedback : XRBaseFeedback
{
    private MeshRenderer elementMesh;

    protected override void Awake()
    {
        base.Awake();

        elementMesh = GetComponent<MeshRenderer>();

        if (elementMesh.material == null)
            elementMesh.material = new Material(Shader.Find("Unlit/TransparentColor"));
        else //Copy the material to don't change every meshFeedbacks
            elementMesh.material = new Material(elementMesh.material);

        originalColor = elementMesh.material.color;
    }

    public override void SetColor(Color color)
    {
        if (elementMesh != null)
            elementMesh.material.color = color;
    }
}
