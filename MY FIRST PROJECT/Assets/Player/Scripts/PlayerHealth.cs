using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject gameOverCanvas;
    public Button restartButton;

    public bool isInvulnerable = false;

    void Start()
    {
        currentHealth = maxHealth;
        gameOverCanvas.SetActive(false);

        restartButton.onClick.AddListener(RestartGame);
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }
    public void ToggleInvincibility()
    {
        if (isInvulnerable)
        {
            DisableInvincibility();
        }
        else
        {
            EnableInvincibility();
        }
    }
    public void EnableInvincibility()
    {
        Debug.Log("Invincibility Enabled!");
        isInvulnerable = true;
    }

    public void DisableInvincibility()
    {
        Debug.Log("Invincibility Disabled!");
        isInvulnerable = false;
    }
    void Die()
    {
        Debug.Log("Player morreu!");
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Debug.Log("Botão Restart foi pressionado!");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
