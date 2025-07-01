using UnityEngine;

public class ButtonInteractable : MonoBehaviour, IInteractable
{
    [Header("Button Settings")]
    [SerializeField] private int maxPresses = 3; // Maximum number of times the button can be pressed
    [SerializeField] private Transform[] platformsToRaise; // Platforms to raise
    [SerializeField] private Vector3 raiseAmount = new Vector3(0, 5f, 0); // Amount to raise the platforms
    [SerializeField] private float raiseSpeed = 2f; // Speed at which the platforms rise
    [SerializeField] private float buttonPushDistance = 0.1f; // Distance the button moves when pressed
    [SerializeField] private float buttonPushSpeed = 5f; // Speed at which the button moves

    private Vector3 initialButtonPosition;
    private Vector3[] initialPlatformPositions;
    private bool isButtonPushed = false;
    private bool isActivated = false;
    private int remainingPresses;

    void Start()
    {
        // Store initial positions
        initialButtonPosition = transform.position;
        if (platformsToRaise != null && platformsToRaise.Length > 0)
        {
            initialPlatformPositions = new Vector3[platformsToRaise.Length];
            for (int i = 0; i < platformsToRaise.Length; i++)
            {
                if (platformsToRaise[i] != null)
                {
                    initialPlatformPositions[i] = platformsToRaise[i].position;
                }
            }
        }
        remainingPresses = maxPresses; // Initialize remaining presses
    }

    void Update()
    {
        if (isButtonPushed)
        {
            // Smoothly move the button back to its initial position
            transform.position = Vector3.Lerp(transform.position, initialButtonPosition, Time.deltaTime * buttonPushSpeed);
            if (Vector3.Distance(transform.position, initialButtonPosition) < 0.01f)
            {
                isButtonPushed = false;
            }
        }

        if (isActivated && platformsToRaise != null && platformsToRaise.Length > 0)
        {
            // Smoothly raise each platform
            for (int i = 0; i < platformsToRaise.Length; i++)
            {
                if (platformsToRaise[i] != null)
                {
                    Vector3 targetPosition = initialPlatformPositions[i] + raiseAmount;
                    platformsToRaise[i].position = Vector3.Lerp(platformsToRaise[i].position, targetPosition, Time.deltaTime * raiseSpeed);
                }
            }
        }
    }

    public void OnInteract()
    {
        if (remainingPresses > 0)
        {
            ActivateButton();
            remainingPresses--;
        }
        else
        {
            Debug.Log("Button press limit reached.");
        }
    }

    private void ActivateButton()
    {
        isActivated = true;

        // Push the button back slightly
        Vector3 pushPosition = initialButtonPosition - transform.forward * buttonPushDistance;
        transform.position = pushPosition;
        isButtonPushed = true;

        Debug.Log("Button activated! Raising platforms...");
    }
}
