using UnityEngine;

public class GroundAnchoredCameraFollow : MonoBehaviour
{
    [Header("Target and Movement")]
    // The object the camera should follow (only in x).
    public Transform target;
    // Damping time for following the target horizontally.
    public float followDampTime = 0.2f;
    
    [Header("Zoom Settings")]
    // Normal orthographic size (when target is low).
    public float normalSize = 5f;
    // Time for zoom transitions.
    public float zoomDampTime = 0.5f;
    
    [Header("Ground Anchor")]
    // The fixed y position of the ground (bottom edge of the camera).
    public float groundLevel = 0f;

    private Camera cam;
    private Vector3 followVelocity = Vector3.zero;
    private float sizeVelocity = 0f;

    void Start()
    {
        cam = Camera.main;
        if (target != null)
        {
            // Initialize the camera position: x follows the target, 
            // y is set so that the bottom of the view is at groundLevel.
            Vector3 pos = transform.position;
            pos.x = target.position.x;
            pos.y = groundLevel + cam.orthographicSize;
            transform.position = pos;
        }
    }

    void LateUpdate()
    {
        if (target == null)
            return;
        
        // Compute the minimum orthographic size required so that the top of the camera (groundLevel + 2*size)
        // reaches the target. In other words, we need:
        //    groundLevel + 2*desiredSize >= target.position.y
        // Hence, desiredSize should be at least (target.position.y - groundLevel)/2.
        float requiredSize = (target.position.y - groundLevel) / 2f;
        // The camera should never be smaller than the normalSize.
        float desiredSize = Mathf.Max(normalSize, requiredSize);
        
        // Smoothly adjust the orthographic size.
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, desiredSize, ref sizeVelocity, zoomDampTime);
        
        // Compute the desired camera position:
        // - x smoothly follows the target.
        // - y is determined by the current orthographic size so that the bottom remains at groundLevel.
        Vector3 desiredPos = new Vector3(target.position.x, groundLevel + cam.orthographicSize, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref followVelocity, followDampTime);
    }
}
