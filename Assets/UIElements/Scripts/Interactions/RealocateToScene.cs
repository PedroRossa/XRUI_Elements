using System.Collections;
using UnityEngine;

public class RealocateToScene : MonoBehaviour
{
    public MeshRenderer meshLimits;

    private Vector3 initialPosition;
    private Vector3 limitPosition;
    private const int waitSeconds = 2;

    void Start()
    {
        initialPosition = transform.position;

        //Setando a posição limite
        limitPosition = meshLimits.bounds.extents;

        StartCoroutine(VerifyObject(waitSeconds));
    }

    IEnumerator VerifyObject(int waitSeconds)
    {
        if (surpassLimits())
        {
            transform.position = initialPosition;
        }

        yield return new WaitForSeconds(waitSeconds);


        StartCoroutine(VerifyObject(waitSeconds));
    }

    bool surpassLimits()
    {
        //Sempre considerar o offset em relação ao mundo
        return (transform.position.x > (limitPosition.x + meshLimits.transform.position.x) ||
            transform.position.x < (-limitPosition.x + meshLimits.transform.position.x) ||
            transform.position.y > (limitPosition.y + meshLimits.transform.position.y) ||
            transform.position.y < (-limitPosition.y + meshLimits.transform.position.y) ||
            transform.position.z > (limitPosition.x + meshLimits.transform.position.z) ||
            transform.position.z < (-limitPosition.x + meshLimits.transform.position.z));
    }
}
