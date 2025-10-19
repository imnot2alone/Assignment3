using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EnergyManager : MonoBehaviour
{
    public int energyCount;
    public TMP_Text energyText;

    [Header("UI References")]
    public GameObject gameOverMenu;

    void Start()
    {
        // Hide menu at the start
        if (gameOverMenu != null)
            gameOverMenu.SetActive(false);
    }

    void Update()
    {
        energyText.text = "Energy Count: " + energyCount.ToString() + "/4";

        // When energyCount reaches 4, show the Game Over menu
        if (energyCount >= 4)
        {
            ShowGameOverMenu();
        }
    }

    void ShowGameOverMenu()
    {
        if (gameOverMenu != null && !gameOverMenu.activeSelf)
        {
            gameOverMenu.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }

    // Called by Retry button
    public void RetryGame()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Called by Menu button
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene"); 
    }
}
