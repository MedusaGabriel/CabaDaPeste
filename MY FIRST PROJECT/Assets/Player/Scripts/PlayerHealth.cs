using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para reiniciar a cena

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;  // Vida máxima do jogador
    public int currentHealth;    // Vida atual do jogador

    public GameObject gameOverCanvas; // Agora referenciamos a Canvas toda
    private bool isInvincible = false; // Controla se o jogador está invencível
    public float invincibilityDuration = 2f; // Tempo que o jogador fica invencível
    private float invincibilityTimer = 0f;  // Temporizador para a invencibilidade

    void Start()
    {
        currentHealth = maxHealth; // Define a vida inicial do jogador como a vida máxima
        gameOverCanvas.SetActive(false); // Garante que a tela de Game Over comece desativada
    }

    public void TakeDamage(int damage)
    {
        // Se o jogador está invencível, ele não pode levar dano
        if (isInvincible)
            return;

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
        // Verifica se o jogador está invencível e se o tempo de invencibilidade acabou
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false; // Desativa a invencibilidade quando o tempo acaba
            }
        }

        if (gameOverCanvas.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    public void EnableInvincibility()
    {
        isInvincible = true; 
        invincibilityTimer = invincibilityDuration;
    }

    public void DisableInvincibility()
    {
        isInvincible = false; 
    }
}
