using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;
    public float angularSpeed = 0f;
    public float acceleration = 10f;
    public float attackCooldown = 1.5f;
    public float attackRange = 2f;
    public bool canChasePlayer = true;

    private float lastAttackTime = 0f;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyHit enemyHit;
    private bool isHit = false;
    private float storedAngularSpeed;

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
        enemyHit = GetComponent<EnemyHit>();
    }
    public void HandleHitReaction()
    {
        isHit = true;
        if (agent != null)
        {
            agent.isStopped = true;
        }
        animator.SetTrigger("GetHit");
    }
    public void OnHitAnimationStart()
    {
        isHit = true;
        if (agent != null)
        {
            storedAngularSpeed = agent.angularSpeed;
            agent.isStopped = true;
            agent.angularSpeed = 0f;
        }
    }
    public void OnHitAnimationEnd()
    {
        isHit = false;
        if (agent != null)
        {
            agent.isStopped = false;
            agent.angularSpeed = storedAngularSpeed;
        }
    }
    void Update()
    {
        if (player != null && agent != null && !isHit && canChasePlayer)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                if (!animator.GetBool("IsAttacking"))
                    animator.SetBool("IsAttacking", true);

                agent.isStopped = true;
                TryDealDamage(player.gameObject);
            }
            else
            {
                if (animator.GetBool("IsAttacking"))
                    animator.SetBool("IsAttacking", false);

                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
        }
        else
        {
            if (animator.GetBool("IsAttacking"))
                animator.SetBool("IsAttacking", false);
            if (agent != null)
                agent.isStopped = true;
        }
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         animator.SetBool("IsAttacking", true);
    //         agent.isStopped = true;

    //         // Tenta aplicar dano ao jogador
    //         TryDealDamage(collision.gameObject);
    //     }
    // }

    // void OnCollisionStay(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         TryDealDamage(collision.gameObject);
    //     }
    // }

    // void OnCollisionExit(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         animator.SetBool("IsAttacking", false);
    //         agent.isStopped = false;
    //     }
    // }

    private void TryDealDamage(GameObject playerObj)
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PlayerHealth playerHealth = playerObj.GetComponent<PlayerHealth>();
            if (playerHealth != null && enemyHit != null)
            {
                int damage = enemyHit.CalculateDamage();
                playerHealth.TakeDamage(damage);
                lastAttackTime = Time.time;
            }
        }
    }

}