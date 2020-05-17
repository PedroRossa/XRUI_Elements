using TMPro;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    private GameObject generatedCubes;

    public Color[] cubeColors = { Color.white, Color.red, Color.blue };
    public Texture2D cubeTexture;

    private void Awake()
    {
        generatedCubes = new GameObject("GeneratedCubes");
        generatedCubes.transform.SetParent(transform);
    }

    public void GenerateCube(int colorId)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.SetParent(generatedCubes.transform);
        cube.transform.position = transform.position;
        cube.transform.localScale = transform.localScale;
        cube.AddComponent<Rigidbody>();

        MeshRenderer mr = cube.GetComponent<MeshRenderer>();
        mr.sharedMaterial = new Material(mr.sharedMaterial);

        if (colorId >= cubeColors.Length || colorId < 0)
            mr.sharedMaterial.color = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), 1);
        else
            mr.sharedMaterial.color = cubeColors[colorId];
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
            MeshRenderer mr = generatedCubes.transform.GetChild(i).GetComponent<MeshRenderer>();
            mr.sharedMaterial.color = cubeColors[colorId];
        }
    }
}
