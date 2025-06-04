using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{

    [Header("Chase Settings")]
    public float chaseDistance = 10f;
    public bool canChasePlayer = true;


    public Transform mySpawnPoint;
    private SpawnEnemy mySpawner;



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
    public Transform Target;

    void Start()
    {
        enemyStatus = GetComponent<EnemyStatus>();
        player = Target;
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
    public void SetSpawner(SpawnEnemy spawner, Transform spawnPoint)
    {
        mySpawner = spawner;
        mySpawnPoint = spawnPoint;
    }
    public void OnDeath()
    {
        if (mySpawner != null)
        {
            mySpawner.OnEnemyDeath(gameObject, mySpawnPoint);
        }
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
        if (agent != null && agent.enabled)
        {
            storedAngularSpeed = agent.angularSpeed;
            agent.isStopped = true;
            agent.updateRotation = false;
        }
    }

    public void OnHitAnimationEnd()
    {
        isHit = false;
        if (agent != null && agent.enabled)
        {
            agent.angularSpeed = storedAngularSpeed;
            agent.isStopped = false;
            agent.updateRotation = true;
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
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // Só persegue se o player estiver dentro do chaseDistance
        if (distanceToPlayer <= chaseDistance)
        {
            if (distanceToPlayer <= enemyStatus.attackRange && angleToPlayer <= enemyStatus.attackAngle / 2f)
            {
                agent.isStopped = true;

                if (!isAttacking && Time.time >= lastAttackTime + enemyStatus.attackCooldown)
                {
                    animator.SetTrigger("Attack");
                    isAttacking = true;
                    lastAttackTime = Time.time;
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
        else
        {
            agent.isStopped = true;
            // Aqui você pode adicionar lógica de idle ou patrulha, se quiser
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
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // Verifica se o player está dentro da meia-lua de ataque
        if (distanceToPlayer <= enemyStatus.attackRange && angleToPlayer <= enemyStatus.attackAngle / 2f)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null && enemyHit != null)
            {
                int damage = enemyHit.CalculateDamage();
                playerHealth.TakeDamage(damage);
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

    private void OnDrawGizmosSelected()
    {
        if (enemyStatus != null)
        {
            Gizmos.color = Color.red;
            Vector3 center = transform.position + Vector3.up * (enemyStatus.height / 2f);
            int segments = 30;
            float halfAngle = enemyStatus.attackAngle / 2f;
            float radius = enemyStatus.attackRange;
            Vector3 forward = transform.forward;

            Vector3 prevPoint = center + Quaternion.Euler(0, -halfAngle, 0) * forward * radius;
            for (int i = 1; i <= segments; i++)
            {
                float angle = -halfAngle + (enemyStatus.attackAngle * i / segments);
                Vector3 nextPoint = center + Quaternion.Euler(0, angle, 0) * forward * radius;
                Gizmos.DrawLine(prevPoint, nextPoint);
                Gizmos.DrawLine(center, nextPoint);
                prevPoint = nextPoint;
            }
        }
    }

}