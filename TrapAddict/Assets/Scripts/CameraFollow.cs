using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform player; // Reference to the player object
    public Vector3 offset = new Vector3(0, 0, -10); // Offset from the player's position
    public float smoothSpeed = 0.125f; // Speed of camera smoothing

    void LateUpdate()
    {
        if (player != null)
        {
            // Target position based on player's position and offset
            Vector3 targetPosition = player.position + offset;

            // Smoothly interpolate between current position and target position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

            // Update camera position
            transform.position = smoothedPosition;
        }
    }
}
