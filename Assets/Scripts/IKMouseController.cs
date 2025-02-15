using UnityEngine;

public class IKTargetMouseFollow : MonoBehaviour
{
    public enum ControlMode { CursorLock, SlidingPower }

    [Header("General Settings")]
    public ControlMode controlMode = ControlMode.CursorLock;
    [Tooltip("The Rigidbody2D target that follows the mouse.")]
    public Rigidbody2D targetRB;        
    [Tooltip("The base pivot/anchor from where distances are measured.")]
    public Transform baseTransform;     
    [Tooltip("How fast the target moves.")]
    public float followSpeed = 10f;     
    [Tooltip("Primary limit distance from the base (red circle).")]
    public float chainLength = 5f;      

    [Header("Sliding Power Settings (for SlidingPower mode)")]
    [Tooltip("Extra distance allowed beyond the chainLength.")]
    public float maxExtendDistance = 3f;
    [Tooltip("Multiplier for the sliding velocity applied to the sliding object.")]
    public float slidingMultiplier = 1f;
    [Tooltip("The separate object that will slide (not the base object).")]
    public Transform slidingObject;

    void Update()
    {
        if (targetRB == null || baseTransform == null)
            return;

        // Get the mouse position in world space and calculate its vector from the base
        Vector2 basePos = baseTransform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dirFromBase = mousePos - basePos;
        float distance = dirFromBase.magnitude;

        if (controlMode == ControlMode.CursorLock)
        {
            // Clamp the mouse position so it never exceeds chainLength
            if (distance > chainLength)
            {
                mousePos = basePos + dirFromBase.normalized * chainLength;
            }

            // Set the velocity so that the target moves toward the clamped position
            Vector2 moveDir = mousePos - targetRB.position;
            targetRB.velocity = moveDir * followSpeed;
        }
        else if (controlMode == ControlMode.SlidingPower)
        {
            // In SlidingPower mode, the target follows the actual mouse position
            Vector2 moveDir = mousePos - targetRB.position;
            targetRB.velocity = moveDir * followSpeed;

            // Calculate how much the mouse extends beyond the primary limit
            float extension = Mathf.Max(0f, distance - chainLength);
            float extensionFactor = Mathf.Clamp01(extension / maxExtendDistance);

            // If a slidingObject is assigned, slide it horizontally based on the extension factor
            if (slidingObject != null)
            {
                // Determine the horizontal direction (left/right) from the base to the mouse
                float horizontalDir = Mathf.Sign(dirFromBase.x);
                // Compute a target position offset for the sliding object
                Vector3 targetPos = slidingObject.position + new Vector3(horizontalDir * extensionFactor * slidingMultiplier, 0f, 0f);
                // Smoothly move the sliding object toward the target position
                slidingObject.position = Vector3.Lerp(slidingObject.position, targetPos, followSpeed * Time.deltaTime);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (baseTransform != null)
        {
            // Draw the primary limit in red.
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(baseTransform.position, chainLength);

            // Draw the extended limit (chainLength + maxExtendDistance) in blue if in SlidingPower mode.
            if (controlMode == ControlMode.SlidingPower)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(baseTransform.position, chainLength + maxExtendDistance);
            }
        }
    }
}
