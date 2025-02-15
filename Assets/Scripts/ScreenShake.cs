using UnityEngine;
using System.Collections;

public class ScreenShake2D : MonoBehaviour
{
    public float baseShakeMagnitude = 0.1f; // Base intensity for small impacts
    public float maxShakeMagnitude = 0.5f; // Max shake intensity for high impacts
    public float baseShakeDuration = 0.2f; // Minimum shake duration
    public float maxShakeDuration = 0.5f; // Maximum shake duration

    private Vector3 originalPosition;

    private void OnEnable()
    {
        // Subscribe to the collision event
        JianziCollider.OnCollisionEntered += StartShake;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event
        JianziCollider.OnCollisionEntered -= StartShake;
    }

    private void StartShake(float impactForce)
    {
        // Normalize the impact force (Clamp between 0 and 1)
        float normalizedForce = Mathf.Clamp01(impactForce / 10f); // Adjust divisor for scaling
        float shakeMagnitude = Mathf.Lerp(baseShakeMagnitude, maxShakeMagnitude, normalizedForce);
        float shakeDuration = Mathf.Lerp(baseShakeDuration, maxShakeDuration, normalizedForce);

        StartCoroutine(Shake(shakeMagnitude, shakeDuration));
    }

    private IEnumerator Shake(float magnitude, float duration)
    {
        originalPosition = Camera.main.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float xOffset = Random.Range(-1f, 1f) * magnitude;
            float yOffset = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.position = new Vector3(originalPosition.x + xOffset, originalPosition.y + yOffset, originalPosition.z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        // Smoothly return to the original position
        while (Vector3.Distance(Camera.main.transform.position, originalPosition) > 0.01f)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, originalPosition, 10f * Time.deltaTime);
            yield return null;
        }

        Camera.main.transform.position = originalPosition;
    }
}
