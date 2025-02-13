using UnityEngine;

public class PhysicsIKMouse2D : MonoBehaviour
{
    public Rigidbody2D baseJoint; // The fixed base of the arm
    public Rigidbody2D upperArm;  // The upper arm Rigidbody
    public Rigidbody2D lowerArm;  // The lower arm Rigidbody

    public float forceStrength = 10f; // Strength of forces applied
    public float torqueStrength = 5f; // Strength of torques applied

    private Vector2 targetPosition; // Position the hand should reach

    private void FixedUpdate()
    {
        // Get mouse position in world space
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Compute an automatic pole target (elbow direction)
        Vector2 poleTarget = (Vector2)baseJoint.position + ((targetPosition - (Vector2)baseJoint.position).normalized * 1.5f);
        
        // Apply physics-based IK
        ApplyIK(upperArm, lowerArm, targetPosition, poleTarget);
    }

    void ApplyIK(Rigidbody2D upper, Rigidbody2D lower, Vector2 targetPos, Vector2 polePos)
    {
        // Move lower arm toward target
        Vector2 direction = (targetPos - (Vector2)lower.position).normalized;
        lower.AddForce(direction * forceStrength, ForceMode2D.Force);

        // Adjust elbow using pole target
        Vector2 bendDir = (polePos - (Vector2)upper.position).normalized;
        float angleDiff = Vector2.SignedAngle(upper.transform.up, bendDir);
        upper.AddTorque(angleDiff * torqueStrength, ForceMode2D.Force);
    }
}