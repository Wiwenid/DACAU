using UnityEngine;

public class IKTargetMouseFollow : MonoBehaviour
{
    public Rigidbody2D rb;
    public float followSpeed = 10f;

    // Define the maximum allowed chain length.
    public float chainLength = 5f;

    // The base transform from which the chain's reach is measured.
    public Transform baseTransform;

    void Update()
    {
        // Get the mouse position in world coordinates.
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (baseTransform != null)
        {
            Vector2 basePos = baseTransform.position;
            Vector2 dirFromBase = mousePos - basePos;
            float distance = dirFromBase.magnitude;

            // Draw a red debug ray from the base position, showing the allowed chain length.
            Debug.DrawRay(basePos, dirFromBase.normalized * chainLength, Color.red);

            // Clamp the target if it exceeds the chain length.
            if (distance > chainLength)
            {
                mousePos = basePos + dirFromBase.normalized * chainLength;
            }
        }

        // Calculate the movement direction from the current rigidbody position.
        Vector2 moveDir = mousePos - rb.position;

        // Move the rigidbody toward the (possibly clamped) target.
        rb.velocity = moveDir * followSpeed;
    }
}