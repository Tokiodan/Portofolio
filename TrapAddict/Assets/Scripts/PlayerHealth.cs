using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject gameOverPanel; // UI panel to show when player dies

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            PlayerDie(); // Call the death logic
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player Health: " + currentHealth);
    }

    void PlayerDie()
    {
        Debug.Log("Player Died");
        gameOverPanel.SetActive(true); // Show the Game Over panel
        // Additional logic for handling player death can be added here
    }
}
