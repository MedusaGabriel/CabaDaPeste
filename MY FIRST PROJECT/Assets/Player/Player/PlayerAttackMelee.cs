using UnityEngine;
using System.Collections.Generic;

public class PlayerAttackMelee : MonoBehaviour
{
    [Header("Referência para a Arma")]
    [Tooltip("Arraste aqui o objeto da arma que contém o BoxCollider.")]
    public GameObject weaponObject;
    private PlayerStamina playerStamina;
    private PlayerStatus playerStatus;
    private int comboIndex = 0;
    private float comboResetTime = 1.5f;
    private float comboTimer = 0f;
    public Animator playerAnimator;

    private bool comboQueued = false;
    public ParticleSystem weaponEffect;
    public bool CanMove { get; private set; } = true;
    public bool IsAttacking { get; private set; } = false;

    private HashSet<GameObject> _enemiesHit = new HashSet<GameObject>();

    private void Start()
    {
        playerStamina = GetComponent<PlayerStamina>();
        playerStatus = GetComponent<PlayerStatus>();

        if (weaponEffect != null)
            weaponEffect.Stop();

        Collider playerCollider = GetComponent<Collider>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Collider enemyCollider = enemy.GetComponent<Collider>();
            if (enemyCollider != null)
                Physics.IgnoreCollision(playerCollider, enemyCollider);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsAttacking && playerStamina.TryConsumeStamina())
            {
                PerformComboAttack();
            }
            else if (IsAttacking)
            {
                comboQueued = true;
            }
        }

        if (!IsAttacking && comboQueued)
        {
            if (playerStamina.TryConsumeStamina())
            {
                PerformComboAttack();
                comboQueued = false;
            }
        }

        if (comboIndex > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= comboResetTime)
            {
                ResetCombo();
            }
        }
    }

    private void PerformComboAttack()
    {
        SetAttackState(true, false);
        comboTimer = 0f;

        if (comboIndex == 0)
            playerAnimator.SetTrigger("PlayerMelee");
        else if (comboIndex % 2 == 1)
            playerAnimator.SetTrigger("AtkCombo1");
        else
            playerAnimator.SetTrigger("AtkCombo2");

        comboIndex++;
    }

    public void OnAttackAnimationEnd()
    {
        if (comboQueued && playerStamina.TryConsumeStamina())
        {
            PerformComboAttack();
            comboQueued = false;
        }
        else
        {
            ResetCombo();
        }
    }

    private void ResetCombo()
    {
        comboIndex = 0;
        comboTimer = 0f;
        SetAttackState(false, true);

    }

    private void SetAttackState(bool isAttacking, bool canMove)
    {
        IsAttacking = isAttacking;
        CanMove = canMove;
    }

    public void EnableCollider()
    {
        _enemiesHit.Clear();

        float attackRange = playerStatus.attackRange;
        float attackAngle = 90f;

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy") && !_enemiesHit.Contains(enemy.gameObject))
            {
                Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;

                float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);
                if (angleToEnemy <= attackAngle / 2f)
                {
                    EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                        enemyHealth.TakeDamage(playerStatus.attackDamage);

                    EnemyController enemyController = enemy.GetComponent<EnemyController>();
                    if (enemyController != null)
                    {
                        Vector3 knockbackDirection = directionToEnemy;
                        enemyController.ApplyKnockback(knockbackDirection, playerStatus.knockbackForce);
                    }

                    _enemiesHit.Add(enemy.gameObject);
                }
            }
        }

        playerAnimator.SetBool("IsAttacking", true);
    }

    public void DisableCollider()
    {
        _enemiesHit.Clear();
        playerAnimator.SetBool("IsAttacking", false);
        IsAttacking = false;

        if (weaponEffect != null)
            weaponEffect.Stop();
    }
    public void MovePlayerOn()
    {
        CanMove = true;
    }

    public void MovePlayerOff()
    {
        CanMove = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (playerStatus != null)
        {
            Gizmos.color = Color.blue;

            float attackRange = playerStatus.attackRange; // Distância máxima do ataque
            float attackAngle = 90f; // Ângulo da meia-lua (90 graus)

            // Desenha a meia-lua
            Vector3 forward = transform.forward;
            Vector3 startPosition = transform.position;

            int segments = 20; // Número de segmentos para desenhar o arco
            float angleStep = attackAngle / segments;

            Vector3 previousPoint = startPosition + Quaternion.Euler(0, -attackAngle / 2f, 0) * forward * attackRange;

            for (int i = 1; i <= segments; i++)
            {
                float currentAngle = -attackAngle / 2f + i * angleStep;
                Vector3 currentPoint = startPosition + Quaternion.Euler(0, currentAngle, 0) * forward * attackRange;

                Gizmos.DrawLine(previousPoint, currentPoint);
                previousPoint = currentPoint;
            }

            // Conecta o arco ao centro
            Gizmos.DrawLine(startPosition, startPosition + Quaternion.Euler(0, -attackAngle / 2f, 0) * forward * attackRange);
            Gizmos.DrawLine(startPosition, startPosition + Quaternion.Euler(0, attackAngle / 2f, 0) * forward * attackRange);
        }
    }
}