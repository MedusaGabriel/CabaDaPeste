using UnityEngine;
using System.Collections;

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
            if (collision.gameObject.CompareTag("Enemy"))
            {
                // Adiciona uma força para baixo (opcional)
                GetComponent<Rigidbody>().AddForce(Vector3.down * 10f, ForceMode.Impulse);
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

    public void DisableEnemyCollision()
    {
        Collider playerCollider = GetComponent<Collider>();
        Collider[] enemyColliders = FindObjectsOfType<Collider>();

        foreach (Collider enemy in enemyColliders)
        {
            if (enemy.CompareTag("Enemy") || enemy.gameObject.name.StartsWith("Enemy"))
            {
                Physics.IgnoreCollision(playerCollider, enemy, true);

                Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
                if (enemyRigidbody != null)
                {
                    enemyRigidbody.isKinematic = true;
                }
            }
        }
        Debug.Log("Colisão com inimigos DESATIVADA!");
    }

    public void EnableEnemyCollision()
    {
        Collider playerCollider = GetComponent<Collider>();
        Collider[] enemyColliders = FindObjectsOfType<Collider>();

        foreach (Collider enemy in enemyColliders)
        {
            if (enemy.CompareTag("Enemy") || enemy.gameObject.name.StartsWith("Enemy"))
            {
                Physics.IgnoreCollision(playerCollider, enemy, false);

                Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
                if (enemyRigidbody != null)
                {
                    enemyRigidbody.isKinematic = false;
                }
            }
        }
        StartCoroutine(AdjustPlayerPositionAfterCollision());
        Debug.Log("Colisão com inimigos ATIVADA!");
    }

    IEnumerator AdjustPlayerPositionAfterCollision()
    {
        yield return null;

        Collider playerCollider = GetComponent<Collider>();
        Collider[] enemyColliders = FindObjectsOfType<Collider>();

        foreach (Collider enemy in enemyColliders)
        {
            if (enemy.CompareTag("Enemy") || enemy.gameObject.name.StartsWith("Enemy"))
            {
                if (playerCollider.bounds.Intersects(enemy.bounds))
                {
                    Vector3 directionAwayFromEnemy = (transform.position - enemy.transform.position).normalized;
                    transform.position += directionAwayFromEnemy * 0.1f;
                }
            }
        }
    }
}
