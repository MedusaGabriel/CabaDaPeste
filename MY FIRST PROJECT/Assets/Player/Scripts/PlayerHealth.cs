using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject gameOverCanvas;
    public Button restartButton; 

    void Start()
    {
        currentHealth = maxHealth;
        gameOverCanvas.SetActive(false);

        restartButton.onClick.AddListener(RestartGame);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
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
