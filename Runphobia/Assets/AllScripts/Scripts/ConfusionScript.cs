using UnityEngine;

public class ConfusionScript : MonoBehaviour
{
    [Header("Portal Properties")]
    public string portalName;  // Optional: A name for your portal
    public ConfusionScript destinationPortal;  // The portal this portal will lead to

    private void OnTriggerEnter(Collider other)
    {
        // If the player enters the portal, teleport them to the destination
        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.transform);
        }
    }

    private void TeleportPlayer(Transform player)
    {
        // Teleport the player to the destination portal's position and rotation
        if (destinationPortal != null)
        {
            player.position = destinationPortal.transform.position;
            player.rotation = destinationPortal.transform.rotation;
        }
    }
}
