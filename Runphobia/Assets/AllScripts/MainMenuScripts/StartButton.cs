using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    // This function will be called when the button is clicked
    public void LoadDream()
    {
        SceneManager.LoadScene("DreamLevel");
    }
}
