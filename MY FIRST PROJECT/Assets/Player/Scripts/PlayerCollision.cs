using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private EnemyHit enemyHit;
    
    private float damageCooldown = 0.5f; 
    private float nextDamageTime = 0f;  

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemyHit = collision.gameObject.GetComponent<EnemyHit>();

            if (enemyHit != null)
            {
                int damage = enemyHit.CalculateDamage(playerHealth.currentHealth);
                playerHealth.TakeDamage(damage);
                Debug.Log("O jogador recebeu " + damage + " de dano ao entrar em colisão!");
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && enemyHit != null)
        {

            if (Time.time >= nextDamageTime)
            {
                int damage = enemyHit.CalculateDamage(playerHealth.currentHealth);
                playerHealth.TakeDamage(damage);
                Debug.Log("O jogador continua recebendo " + damage + " de dano enquanto colide com o inimigo!");

                nextDamageTime = Time.time + damageCooldown;
            }
        }
    }
}
