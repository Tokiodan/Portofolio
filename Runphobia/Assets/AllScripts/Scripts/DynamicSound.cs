using UnityEngine;

public class DynamicSound : MonoBehaviour
{
    public AudioSource soundSource; // Reference to the sound source
    public Transform playerTransform; // Reference to the player's transform
    public float maxDistance = 20f; // Max distance for the sound to be audible
    public float minVolume = 0.1f; // Minimum volume for the sound when at maxDistance
    public float maxVolume = 1f; // Maximum volume for the sound when close to the player

    private void Start()
    {
        if (soundSource == null)
        {
            soundSource = GetComponent<AudioSource>();
        }

        if (playerTransform == null)
        {
            playerTransform = Camera.main.transform; // Assuming the camera is the player
        }
    }

    private void Update()
    {
        // Calculate the distance between the player and the sound source
        float distance = Vector3.Distance(playerTransform.position, transform.position);

        // Calculate the volume based on the distance
        float volume = Mathf.Clamp01(1 - (distance / maxDistance)); // Volume decreases with distance

        // Set the volume, clamped to the defined min/max volume range
        soundSource.volume = Mathf.Lerp(minVolume, maxVolume, volume);

        // Play or stop the sound based on distance
        if (distance < maxDistance && !soundSource.isPlaying)
        {
            soundSource.Play();  // Start playing sound if the player is within range
        }
        else if (distance >= maxDistance && soundSource.isPlaying)
        {
            soundSource.Stop();  // Stop playing sound if the player is too far
        }
    }
}
