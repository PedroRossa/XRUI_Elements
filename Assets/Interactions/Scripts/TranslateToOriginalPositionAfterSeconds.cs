using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateToOriginalPositionAfterSeconds : MonoBehaviour
{
    public float waitSeconds = 1;
    /// <summary>
    /// The initial position of the object
    /// </summary>
    private Vector3 initialPosition;
    /// <summary>
    /// The proper object's rigidbody
    /// </summary>
    private Rigidbody rb;
    private const float secondsCompass = 0.2f;
    private bool isKinematic;

    /// <summary>
    /// Main setup
    /// </summary>
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        initialPosition = transform.position;
        isKinematic = rb.isKinematic;

        StartCoroutine(ComeBack(waitSeconds));
    }

    private IEnumerator ComeBack(float waitSeconds)
    {
        rb.isKinematic = true;
        float elapsedTime = 0;
        Vector3 actualPos = gameObject.transform.position;
        Vector3 direction = (actualPos - initialPosition).normalized;

        while (elapsedTime < secondsCompass)
        {
            gameObject.transform.position = Vector3.Lerp(actualPos, initialPosition, (elapsedTime / secondsCompass));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.isKinematic = isKinematic;
        yield return new WaitForSeconds(waitSeconds);

        StartCoroutine(ComeBack(waitSeconds));
    }
}
