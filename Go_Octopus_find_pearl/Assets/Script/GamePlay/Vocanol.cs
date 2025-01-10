using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vocanol : MonoBehaviour
{
    public LineRenderer lineRenderer;  // LineRenderer component
    public float maxLength = 10f;      // Maximum length of the line
    public LayerMask obstacleLayers;   // Layers to detect collision (e.g., Wall, Enemy)

    private BoxCollider2D smokeCollider; // Collider for the smoke area

    void Start()
    {
        // Ensure LineRenderer is assigned
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        // Initialize the line with 2 points
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;

        // Add BoxCollider2D dynamically
        smokeCollider = gameObject.AddComponent<BoxCollider2D>();
        smokeCollider.isTrigger = true;

        // Set initial offset
        smokeCollider.offset = Vector2.up * 0.5f; // Offset lên một chút để bắt đầu từ trên đỉnh núi
    }

    void Update()
    {
        DrawVolcanoBeam();
    }

    void DrawVolcanoBeam()
    {
        // Start point of the line
        Vector3 startPoint = transform.position;

        // Cast a ray upwards
        RaycastHit2D hit = Physics2D.Raycast(startPoint, Vector2.up, maxLength, obstacleLayers);

        // Determine the endpoint
        Vector3 endPoint;
        if (hit.collider != null)
        {
            // Stop at the collision point
            endPoint = hit.point;
        }
        else
        {
            // Extend to max length if no collision
            endPoint = startPoint + Vector3.up * maxLength;
        }

        // Set the positions for the line renderer
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);

        // Update BoxCollider2D to match the smoke area
        UpdateSmokeCollider(startPoint, endPoint);
    }

    void UpdateSmokeCollider(Vector3 startPoint, Vector3 endPoint)
    {
        // Calculate the length of the smoke
        float length = Vector3.Distance(startPoint, endPoint);

        // Update the collider size
        smokeCollider.size = new Vector2(0.5f, length); // 0.5f là chiều rộng, chỉnh nếu cần

        // Offset collider để bắt đầu từ gốc núi lửa
        smokeCollider.offset = new Vector2(0f, length / 2f); // Offset dọc theo chiều dài
    }
}
