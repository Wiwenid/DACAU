using UnityEngine;

public class SmoothHorizontalMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of movement
    public float damping = 5f;    // Strength of the damping effect

    private float targetVelocity = 0f; // The target movement speed
    private float currentVelocity = 0f; // The smoothed velocity

    void Update()
    {
        // Get input (-1, 0, 1) based on Horizontal key presses (A/D or Left/Right Arrow)
        targetVelocity = Input.GetAxisRaw("Horizontal") * moveSpeed;

        // Smoothly interpolate current velocity toward target velocity
        currentVelocity = Mathf.Lerp(currentVelocity, targetVelocity, damping * Time.deltaTime);

        // Apply movement
        transform.position += new Vector3(currentVelocity * Time.deltaTime, 0, 0);
    }
}