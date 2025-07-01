using UnityEngine;

public class PickupableItem : MonoBehaviour
{
    public float interactionDistance = 5f; // Distance within which interactions are valid
    public Transform holdPoint; // Reference to the hold point in front of the player
    public float throwForce = 10f; // Force applied when throwing the object

    private GameObject heldObject = null; // The object currently being held
    private Rigidbody heldObjectRigidbody = null; // Rigidbody of the held object

    void Update()
    {
        // Check for mouse button press to pick up an object
        if (Input.GetMouseButtonDown(0) && heldObject == null) // 0 = Left mouse button
        {
            TryPickUpObject();
        }

        // Check for 'Q' key press to throw the object
        if (Input.GetKeyDown(KeyCode.Q) && heldObject != null)
        {
            ThrowObject();
        }
    }

    private void TryPickUpObject()
    {
        // Cast a ray from the camera to the center of the screen (crosshair position)
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // Check if the object hit has the "Pickups" tag
            if (hit.collider.CompareTag("Pickups"))
            {
                PickUpObject(hit.collider.gameObject);
            }
        }
    }

    private void PickUpObject(GameObject objectToPickUp)
    {
        if (objectToPickUp.GetComponent<Rigidbody>() == null)
        {
            Debug.LogWarning("Object does not have a Rigidbody component. Can't pick it up.");
            return;
        }

        heldObject = objectToPickUp;
        heldObjectRigidbody = heldObject.GetComponent<Rigidbody>();
        heldObjectRigidbody.isKinematic = true; // Disable physics to prevent it from moving
        heldObject.transform.SetParent(holdPoint); // Parent to the hold point
        heldObject.transform.localPosition = Vector3.zero; // Position it at the hold point
        heldObject.transform.localRotation = Quaternion.identity; // Reset rotation
        Debug.Log($"Picked up {heldObject.name}");
    }

    private void ThrowObject()
    {
        if (heldObject != null)
        {
            heldObjectRigidbody.isKinematic = false; // Enable physics for the thrown object
            heldObject.transform.SetParent(null); // Unparent from the hold point
            Vector3 throwDirection = Camera.main.transform.forward; // Direction the camera is facing
            heldObjectRigidbody.AddForce(throwDirection * throwForce, ForceMode.VelocityChange);
            heldObject = null;
            Debug.Log("Thrown the object");
        }
    }
}
