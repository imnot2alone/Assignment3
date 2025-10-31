using UnityEngine;
using TMPro; 
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public TextMeshProUGUI healthText; // Reference to the UI text element
    public DeathScreenManager deathScreen; // Reference to the death screen manager

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth;
        }
    }

    void Die()
    {
        // Add any death effects or animations here
        Debug.Log("Player has died.");
        
        if (deathScreen != null)
        {
            deathScreen.ShowDeathScreen();
        }
        // Optionally, you might want to disable player movement or other components
        // For example: GetComponent<ThirdPersonMovement>().enabled = false;
        gameObject.SetActive(false); // A simple way to "remove" the player
    }
}