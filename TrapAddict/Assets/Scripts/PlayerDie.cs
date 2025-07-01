using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    public GameObject gameOverPanel; // UI panel to show when player dies

    void OnEnable()
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        playerHealth.gameOverPanel = gameOverPanel; // Assign the Game Over panel
    }
}
