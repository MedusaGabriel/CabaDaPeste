using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public bool canChasePlayer = true;
    private float lastAttackTime = 0f;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyHit enemyHit;
    private float storedAngularSpeed;
    private bool isAttacking = false;
    private bool canCombo = true;
    private bool isHit = false;

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
        ResetAttack();
    }
    void Update()
    {
        if (agent == null || !agent.enabled)
            return;

        animator.SetFloat("Move", agent.velocity.magnitude / enemyStatus.speed);

        if (isHit || !canChasePlayer || player == null)
        {
            agent.isStopped = true;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= enemyStatus.attackRange)
        {
            agent.isStopped = true;

            if (!isAttacking)
            {
                animator.SetTrigger("Attack");
                isAttacking = true;
            }
            else if (canCombo && distanceToPlayer <= enemyStatus.attackRange)
            {
                animator.SetTrigger("ComboAttack");
                canCombo = false;
            }
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (enemyStatus != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyStatus.attackRange);
        }
    }
    void EnableCombo()
    {
        canCombo = true;
    }

    void ResetAttack()
    {
        isAttacking = false;
        canCombo = true;
    }
    public void DealDamage()
    {
        if (player != null && Time.time >= lastAttackTime + enemyStatus.attackCooldown)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
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