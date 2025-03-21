using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;  // Vida máxima do jogador
    public int currentHealth;    // Vida atual do jogador

    void Start()
    {
        currentHealth = maxHealth; // Define a vida inicial do jogador como a vida máxima
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
        
    }

    void Update()
    {
    }
}
