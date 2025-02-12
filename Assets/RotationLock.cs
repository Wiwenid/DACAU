using UnityEngine;

public class RotationConstraint2D : MonoBehaviour
{
    [Header("Rotation Limits")]
    public float minRotation = -45f; // Minimum rotation angle
    public float maxRotation = 45f;  // Maximum rotation angle

    private void LateUpdate()
    {
        // Get the current rotation
        float currentRotation = transform.localEulerAngles.z;

        // Convert rotation to a -180 to 180 range
        if (currentRotation > 180f)
            currentRotation -= 360f;

        // Clamp rotation within min/max bounds
        float clampedRotation = Mathf.Clamp(currentRotation, minRotation, maxRotation);

        // Apply the clamped rotation
        transform.localRotation = Quaternion.Euler(0, 0, clampedRotation);
    }
}