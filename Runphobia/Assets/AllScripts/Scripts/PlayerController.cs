using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;  // Sprinting speed
    public float jumpForce = 5f;
    public float slideSpeed = 8f;
    public float dashDistance = 5f;
    public float dashCooldown = 1f;
    public float slideCooldown = 3f;
    public float jumpCooldown = 2f;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float JumpStam = 20f;
    public float SlideStam = 30f;
    public float DashStam = 50f;
    public float staminaRegenRate = 5f;
    public float sprintStamDrainRate = 10f;  // Sprinting stamina drain rate

    [Header("UI Settings")]
    public Image staminaBar;
    public TMP_Text notificationText;
    public Image dashCooldownImage;

    [Header("Camera Settings")]
    public Camera playerCamera;  // Reference to the camera for zoom effect
    public float zoomedFov = 80f; // The FOV when dashing (zoom out)
    public float zoomSpeed = 10f; // Speed of FOV transition
    public float slideFov = 90f; // The FOV when sliding
    public float slideFovSpeed = 5f; // Speed of FOV transition for sliding
    public float smoothTransitionDuration = 0.3f;  // Smooth transition duration for sliding and standing up

    [Header("Debug Settings")]
    public bool debugLogs = true;

    [Header("Audio Settings")]
    public AudioSource sprintAudioSource;  // Reference to sprinting sound
    public AudioSource dashAudioSource;    // Reference to dashing sound

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isSliding = false;
    private bool canDash = true;
    private float gravity = -9.81f;
    private float yRotation = 0f;
    private Transform cameraTransform;
    private float nextSlideTime = 0f;
    private float currentStamina;
    private float dashCooldownTimer = 0f;

    public float mouseSensitivity = 100f;
    public float slideHeight = 1f;
    public float slideDuration = 1f;  // Reduced slide duration to 1 second
    public float standingHeight = 2f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        if (cameraTransform == null)
        {
            Debug.LogError("Main Camera not found. Ensure your scene has a Main Camera with the tag 'MainCamera'.");
        }

        if (debugLogs) Debug.Log("PlayerController initialized.");

        currentStamina = maxStamina;

        Cursor.lockState = CursorLockMode.Locked;

        // Ensure the playerCamera reference is assigned in the Inspector
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleJump();
        HandleSlide();
        HandleDash();
        UpdateStaminaBar();
        UpdateCooldowns();
        HandleSprinting();  // Sprinting mechanic
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        characterController.Move(move * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            if (currentStamina >= JumpStam && dashCooldownTimer <= 0f)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                currentStamina -= JumpStam;
            }
            else if (currentStamina < JumpStam)
            {
                ShowNotification("Not enough stamina to Jump");
            }
        }
    }

    private void HandleSlide()
    {
        bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

        if (Input.GetKey(KeyCode.LeftControl) && isMoving && characterController.isGrounded && dashCooldownTimer <= 0f)
        {
            if (currentStamina >= SlideStam)
            {
                StartCoroutine(Slide());
            }
            else
            {
                ShowNotification("Not enough stamina to Slide");
            }
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;

        float originalHeight = characterController.height;
        float originalFov = playerCamera.fieldOfView;
        float targetFov = slideFov;
        float timeElapsed = 0f;

        // Smooth transition to sliding FOV
        while (timeElapsed < 0.2f)  // Shorter transition time for FOV zoom
        {
            playerCamera.fieldOfView = Mathf.Lerp(originalFov, targetFov, timeElapsed / 0.2f);
            characterController.height = Mathf.Lerp(originalHeight, slideHeight, timeElapsed / 0.2f);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Set final values for the slide
        playerCamera.fieldOfView = targetFov;
        characterController.height = slideHeight;

        // Smoothly transition the speed to the slide speed
        moveSpeed = Mathf.Lerp(moveSpeed, slideSpeed, 0.2f);

        // Slide duration (1 second)
        float slideTimer = 0f;
        while (slideTimer < 1f)  // Reduced slide duration to 1 second
        {
            slideTimer += Time.deltaTime;
            yield return null;  // Wait until the next frame
        }

        // Smoothly reset back to standing position and speed
        timeElapsed = 0f;
        while (timeElapsed < smoothTransitionDuration)
        {
            playerCamera.fieldOfView = Mathf.Lerp(targetFov, originalFov, timeElapsed / smoothTransitionDuration);
            characterController.height = Mathf.Lerp(slideHeight, standingHeight, timeElapsed / smoothTransitionDuration);
            moveSpeed = Mathf.Lerp(slideSpeed, 5f, timeElapsed / smoothTransitionDuration);  // 5f is the normal speed
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Set final standing values
        playerCamera.fieldOfView = originalFov;
        characterController.height = standingHeight;
        moveSpeed = 5f;  // Reset to normal movement speed

        isSliding = false;
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.C) && canDash && dashCooldownTimer <= 0f)
        {
            bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
            if (isMoving && currentStamina >= DashStam)
            {
                Vector3 dashDirection = Vector3.zero;

                if (Input.GetKey(KeyCode.W)) dashDirection += transform.forward;
                if (Input.GetKey(KeyCode.S)) dashDirection -= transform.forward;
                if (Input.GetKey(KeyCode.A)) dashDirection -= transform.right;
                if (Input.GetKey(KeyCode.D)) dashDirection += transform.right;

                StartCoroutine(Dash(dashDirection.normalized));

                // Play dash sound
                if (!dashAudioSource.isPlaying)
                {
                    dashAudioSource.Play();
                }
            }
            else if (currentStamina < DashStam)
            {
                ShowNotification("Not enough stamina to Dash");
            }
        }
    }

    private IEnumerator Dash(Vector3 direction)
    {
        canDash = false;
        float dashSpeed = dashDistance / 0.2f;  // Dash speed based on the dash duration
        float elapsedTime = 0f;

        // Subtract stamina for the dash
        currentStamina -= DashStam;
        dashCooldownTimer = dashCooldown;

        // Start the zoom-out effect by increasing the FOV
        float originalFov = playerCamera.fieldOfView;
        float targetFov = zoomedFov;  // FOV while dashing
        float timeElapsed = 0f;

        // Smoothly zoom out (increase FOV) during the dash
        while (timeElapsed < 0.2f)
        {
            playerCamera.fieldOfView = Mathf.Lerp(originalFov, targetFov, timeElapsed / 0.2f);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.fieldOfView = targetFov;  // Ensure it reaches the target FOV

        // Perform the dash
        elapsedTime = 0f;
        while (elapsedTime < 0.2f)  // Dash movement duration
        {
            characterController.Move(direction * dashSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset the FOV after dash (smooth transition back to original FOV)
        timeElapsed = 0f;
        while (timeElapsed < 0.2f)
        {
            playerCamera.fieldOfView = Mathf.Lerp(targetFov, originalFov, timeElapsed / 0.2f);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.fieldOfView = originalFov;  // Ensure it reaches the original FOV

        // Stop dash sound after dash is completed
        if (dashAudioSource.isPlaying)
        {
            dashAudioSource.Stop();
        }

        yield return new WaitForSeconds(dashCooldown);  // Wait for dash cooldown
        canDash = true;
    }

    private void HandleSprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            moveSpeed = sprintSpeed; // Set to sprint speed
            currentStamina -= sprintStamDrainRate * Time.deltaTime; // Drain stamina over time while sprinting

            // Play sprint sound if not already playing
            if (!sprintAudioSource.isPlaying)
            {
                sprintAudioSource.Play();
            }
        }
        else
        {
            moveSpeed = 5f; // Reset to normal speed

            // Stop sprint sound when not sprinting
            if (sprintAudioSource.isPlaying)
            {
                sprintAudioSource.Stop();
            }
        }

        // If stamina runs out, stop sprinting
        if (currentStamina < 0)
        {
            currentStamina = 0;
            moveSpeed = 5f;
        }
    }

    private void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
        }

        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
        }
    }

    private void UpdateCooldowns()
    {
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
            dashCooldownImage.fillAmount = dashCooldownTimer / dashCooldown;
        }
    }

    private void ShowNotification(string message)
    {
        notificationText.text = message;
        StartCoroutine(HideNotification());
    }

    private IEnumerator HideNotification()
    {
        yield return new WaitForSeconds(2f);
        notificationText.text = ""; // Hide the notification
    }
}
