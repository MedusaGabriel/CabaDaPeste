using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para reiniciar a cena

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;  // Vida máxima do jogador
    public int currentHealth;    // Vida atual do jogador

    public GameObject gameOverCanvas; // Agora referenciamos a Canvas toda

    void Start()
    {
        currentHealth = maxHealth; // Define a vida inicial do jogador como a vida máxima
        gameOverCanvas.SetActive(false); // Garante que a tela de Game Over comece desativada
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
        gameOverCanvas.SetActive(true); // Exibe a tela de Game Over
        Time.timeScale = 0f; // Pausa o jogo
    }

    void Update()
    {
        if (gameOverCanvas.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    void RestartGame()
    {
        Time.timeScale = 1f; // Retorna o tempo ao normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recarrega a cena inteira
    }
}

