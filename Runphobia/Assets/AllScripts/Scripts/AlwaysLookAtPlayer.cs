using UnityEngine;

public class AlwaysLookAtPlayer : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private bool lockYAxis = true; // If true, the object will only rotate horizontally
    [SerializeField] private float rotationSpeed = 5f; // Speed at which the object rotates toward the player

    private Transform playerTransform;

    private void Start()
    {
        // Find the GameObject with the "Player" tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("No GameObject with the 'Player' tag was found in the scene.");
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // Calculate the target position
        Vector3 targetPosition = playerTransform.position;

        // If locking Y-axis, keep the object's height unchanged
        if (lockYAxis)
        {
            targetPosition.y = transform.position.y;
        }

        // Calculate the direction to the target
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Smoothly rotate towards the direction
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }
}
