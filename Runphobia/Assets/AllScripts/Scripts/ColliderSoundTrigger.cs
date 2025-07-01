using UnityEngine;

public class ColliderSoundTrigger : MonoBehaviour
{
    [Header("Collider Sound Trigger Settings")]
    [SerializeField] private int touchCount = 3; // Number of times the collider must be touched to play a sound
    [SerializeField] private AudioClip[] soundClips; // Array of sounds to play
    [SerializeField] private float volume = 1f; // Volume of the sound

    private int currentTouchCount = 0;
    private bool hasPlayedSound = false;
    private AudioSource audioSource;

    void Start()
    {
        // Ensure the GameObject has an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = volume;
    }

    void OnTriggerEnter(Collider other)
    {
        // Increment touch count when another collider enters
        currentTouchCount++;

        // Check if the touch count has reached the specified number and the sound hasn't been played yet
        if (currentTouchCount >= touchCount && !hasPlayedSound)
        {
            PlaySound();
            hasPlayedSound = true; // Set the flag to true to prevent further sound plays
        }
    }

    private void PlaySound()
    {
        if (audioSource != null && soundClips.Length > 0)
        {
            // Select a random sound clip
            AudioClip clipToPlay = soundClips[Random.Range(0, soundClips.Length)];
            audioSource.PlayOneShot(clipToPlay);
            Debug.Log("Sound played after " + touchCount + " touches.");
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClips are missing.");
        }
    }
}
