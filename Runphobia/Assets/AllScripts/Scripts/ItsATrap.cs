using UnityEngine;

public class ItsATrap : MonoBehaviour
{
    [Header("Objects to Modify")]
    [SerializeField] private GameObject[] objectsToModify; // Array of objects to modify

    [Header("Modification Settings")]
    [SerializeField] private Vector3 moveDirection = Vector3.left; // Direction to move the objects
    [SerializeField] private float moveSpeed = 1f; // Speed at which the objects move
    [SerializeField] private Vector3 scaleChange = new Vector3(1f, 1f, 1f); // Scale change to apply
    [SerializeField] private Vector3 rotationChange = new Vector3(0f, 45f, 0f); // Rotation change to apply (in degrees)

    private bool playerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the "Player" tag
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            // Start moving, scaling, and rotating the objects
            foreach (GameObject obj in objectsToModify)
            {
                if (obj != null)
                {
                    StartCoroutine(MoveScaleAndRotateObject(obj));
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger has the "Player" tag
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    private System.Collections.IEnumerator MoveScaleAndRotateObject(GameObject obj)
    {
        Vector3 targetPosition = obj.transform.position + moveDirection;
        Vector3 targetScale = obj.transform.localScale + scaleChange;
        Quaternion targetRotation = obj.transform.rotation * Quaternion.Euler(rotationChange);

        // Move, scale, and rotate the object towards the targets
        while (playerInside && (obj.transform.position != targetPosition || obj.transform.localScale != targetScale || obj.transform.rotation != targetRotation))
        {
            if (obj.transform.position != targetPosition)
            {
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }

            if (obj.transform.localScale != targetScale)
            {
                obj.transform.localScale = Vector3.MoveTowards(obj.transform.localScale, targetScale, moveSpeed * Time.deltaTime);
            }

            if (obj.transform.rotation != targetRotation)
            {
                obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, targetRotation, moveSpeed * Time.deltaTime);
            }

            yield return null;
        }
    }
}
