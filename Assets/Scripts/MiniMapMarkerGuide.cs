using UnityEngine;
using UnityEngine.UI;

public class MiniMapMarkerGuide : MonoBehaviour
{
    public RectTransform miniMapMarker; // The marker UI Image on the mini-map
    public Transform target;            // The target location in world space
    public RectTransform miniMapPanel;  // The mini-map UI panel (RectTransform)
    public Camera miniMapCamera;        // The mini-map orthographic camera (top-down view)

    void Update()
    {
        UpdateMarkerPosition();
    }

    void UpdateMarkerPosition()
    {
        // Convert the target's world position to mini-map camera viewport space (x, y between 0 and 1)
        Vector3 miniMapPosition = miniMapCamera.WorldToViewportPoint(target.position);

        // Ensure the target is within the viewport (z > 0 means it's in front of the camera)
        if (miniMapPosition.z < 0)
        {
            // If the target is behind the camera, flip its position to keep it in view
            miniMapPosition.x = 1 - miniMapPosition.x;
            miniMapPosition.y = 1 - miniMapPosition.y;
        }

        // Map the viewport coordinates (0 to 1) to the local space of the mini-map panel
        float miniMapWidth = miniMapPanel.rect.width;
        float miniMapHeight = miniMapPanel.rect.height;

        // Calculate the marker's position in the mini-map's local space
        Vector2 markerPosition = new Vector2(
            (miniMapPosition.x - 0.5f) * miniMapWidth,  // Adjusting for the center
            (miniMapPosition.y - 0.5f) * miniMapHeight  // Adjusting for the center
        );

        // Check if the marker is off the mini-map (out of bounds)
        bool isOutOfBounds = miniMapPosition.x < 0 || miniMapPosition.x > 1 || miniMapPosition.y < 0 || miniMapPosition.y > 1;

        if (isOutOfBounds)
        {
            // Normalize the direction and clamp the marker to the edge of the mini-map
            Vector2 direction = new Vector2(miniMapPosition.x - 0.5f, miniMapPosition.y - 0.5f).normalized;
            markerPosition = direction * (Mathf.Min(miniMapWidth, miniMapHeight) / 2f);  // Edge of mini-map
        }

        // Clamp the marker position within the mini-map bounds
        markerPosition.x = Mathf.Clamp(markerPosition.x, -miniMapWidth / 2f, miniMapWidth / 2f);
        markerPosition.y = Mathf.Clamp(markerPosition.y, -miniMapHeight / 2f, miniMapHeight / 2f);

        // Set the marker's anchored position
        miniMapMarker.anchoredPosition = markerPosition;

        // Ensure the marker is visible
        miniMapMarker.gameObject.SetActive(true);

        // Debug to check if the marker position is within bounds
    }
}
