using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreepyMenuManager : MonoBehaviour
{
    [Header("Images and UI")]
    public Image displayImage; // Reference to the UI Image component
    public Sprite originalImage; // The original image to revert to after the event
    public Sprite[] randomImages; // Array of creepy images to choose from

    [Header("Sounds")]
    public AudioSource backgroundMusicSource; // Background music AudioSource

    [Header("Timing")]
    [Tooltip("Minimum time between events (seconds)")]
    public float minTime = 30f;
    [Tooltip("Maximum time between events (seconds)")]
    public float maxTime = 60f;

    [Tooltip("Duration to show creepy image (seconds)")]
    public float imageDisplayDuration = 1f; // How long the creepy image stays visible

    private void Start()
    {
        // Set the original image
        if (displayImage != null && originalImage != null)
        {
            displayImage.sprite = originalImage;
        }

        // Start playing the background music
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();
        }

        // Start the random event coroutine
        StartCoroutine(RandomEventCoroutine());
    }

    private IEnumerator RandomEventCoroutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(waitTime);

            TriggerRandomEvent();
        }
    }

    private void TriggerRandomEvent()
    {
        // Ensure there are images to choose from
        if (randomImages.Length > 0)
        {
            StartCoroutine(DisplayRandomImage());
        }
    }

    private IEnumerator DisplayRandomImage()
    {
        // Choose a random image
        Sprite chosenImage = randomImages[Random.Range(0, randomImages.Length)];

        // Change the background music pitch
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.pitch = 0.37f;
        }

        // Display the creepy image
        displayImage.sprite = chosenImage;
        displayImage.enabled = true;

        // Wait for the creepy image duration
        yield return new WaitForSeconds(imageDisplayDuration);

        // Revert to the original image
        displayImage.sprite = originalImage;

        // Reset the background music pitch
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.pitch = 1f;
        }
    }
}
