using UnityEngine;

public class MouseIKController : MonoBehaviour
{
    public Transform solverTarget; // The IK target (foot control)
    public Transform ikBodyParent; // The entire body parent (hip/torso)
    public float maxStretchDistance = 3f; // Max stretch before body moves
    public float dragSpeed = 2f; // How fast the body moves
    public float sensitivity = 0.1f; // Mouse movement sensitivity

    private Vector3 lastMousePosition;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined; // Lock cursor inside the window
        Cursor.visible = false; // Hide cursor
        lastMousePosition = Input.mousePosition;
    }

    void Update()
    {
        // Get mouse movement
        Vector3 mouseDelta = (Input.mousePosition - lastMousePosition) * sensitivity;
        lastMousePosition = Input.mousePosition;

        // Move the solver target (foot)
        solverTarget.position += new Vector3(mouseDelta.x, mouseDelta.y, 0);

        // Check if leg is fully stretched
        float currentStretch = Vector3.Distance(solverTarget.position, ikBodyParent.position);
        if (currentStretch >= maxStretchDistance)
        {
            // Move the entire IK body parent instead of just the foot
            ikBodyParent.position += new Vector3(mouseDelta.x * dragSpeed, 0, 0);
        }

        // Keep cursor locked to the solver target
        Cursor.lockState = CursorLockMode.Confined;
    }
}