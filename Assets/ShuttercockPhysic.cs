using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JianziPhysics : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Aerodynamics Settings")]
    [Tooltip("A coefficient to adjust the magnitude of aerodynamic drag.")]
    public float dragCoefficient = 1.0f;
    [Tooltip("Offset to add to the calculated angle so the sprite faces correctly.")]
    public float offset = 90f;
    
    [Tooltip("Estimate of the frontal area of the Jianzi (in square meters).")]
    public float frontalArea = 0.05f;
    
    [Tooltip("Density of air (kg/m^3). Standard sea-level value is 1.225.")]
    public float airDensity = 1.225f;
    
    [Header("Rotation Settings")]
    [Tooltip("How quickly the Jianzi rotates to align with its velocity.")]
    public float rotationLerpSpeed = 5f;

    [Header("External Forces")]
    [Tooltip("Optional wind force that can be applied (set to zero if not used).")]
    public Vector2 windForce = Vector2.zero;

    [Header("Impact Response")]
    [Tooltip("Multiplier for the impact’s vertical speed to set the upward velocity.")]
    public float impactMultiplier = 1f;
    [Tooltip("The maximum upward speed allowed after an impact.")]
    public float maxUpwardSpeed = 20f;

    [Header("Velocity Limits")]
    [Tooltip("The maximum allowed overall speed for the Jianzi to prevent tunneling.")]
    public float maxSpeed = 30f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Set collision detection to Continuous Speculative for better collision handling on fast-moving objects.
        rb.collisionDetectionMode = CollisionDetectionMode2D.ContinuousSpeculative;
    }

    void FixedUpdate()
    {
        ApplyAerodynamicDrag();
        ApplyWindForce();
        AlignRotationToVelocity();

        // Clamp overall velocity to prevent tunneling issues.
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }

    /// <summary>
    /// Applies aerodynamic drag based on the Jianzi’s current velocity.
    /// Formula: F_drag = 0.5 * airDensity * speed^2 * dragCoefficient * frontalArea
    /// </summary>
    void ApplyAerodynamicDrag()
    {
        Vector2 velocity = rb.velocity;
        float speed = velocity.magnitude;
        
        if (speed > 0.01f)
        {
            float dragMagnitude = 0.5f * airDensity * speed * speed * dragCoefficient * frontalArea;
            Vector2 dragForce = -dragMagnitude * (velocity / speed);
            rb.AddForce(dragForce);
        }
    }

    /// <summary>
    /// Applies an optional wind force to the Jianzi.
    /// </summary>
    void ApplyWindForce()
    {
        if (windForce != Vector2.zero)
        {
            rb.AddForce(windForce);
        }
    }

    /// <summary>
    /// Aligns the Jianzi’s rotation with its velocity vector.
    /// </summary>
    void AlignRotationToVelocity()
    {
        Vector2 velocity = rb.velocity;
        if (velocity.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + offset;
            float newRotation = Mathf.LerpAngle(rb.rotation, targetAngle, rotationLerpSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(newRotation);
        }
    }

    /// <summary>
    /// Custom collision response to adjust the upward speed based on the impact.
    /// </summary>
    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Check if the contact normal indicates an impact from below.
            if (contact.normal.y > 0.5f)
            {
                float impactVerticalSpeed = Mathf.Abs(collision.relativeVelocity.y);
                float desiredUpwardSpeed = impactVerticalSpeed * impactMultiplier;
                desiredUpwardSpeed = Mathf.Clamp(desiredUpwardSpeed, 0, maxUpwardSpeed);

                // Set the new velocity while preserving the horizontal component.
                Vector2 newVelocity = new Vector2(rb.velocity.x, desiredUpwardSpeed);
                rb.velocity = newVelocity;
                break;
            }
        }
    }
}
