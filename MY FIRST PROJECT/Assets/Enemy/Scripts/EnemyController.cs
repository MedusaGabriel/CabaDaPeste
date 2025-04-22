using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    // public float speed = 3f;
    // public float angularSpeed = 0f;
    // public float acceleration = 10f;
    // public float attackCooldown = 1.5f;
    // public float attackRange = 2f;
    public bool canChasePlayer = true;

    private float lastAttackTime = 0f;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyHit enemyHit;
    private bool isHit = false;
    private float storedAngularSpeed;

    private EnemyStatus enemyStatus;

    void Start()
    {
        enemyStatus = GetComponent<EnemyStatus>();
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
            agent.speed = enemyStatus.speed;
            agent.angularSpeed = enemyStatus.angularSpeed;
            agent.acceleration = enemyStatus.acceleration;
            agent.autoBraking = false;
        }

        animator = GetComponent<Animator>();
        enemyHit = GetComponent<EnemyHit>();
    }
    public void HandleHitReaction()
    {
        isHit = true;
        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
        animator.SetTrigger("GetHit");
    }
    public void OnHitAnimationStart()
    {
        isHit = true;
        if (agent != null & agent.enabled)
        {
            storedAngularSpeed = agent.angularSpeed;
            agent.isStopped = true;
            agent.angularSpeed = 0f;
        }
    }
    public void OnHitAnimationEnd()
    {
        isHit = false;
        if (agent != null & agent.enabled)
        {
            agent.isStopped = false;
            agent.angularSpeed = storedAngularSpeed;
        }
    }
    void Update()
    {
        if (agent != null && agent.enabled)
        {
            if (player != null && !isHit && canChasePlayer)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                if (distanceToPlayer <= enemyStatus.attackRange)
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

                agent.isStopped = true; // Para o agente
            }
        }
    }

    private void TryDealDamage(GameObject playerObj)
    {
        if (Time.time >= lastAttackTime + enemyStatus.attackCooldown)
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

    public void ApplyKnockback(Vector3 direction, float force)
    {
        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        StartCoroutine(KnockbackCoroutine(direction, force));
    }

    private IEnumerator KnockbackCoroutine(Vector3 direction, float force)
    {
        float knockbackDuration = 0.5f;
        float elapsedTime = 0f;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + direction * force;

        targetPosition.y = startPosition.y;

        while (elapsedTime < knockbackDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / knockbackDuration);

            // if (NavMesh.SamplePosition(nextPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            // {
            //     transform.position = hit.position;
            // }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        EnableNavMeshAgent();
    }

    private void EnableNavMeshAgent()
    {
        if (agent != null)
        {
            agent.enabled = true;
            agent.isStopped = false;
        }
    }

}