using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f; // Velocidade do inimigo
    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;

            Collider playerCollider = playerObj.GetComponent<Collider>();
            Collider enemyCollider = GetComponent<Collider>();
            if (playerCollider != null && enemyCollider != null)
            {
                Physics.IgnoreCollision(enemyCollider, playerCollider);
            }
        }
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}
