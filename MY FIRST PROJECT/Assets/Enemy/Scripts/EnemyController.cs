using UnityEngine;
using UnityEngine.AI;  // Necessário para usar o NavMeshAgent

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f; // Velocidade do inimigo
    private Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        // Adicionando o NavMeshAgent ao inimigo
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;  // Definindo a velocidade

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
            // Definindo o destino do inimigo para a posição do player
            agent.SetDestination(player.position);
        }
    }
}
