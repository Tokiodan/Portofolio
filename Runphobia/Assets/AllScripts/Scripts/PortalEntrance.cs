using UnityEngine;

public class PortalEntrance : MonoBehaviour
{
    [Header("Portal Settings")]
    [SerializeField] private Transform linkedExit;  // Reference to the linked portal exit

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.transform);
        }
    }

    private void TeleportPlayer(Transform player)
    {
        if (linkedExit != null)
        {
            CharacterController characterController = player.GetComponent<CharacterController>();
            if (characterController != null)
            {
                characterController.enabled = false; // Temporarily disable to set position
                player.position = linkedExit.position;
                characterController.enabled = true; // Re-enable after setting position
            }
            else
            {
                Debug.LogWarning("CharacterController component not found on player.");
            }
        }
        else
        {
            Debug.LogWarning("Linked exit not assigned.");
        }
    }
}
