using UnityEngine;
using Cinemachine;

public class IndicatorFollowCinemachine : MonoBehaviour
{
    public Transform target; // The object to follow
    public float screenOffset = 10f; // Offset from the screen border
    private Camera mainCamera;

    private Vector3 targetScreenPos;
    private float screenMinX, screenMaxX;

    void Start()
    {
        // Get the active camera from Cinemachine Brain
        CinemachineBrain brain = FindObjectOfType<CinemachineBrain>();
        if (brain != null)
        {
            mainCamera = brain.OutputCamera;
        }
        else
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            Debug.LogError("No main camera or Cinemachine camera found!");
            return;
        }

        // Get screen bounds
        screenMinX = screenOffset;
        screenMaxX = Screen.width - screenOffset;
    }

    void Update()
    {
        if (target == null || mainCamera == null) return;

        // Convert world position to screen space
        targetScreenPos = mainCamera.WorldToScreenPoint(target.position);

        if (targetScreenPos.y > Screen.height || targetScreenPos.y < 0) // If the target is offscreen vertically
        {
            float clampedX = Mathf.Clamp(targetScreenPos.x, screenMinX, screenMaxX); // Restrict movement to left/right
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(clampedX, Screen.height - screenOffset, targetScreenPos.z));

            transform.position = new Vector3(worldPosition.x, transform.position.y, transform.position.z); // Keep Y & Z fixed
            SetVisibility(true);
        }
        else
        {
            SetVisibility(false); // Hide when the object is visible
        }
    }

    private void SetVisibility(bool isVisible)
    {
        GetComponent<SpriteRenderer>().enabled = isVisible;
    }
}