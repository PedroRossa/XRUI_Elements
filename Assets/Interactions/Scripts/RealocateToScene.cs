using System.Collections;
using UnityEngine;

/// <summary>
/// Class to control objects that come out a limits field to realocate them
/// </summary>
public class RealocateToScene : MonoBehaviour
{
    /// <summary>
    /// The mesh of the limits field
    /// </summary>
    public MeshRenderer meshLimits;

    /// <summary>
    /// The initial position of the object
    /// </summary>
    private Vector3 initialPosition;
    /// <summary>
    /// The positive limit point
    /// </summary>
    private Vector3 limitPosition;
    /// <summary>
    /// Constant to wait till realocate the object to scene
    /// </summary>
    private const int waitSeconds = 2;
    /// <summary>
    /// The proper object's rigidbody
    /// </summary>
    private new Rigidbody rigidbody;

    /// <summary>
    /// Main setup
    /// </summary>
    void Start()
    {
        initialPosition = transform.position;
        rigidbody = GetComponent<Rigidbody>();

        //Setando a posição limite
        limitPosition = meshLimits.bounds.extents;

        StartCoroutine(VerifyObject(waitSeconds));
    }

    /// <summary>
    /// Verify if the object has surpassed the limit field perimeter each 2 seconds. If it has, realocate it.
    /// </summary>
    /// <param name="waitSeconds"></param>
    /// <returns></returns>
    IEnumerator VerifyObject(int waitSeconds)
    {
        if (surpassLimits())
        {
            transform.position = initialPosition;
            rigidbody.velocity = Vector3.zero;
        }

        yield return new WaitForSeconds(waitSeconds);


        StartCoroutine(VerifyObject(waitSeconds));
    }

    /// <summary>
    /// Function to verify if the object has surpassed the limit field perimeter
    /// </summary>
    /// <returns></returns>
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
