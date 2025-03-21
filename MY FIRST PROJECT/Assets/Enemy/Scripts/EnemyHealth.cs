using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;  // Vida máxima do inimigo
    private float currentHealth;   // Vida atual

    void Start()
    {
        currentHealth = maxHealth; 
    }

    // Função para o inimigo receber dano
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;  // Reduz a vida do inimigo ao sofrer dano
        if (currentHealth <= 0)
        {
            Die();  // Chama a função de morte se a vida chegar a 0
        }
    }

    // Função chamada quando o inimigo morre
    void Die()
    {
        Debug.Log("Inimigo morreu!");
        // Aqui você pode adicionar animação de morte, efeitos ou destruir o inimigo
        Destroy(gameObject);  // Destrói o inimigo quando a vida chega a 0
    }
}
