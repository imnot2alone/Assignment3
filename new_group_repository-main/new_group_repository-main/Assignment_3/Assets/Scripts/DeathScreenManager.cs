using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    public GameObject deathScreenPanel; // The UI Panel for the death screen

    void Start()
    {
        // Ensure the death screen is hidden at the start
        if (deathScreenPanel != null)
        {
            deathScreenPanel.SetActive(false);
        }
    }

    public void ShowDeathScreen()
    {
        
        if (deathScreenPanel != null)
        {
            deathScreenPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f; // Unpause the game
        // Reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Unpause the game
        SceneManager.LoadScene("MainMenu");
    }
}