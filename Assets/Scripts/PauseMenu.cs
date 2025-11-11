using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel; // Assign your Pause UI Panel here
    private bool isPaused = false;

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Freeze the game
            pausePanel.SetActive(true); // Show pause menu
        }
        else
        {
            Time.timeScale = 1f; // Resume game
            pausePanel.SetActive(false); // Hide pause menu
        }
    }

    // Optional: allow pressing Escape to toggle pause
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}
