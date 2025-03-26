using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;
    public float angularSpeed = 500f;
    public float acceleration = 10f;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;

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

        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = speed; 
            agent.angularSpeed = angularSpeed; 
            agent.acceleration = acceleration; 
            agent.autoBraking = false; 
        }

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player != null && agent != null)
        {
            if (!animator.GetBool("IsAttacking"))
            {
                agent.SetDestination(player.position);
            }
            else
            {
                agent.isStopped = true;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("IsAttacking", true);
            
            agent.isStopped = true; 
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("IsAttacking", false);
            agent.isStopped = false; 
        }
    }
}