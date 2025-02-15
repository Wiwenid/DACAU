using UnityEngine;
using System;

public class JianziCollider : MonoBehaviour
{
    public static event Action<float> OnCollisionEntered; // Now sends force value
    public string targetTag = "Player"; // Tag to check collision
    private ParticleSystem particleSystem;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(targetTag))
        {
            particleSystem.Play();
            // Calculate impact force (relative velocity of collision)
            float impactForce = collision.relativeVelocity.magnitude;

            Debug.Log($"Collision detected with {collision.collider.name} | Impact Force: {impactForce}");

            // Raise the event with the impact force value
            OnCollisionEntered?.Invoke(impactForce);
        }
    }
}