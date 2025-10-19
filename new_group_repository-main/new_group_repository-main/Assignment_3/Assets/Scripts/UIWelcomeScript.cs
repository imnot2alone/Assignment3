using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIWelcomeScript : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI promptText;
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;

    [Header("Colors for Locked Levels")]
    public Color unlockedColor = Color.white;
    public Color lockedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // greyed-out

    void Start()
    {
        UpdateLevelButtons();
    }

    private void UpdateLevelButtons()
    {
        // Level 1 is always unlocked
        bool level1Unlocked = true;
        bool level2Unlocked = PlayerPrefs.GetInt("Level1Completed", 0) == 1;
        bool level3Unlocked = PlayerPrefs.GetInt("Level2Completed", 0) == 1;

        // Update each button's interactable state
        SetButtonState(level1Button, level1Unlocked);
        SetButtonState(level2Button, level2Unlocked);
        SetButtonState(level3Button, level3Unlocked);

        // Set prompt text
        if (!level2Unlocked)
            promptText.text = "Begin by selecting Level 1 to start your adventure!";
        else if (!level3Unlocked)
            promptText.text = "Great job! Level 2 is now unlocked!";
        else
            promptText.text = "All levels unlocked! Choose any to play again!";
    }

    private void SetButtonState(Button button, bool unlocked)
    {
        button.interactable = unlocked;
        var colors = button.colors;
        var image = button.GetComponent<Image>();

        // Change button tint based on locked/unlocked
        if (image != null)
            image.color = unlocked ? unlockedColor : lockedColor;
    }
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();  // Clears all saved keys and values
        PlayerPrefs.Save();       // Force save the cleared data
        UpdateLevelButtons();     // Refresh button states on the menu

        // Optional: update prompt text to confirm reset
        if (promptText != null)
            promptText.text = "Progress reset! Begin again from Level 1.";
    }

}
