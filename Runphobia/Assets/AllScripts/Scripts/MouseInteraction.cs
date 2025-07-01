using UnityEngine;
using UnityEngine.UI;

public class MouseInteraction : MonoBehaviour
{
    public float interactionDistance = 5f; // Distance within which interactions are valid
    public Image crosshair; // Reference to the crosshair UI image
    public Color defaultCrosshairColor = Color.red; // Default color
    public Color interactableCrosshairColor = Color.white; // Color when looking at interactable

    void Update()
    {
        UpdateCrosshairColor();

        // Check for mouse button press to interact with an object
        if (Input.GetMouseButtonDown(0))
        {
            TryInteractWithObject();
        }
    }

    private void TryInteractWithObject()
    {
        // Cast a ray from the camera to the center of the screen (crosshair position)
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // Check if the object hit has the "Interactable" tag
            if (hit.collider.CompareTag("Interactable"))
            {
                // Attempt to get the IInteractable component and invoke interaction
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.OnInteract();
                }
            }
        }
    }

    private void UpdateCrosshairColor()
    {
        // Cast a ray from the camera to the center of the screen (crosshair position)
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // Change crosshair color if looking at an interactable object
            if (hit.collider.CompareTag("Interactable"))
            {
                if (crosshair != null)
                {
                    crosshair.color = interactableCrosshairColor;
                }
                return;
            }
        }

        // Revert to default color if not looking at an interactable
        if (crosshair != null)
        {
            crosshair.color = defaultCrosshairColor;
        }
    }
}
