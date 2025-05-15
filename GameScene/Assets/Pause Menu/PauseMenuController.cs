using UnityEngine;
using UnityEngine.SceneManagement;  // For loading main menu scene

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    void Start()
    {
        // Ensure pause menu is hidden on start
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;  // Normal time

        // Lock and hide cursor on game start (typical for 3D games)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;  // Resume game time
        isPaused = false;

        // Lock and hide cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;  // Freeze game time
        isPaused = true;

        // Unlock and show cursor for UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadOptions()
    {
        // You can load an options menu scene or open options panel here
        Debug.Log("Options clicked - implement your options menu.");
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;  // Reset time scale before loading new scene
        SceneManager.LoadScene("MainMenu");  // Replace with your main menu scene name
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
