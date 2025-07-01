using UnityEngine;
using UnityEngine.SceneManagement; // For quitting or scene management

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu Settings")]
    public GameObject pausePanel; // Assign your panel in the Inspector
    private bool isPaused = false;

    private void Update()
    {
        // Toggle pause menu when Escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true); // Show the pause panel
        Time.timeScale = 0f; // Stop the game's time
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false); // Hide the pause panel
        Time.timeScale = 1f; // Resume the game's time
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor back
        Cursor.visible = false; // Hide the cursor again
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        // Quit the application (will work in a build, not in the editor)
        Application.Quit();

        // Optional: If you're testing in the editor, stop playing:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
