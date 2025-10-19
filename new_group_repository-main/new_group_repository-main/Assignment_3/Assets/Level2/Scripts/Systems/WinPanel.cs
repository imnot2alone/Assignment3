using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinPanel : MonoBehaviour
{
    [Header("Root & Text")]
    [SerializeField] GameObject root;       
    [SerializeField] TMP_Text headerText;   
    [SerializeField] TMP_Text statsText;    

    [Header("Buttons")]
    [SerializeField] Button retryButton;
    [SerializeField] Button menuButton;
    [SerializeField] string menuSceneName = "MenuScene";

    void Awake()
    {
        if (!root) root = gameObject;
        root.SetActive(false);              

        if (retryButton) retryButton.onClick.AddListener(OnRetry);
        if (menuButton)  menuButton.onClick.AddListener(OnMenu);
    }

    public void Show(int energy, int target, float co2)
    {
        if (headerText) headerText.text = "Level Completed";
        if (statsText)  statsText.text  = $"Energy {energy}/{target}\nCO2 {Mathf.RoundToInt(co2)}";
        root.SetActive(true);
        Time.timeScale = 0f;                
    }

    public void Hide()
    {
        Time.timeScale = 1f;
        root.SetActive(false);
    }

    public void OnRetry()
    {
        Hide();
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    public void OnMenu()
    {
        Hide();
        SceneManager.LoadScene(menuSceneName);
    }
}