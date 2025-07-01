using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectPusher : MonoBehaviour
{
    [Header("Power Settings")]
    public float maxPower = 100f;              // Maximum power capacity
    public float currentPower;                 // Current power
    public float powerRegenRate = 5f;          // Rate of power regeneration
    public float powerUsageRate = 10f;         // Amount of power used per action
    public Image powerBar;                     // Reference to the power bar image
    public TMP_Text notificationText;          // Notification Text
    public Camera playerCamera;                // Reference to the player's camera

    [Header("Effect Settings")]
    public float forcePower = 10f;             // The power of the force applied to "Forcable" objects
    public float pushDistance = 5f;            // Maximum distance at which force can push objects

    private bool canUsePower = true;           // If power can be used

    private void Start()
    {
        currentPower = maxPower;

        if (powerBar == null || playerCamera == null || notificationText == null)
        {
            Debug.LogError("Power Bar, Camera, or Notification Text is not assigned!");
        }
    }

    private void Update()
    {
        HandlePowerUsage();
        RegeneratePower();
        UpdatePowerBar();
    }

    // Handle power usage (triggered by mouse button or other inputs)
    private void HandlePowerUsage()
    {
        if (Input.GetMouseButtonDown(1) && currentPower >= powerUsageRate && canUsePower) // Right Mouse Button
        {
            UsePower();
        }
        else if (Input.GetMouseButtonDown(1) && currentPower < powerUsageRate)
        {
            ShowNotification("Not enough power!");
        }
    }

    // Activate power and apply force to "Forcable" objects
    private void UsePower()
    {
        // Drain power
        currentPower -= powerUsageRate;

        // Apply force to "Forcable" objects
        ApplyForceToForcableObjects();

        // Show notification
        ShowNotification("Pushed!");

        // Disable further power use until cooldown completes
        canUsePower = false;
        Invoke(nameof(ResetPowerUsage), 0.5f); // Cooldown duration
    }

    // Reset the ability to use power
    private void ResetPowerUsage()
    {
        canUsePower = true;
    }

    // Apply force to "Forcable" objects in front of the player
    private void ApplyForceToForcableObjects()
    {
        RaycastHit hit;
        // Check for objects in front of the player within a certain distance
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pushDistance))
        {
            if (hit.collider.CompareTag("Forcable"))
            {
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Apply the force
                    rb.AddForce(playerCamera.transform.forward * forcePower, ForceMode.Impulse);
                }
            }
        }
    }

    // Regenerate power over time when not in use
    private void RegeneratePower()
    {
        if (currentPower < maxPower)
        {
            currentPower += powerRegenRate * Time.deltaTime;
            if (currentPower > maxPower)
            {
                currentPower = maxPower;
            }
        }
    }

    // Update the power bar UI
    private void UpdatePowerBar()
    {
        if (powerBar != null)
        {
            powerBar.fillAmount = currentPower / maxPower;
        }
    }

    // Show notification message
    private void ShowNotification(string message)
    {
        notificationText.text = message;
        Invoke(nameof(ClearNotification), 2f); // Duration to display the notification
    }

    // Clear the notification text
    private void ClearNotification()
    {
        notificationText.text = "";
    }
}
