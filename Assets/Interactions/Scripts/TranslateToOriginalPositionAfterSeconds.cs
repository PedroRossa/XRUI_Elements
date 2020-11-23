using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TranslateToOriginalPositionAfterSeconds : MonoBehaviour
{
    /// <summary>
    /// Time in seconds to wait until translate again
    /// </summary>
    public float waitSeconds = 1;
    /// <summary>
    /// The initial position of the object
    /// </summary>
    private Vector3 initialPosition;
    /// <summary>
    /// The proper object's rigidbody
    /// </summary>
    private Rigidbody rb;
    /// <summary>
    /// Time compass in seconds to translate back
    /// </summary>
    private const float secondsCompass = 0.2f;
    /// <summary>
    /// Starts the object being kinematic?
    /// </summary>
    private bool isKinematic;
    /// <summary>
    /// Can the object translate now?
    /// </summary>
    private bool canTranslate = true;

    /// <summary>
    /// Main setup
    /// </summary>
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        initialPosition = gameObject.transform.localPosition;
        isKinematic = rb.isKinematic;

        StartCoroutine(ComeBack(waitSeconds));
    }

    private void OnCollisionEnter(Collision collision)
    {
        canTranslate = false;
    }
    private void OnCollisionExit(Collision collision)
    {
        canTranslate = true;
    }

    /// <summary>
    /// Coroutine to translate the object back to its original position
    /// </summary>
    /// <param name="waitSeconds"> Time in seconds to wait to execute coroutine again </param>
    /// <returns></returns>
    private IEnumerator ComeBack(float waitSeconds)
    {
        if (canTranslate)
        {
            rb.isKinematic = true;
            float elapsedTime = 0;
            Vector3 actualPos = gameObject.transform.localPosition;
            Vector3 direction = (actualPos - initialPosition).normalized;

            while (elapsedTime < secondsCompass)
            {
                gameObject.transform.localPosition = Vector3.Lerp(actualPos, initialPosition, (elapsedTime / secondsCompass));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            gameObject.transform.localPosition = initialPosition;

            rb.isKinematic = isKinematic;
        }
        yield return new WaitForSeconds(waitSeconds);

        StartCoroutine(ComeBack(waitSeconds));
    }
}
