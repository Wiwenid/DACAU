using UnityEngine;
using Cinemachine;

public class Cinemachine2DCameraShake : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("Shake Duration")]
    [SerializeField] private float shakeDuration = 0.5f;
    private float shakeTimer;

    [Header("Amplitude Settings")]
    [SerializeField] private float minAmplitude = 1f;      // Minimum amplitude at low impact
    [SerializeField] private float maxAmplitude = 5f;      // Maximum amplitude at high impact

    [Header("Frequency Settings")]
    [SerializeField] private float minFrequency = 1f;      // Minimum frequency at low impact
    [SerializeField] private float maxFrequency = 5f;      // Maximum frequency at high impact

    [Header("Impact Range for Mapping")]
    [SerializeField] private float minImpact = 10f;        // Impact force that triggers minAmplitude
    [SerializeField] private float maxImpact = 30f;        // Impact force that triggers maxAmplitude

    [Header("Damping Settings")]
    [Tooltip("How quickly amplitude and frequency drop during the shake.")]
    [SerializeField] private float dampingSpeed = 1f;      // Larger = faster fade out

    private CinemachineBasicMultiChannelPerlin noise;
    private float originalAmplitude;
    private float targetAmplitude;
    private float targetFrequency;

    private void Awake()
    {
        if (virtualCamera == null)
            virtualCamera = GetComponent<CinemachineVirtualCamera>();

        if (virtualCamera != null)
        {
            noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (noise != null)
            {
                originalAmplitude = noise.m_AmplitudeGain;
            }
            else
            {
                Debug.LogWarning("No CinemachineBasicMultiChannelPerlin component found on the virtual camera.");
            }
        }
        else
        {
            Debug.LogError("Virtual Camera reference is missing!");
        }
    }

    private void OnEnable()
    {
        // Subscribe to the collision event
        JianziCollider.OnCollisionEntered += StartShake;
    }

    private void OnDisable()
    {
        // Unsubscribe from the collision event
        JianziCollider.OnCollisionEntered -= StartShake;
    }

    private void StartShake(float impactForce)
    {
        if (noise == null)
            return;

        shakeTimer = shakeDuration;

        // 1) Map the impactForce to a 0..1 range
        float t = Mathf.InverseLerp(minImpact, maxImpact, impactForce);

        // 2) Lerp amplitude and frequency using that normalized factor
        targetAmplitude = Mathf.Lerp(minAmplitude, maxAmplitude, t);
        targetFrequency = Mathf.Lerp(minFrequency, maxFrequency, t);

        // Immediately set noise to the chosen amplitude/frequency
        noise.m_AmplitudeGain = targetAmplitude;
        noise.m_FrequencyGain = targetFrequency;
    }

    private void Update()
    {
        if (noise == null) return;

        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            // As time goes on, dampen amplitude & frequency
            float damper = shakeTimer / shakeDuration; // This goes from 1 down to 0

            float currentAmplitude = Mathf.Lerp(originalAmplitude, targetAmplitude, damper);
            float currentFrequency = Mathf.Lerp(0, targetFrequency, damper);

            // Optionally apply a damping speed factor to make it drop more quickly
            noise.m_AmplitudeGain = Mathf.Lerp(noise.m_AmplitudeGain, originalAmplitude, Time.deltaTime * dampingSpeed);
            noise.m_FrequencyGain = Mathf.Lerp(noise.m_FrequencyGain, 0, Time.deltaTime * dampingSpeed);
        }
        else
        {
            // Shake ended; reset noise to defaults
            noise.m_AmplitudeGain = originalAmplitude;
            noise.m_FrequencyGain = 0;
        }
    }
}
