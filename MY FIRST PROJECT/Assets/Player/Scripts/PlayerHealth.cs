using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Importa a UI

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject gameOverCanvas;
    public Button restartButton; // Referência ao botão de restart

    void Start()
    {
        currentHealth = maxHealth;
        gameOverCanvas.SetActive(false);

        // Garante que o botão chama o método RestartGame() quando clicado
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
        Time.timeScale = 0f; // Pausa o jogo
    }

    public void RestartGame()
    {
        Debug.Log("Botão Restart foi pressionado!"); // Teste no Console
        Time.timeScale = 1f; // Despausa o jogo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recarrega a cena atual
    }
}
