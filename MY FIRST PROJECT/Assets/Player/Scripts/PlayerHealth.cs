using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para reiniciar a cena

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;  // Vida máxima do jogador
    public int currentHealth;    // Vida atual do jogador

    public GameObject gameOverCanvas; // Agora referenciamos a Canvas toda
    private Vector3 spawnPosition;   // Posição inicial do player


    void Start()
    {
        currentHealth = maxHealth; // Define a vida inicial do jogador como a vida máxima
        spawnPosition = transform.position; // Salva a posição inicial do jogador
        gameOverCanvas.SetActive(false); // Garante que a tela de Game Over comece desativada
    }

    public void TakeDamage(int damage)  // Função para aplicar dano
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Heal(int amount)  // Função para curar o jogador
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Evita ultrapassar a vida máxima
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
        Time.timeScale = 1f; // Volta ao tempo normal
        currentHealth = maxHealth; // Restaura a vida
        transform.position = spawnPosition; // Move o player para a posição inicial
        gameOverCanvas.SetActive(false); // Desativa a Canvas inteira
    }


}