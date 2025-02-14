using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class HitAudio : MonoBehaviour
{
    public EventReference impactSoundEvent; // FMOD Event Reference
    private EventInstance impactSoundInstance;

    private void Start()
    {
        // Create an instance of the FMOD event
        impactSoundInstance = RuntimeManager.CreateInstance(impactSoundEvent);
    }

    private void OnEnable() => JianziCollider.OnCollisionEntered += PlayImpactSound;
    private void OnDisable() => JianziCollider.OnCollisionEntered -= PlayImpactSound;

    private void PlayImpactSound(float impactForce)
    {
        float normalizedForce = Mathf.Clamp01(impactForce / 10f); // Normalize force

        impactSoundInstance.setParameterByName("ImpactForce", normalizedForce); // Set FMOD parameter
        impactSoundInstance.setVolume(normalizedForce); // Adjust volume dynamically

        impactSoundInstance.start(); // Play sound
    }

    private void OnDestroy()
    {
        impactSoundInstance.release(); // Release FMOD instance
    }
}