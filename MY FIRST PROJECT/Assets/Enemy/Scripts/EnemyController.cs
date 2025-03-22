using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f; // Velocidade do inimigo
    public float angularSpeed = 500f; // Velocidade de rotação
    public float acceleration = 10f; // Quão rápido ele acelera
    private Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        // Procurar o player pela tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            
            // Ignorar colisão entre inimigo e player
            Collider playerCollider = playerObj.GetComponent<Collider>();
            Collider enemyCollider = GetComponent<Collider>();
            if (playerCollider != null && enemyCollider != null)
            {
                Physics.IgnoreCollision(enemyCollider, playerCollider);
            }
        }

        // Configurar o NavMesh Agent
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = speed; // Controla a velocidade de movimento
            agent.angularSpeed = angularSpeed; // Controla a rapidez na rotação
            agent.acceleration = acceleration; // Controla quão rápido atinge a velocidade máxima
            agent.autoBraking = false; // Evita que ele desacelere antes das curvas
        }
    }

    void Update()
    {
        if (player != null && agent != null)
        {
            agent.SetDestination(player.position); // Fazer o inimigo seguir o player
        }
    }
}
