using UnityEngine;
using Cinemachine;
using System.Collections;

public class ScreenShake2D : MonoBehaviour
{
    public float baseShakeMagnitude = 0.1f; // Base intensity for small impacts
    public float maxShakeMagnitude = 0.5f; // Max shake intensity for high impacts
    public float baseShakeDuration = 0.2f; // Minimum shake duration
    public float maxShakeDuration = 0.5f; // Maximum shake duration

    private CinemachineVirtualCamera virtualCam;
    private CinemachineBasicMultiChannelPerlin noise;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        // Find the Cinemachine Virtual Camera 2D in the scene
        virtualCam = FindObjectOfType<CinemachineVirtualCamera>();

        if (virtualCam == null)
        {
            Debug.LogWarning("[ScreenShake2D] WARNING: No Cinemachine 2D Virtual Camera found! Screen shake will be disabled.");
            return; // Exit early to prevent errors
        }

        // Get the Noise component for screen shake
        noise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise == null)
        {
            Debug.LogWarning("[ScreenShake2D] WARNING: CinemachineBasicMultiChannelPerlin not found on Virtual Camera! Screen shake will not work.");
        }
    }

    public void StartShake(float impactForce)
    {
        if (virtualCam == null || noise == null)
        {
            Debug.LogWarning("[ScreenShake2D] WARNING: Shake attempted but Virtual Camera or Noise component is missing.");
            return;
        }

        float normalizedForce = Mathf.Clamp01(impactForce / 10f); // Adjust divisor for scaling
        float shakeMagnitude = Mathf.Lerp(baseShakeMagnitude, maxShakeMagnitude, normalizedForce);
        float shakeDuration = Mathf.Lerp(baseShakeDuration, maxShakeDuration, normalizedForce);

        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(Shake(shakeMagnitude, shakeDuration));
    }

    private IEnumerator Shake(float magnitude, float duration)
    {
        if (noise == null)
        {
            Debug.LogWarning("[ScreenShake2D] WARNING: Shake coroutine stopped - Noise component is null!");
            yield break;
        }

        noise.m_AmplitudeGain = magnitude;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Smooth fade-out of screen shake
        float fadeTime = 0.2f;
        float fadeElapsed = 0f;
        float startMagnitude = noise.m_AmplitudeGain;

        while (fadeElapsed < fadeTime)
        {
            noise.m_AmplitudeGain = Mathf.Lerp(startMagnitude, 0f, fadeElapsed / fadeTime);
            fadeElapsed += Time.deltaTime;
            yield return null;
        }

        noise.m_AmplitudeGain = 0f;
    }
}
