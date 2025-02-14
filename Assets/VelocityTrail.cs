using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer), typeof(Rigidbody2D))]
public class VelocityTrail2D : MonoBehaviour
{
    public int maxPoints = 50; // Maximum points in the trail
    public float minDistance = 0.1f; // Minimum distance between points
    public float trailDuration = 1f; // Time before the trail fades

    private LineRenderer lineRenderer;
    private Rigidbody2D rb;
    private List<Vector3> trailPoints = new List<Vector3>();

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();

        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.useWorldSpace = true; // Ensures proper positioning in world space
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;

        // Add a point only if it's far enough from the last point
        if (trailPoints.Count == 0 ||
            Vector3.Distance(trailPoints[trailPoints.Count - 1], currentPosition) > minDistance)
        {
            trailPoints.Add(currentPosition);

            // Limit the trail length
            if (trailPoints.Count > maxPoints)
            {
                trailPoints.RemoveAt(0);
            }

            // Update the LineRenderer
            lineRenderer.positionCount = trailPoints.Count;
            lineRenderer.SetPositions(trailPoints.ToArray());
        }

        // Fade out the trail over time
        StartCoroutine(FadeTrail());
    }

    IEnumerator FadeTrail()
    {
        float elapsedTime = 0f;
        while (elapsedTime < trailDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / trailDuration);
            Color trailColor = new Color(1f, 1f, 1f, alpha); // White with transparency
            lineRenderer.startColor = trailColor;
            lineRenderer.endColor = trailColor;
            yield return null;
        }
    }
}