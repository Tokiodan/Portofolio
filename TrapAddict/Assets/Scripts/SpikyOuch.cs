using UnityEngine;

public class SpikyOuch : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 10; // Damage taken when player touches the spike trap
    public float pushForce = 5f; // Force to push the player away when hit
    public LayerMask playerLayer; // Layer that represents the player

    private PlayerHealth playerHealth;
    private Rigidbody2D playerRb; // Reference to the player's Rigidbody2D

    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>(); // Find PlayerHealth script
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other collider belongs to the player layer
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            playerHealth.TakeDamage(damage); // Apply damage to player health
            Debug.Log("Player hit by spike trap!");

            // Push player away
            PushPlayerAway(other.gameObject);
        }
    }

    void PushPlayerAway(GameObject player)
    {
        // Check if the player has a Rigidbody2D component
        playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 pushDirection = (player.transform.position - transform.position).normalized;
            playerRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse); // Adjust the force as needed
        }
    }
}
