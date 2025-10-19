using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelCompleteController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;                 // whole panel (set inactive in inspector)
    [SerializeField] private TextMeshProUGUI messageText;      // the message text (optional)
    [SerializeField] private Button retryButton;               // Retry button
    [SerializeField] private Button mainMenuButton;            // Main menu button

    [Header("Scene")]
    [SerializeField] private string mainMenuSceneName = "MainMenu"; // name of your main menu scene

    private bool isShown = false;

    private void Awake()
    {
        if (panel != null) panel.SetActive(false);

        // hook up listeners (optional if you prefer to use the Button OnClick in inspector)
        if (retryButton != null) retryButton.onClick.AddListener(OnRetryClicked);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(OnMainMenuClicked);
    }

    // Show the level complete UI and pause gameplay
    public void Show(string message = "Level 1 Completed!")
    {
        if (isShown) return;
        isShown = true;

        if (messageText != null) messageText.text = message;
        if (panel != null) panel.SetActive(true);

        // pause game
        Time.timeScale = 0f;
    }

    // Hide and resume (not strictly needed here, but useful)
    public void Hide()
    {
        if (!isShown) return;
        isShown = false;
        if (panel != null) panel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Button callbacks
    public void OnRetryClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnMainMenuClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
