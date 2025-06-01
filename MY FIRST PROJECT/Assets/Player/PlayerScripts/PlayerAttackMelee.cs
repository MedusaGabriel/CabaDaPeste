using UnityEngine;
using System.Collections.Generic;

public class PlayerAttackMelee : MonoBehaviour
{
    [Header("Referência para a Arma")]
    [Tooltip("Arraste aqui o objeto da arma que contém o BoxCollider.")]
    public GameObject weaponObject;

    [Header("Efeito da Espada")]
    public GameObject effectSword;
    private PlayerStamina playerStamina;
    private PlayerStatus playerStatus;
    private int comboIndex = 0;

    public Animator playerAnimator;

    private bool comboQueued = false;
    public bool IsAttacking { get; private set; } = false;
    private HashSet<GameObject> _enemiesHit = new HashSet<GameObject>();
    private Rigidbody rb;

    private bool isPerformingSpecialMove = false;
    private float specialMoveDuration = 0.7f;
    private float specialMoveTimer = 0f;
    public float specialMoveSpeed = 8f;

    private void Start()
    {
        playerStamina = GetComponent<PlayerStamina>();
        playerStatus = GetComponent<PlayerStatus>();
        rb = GetComponent<Rigidbody>();

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
            if (!IsAttacking)
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
            PerformComboAttack();
            comboQueued = false;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            playerAnimator.SetTrigger("SpecialMove");
            isPerformingSpecialMove = true;
            specialMoveTimer = 0f;
        }

        if (isPerformingSpecialMove)
        {
            specialMoveTimer += Time.deltaTime;
            transform.position += transform.forward * specialMoveSpeed * Time.deltaTime;

            if (specialMoveTimer >= specialMoveDuration)
            {
                isPerformingSpecialMove = false;
            }
        }

    }

    private void PerformComboAttack()
    {
        if (!playerStamina.TryConsumeStamina())
        {
            ResetCombo();
            return;
        }

        SetAttackState(true);

        if (comboIndex == 0)
            playerAnimator.SetTrigger("PlayerMelee");
        else if (comboIndex % 2 == 1)
            playerAnimator.SetTrigger("AtkCombo1");
        else
            playerAnimator.SetTrigger("AtkCombo2");

        comboIndex++;
    }
    private void ResetCombo()
    {
        comboIndex = 0;
        SetAttackState(false);
    }

    public void ComboTime()
    {
        if (comboQueued)
        {
            PerformComboAttack();
            comboQueued = false;
        }
        else
        {
            ResetCombo();
            IsAttacking = false;
        }
    }

    private void SetAttackState(bool isAttacking)
    {
        IsAttacking = isAttacking;
    }


    public void ComboStart()
    {
        _enemiesHit.Clear();

        if (effectSword != null)
            effectSword.SetActive(true);

        float attackRange = playerStatus.attackRange;
        float attackAngle = 140f;

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

    public void NextCombo()
    {
        _enemiesHit.Clear();
        playerAnimator.SetBool("IsAttacking", false);
        IsAttacking = false;

        if (effectSword != null)
            effectSword.SetActive(false);
    }

    public void SpecialAttack()
    {
        _enemiesHit.Clear();

        float specialAttackRange = playerStatus.attackRange * 1.5f;
        float specialAttackDamage = playerStatus.attackDamage * 2.5f;

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, specialAttackRange);
        if (effectSword != null)
            effectSword.SetActive(true);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy") && !_enemiesHit.Contains(enemy.gameObject))
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                    enemyHealth.TakeDamage(specialAttackDamage);

                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    Vector3 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                    enemyController.ApplyKnockback(knockbackDirection, playerStatus.knockbackForce * 1.5f);
                }

                _enemiesHit.Add(enemy.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (playerStatus != null)
        {
            Gizmos.color = Color.blue;

            float attackRange = playerStatus.attackRange;
            float attackAngle = 140f;
            float width = attackRange * 0.7f;

            Vector3 forward = transform.forward;
            Vector3 startPosition = transform.position;

            Vector3 leftDir = Quaternion.Euler(0, -attackAngle / 2f, 0) * forward;
            Vector3 rightDir = Quaternion.Euler(0, attackAngle / 2f, 0) * forward;

            Vector3 leftStart = startPosition + leftDir * (attackRange * 0.5f);
            Vector3 rightStart = startPosition + rightDir * (attackRange * 0.5f);

            Vector3 leftEnd = leftStart + leftDir * (attackRange * 0.5f);
            Vector3 rightEnd = rightStart + rightDir * (attackRange * 0.5f);

            int segments = 20;
            float angleStep = attackAngle / segments;
            Vector3 previousPoint = leftEnd;
            for (int i = 1; i <= segments; i++)
            {
                float currentAngle = -attackAngle / 2f + i * angleStep;
                Vector3 currentDir = Quaternion.Euler(0, currentAngle, 0) * forward;
                Vector3 currentPoint = startPosition + currentDir * attackRange;
                Gizmos.DrawLine(previousPoint, currentPoint);
                previousPoint = currentPoint;
            }

            Gizmos.DrawLine(startPosition, leftStart);
            Gizmos.DrawLine(startPosition, rightStart);
            Gizmos.DrawLine(leftStart, leftEnd);
            Gizmos.DrawLine(rightStart, rightEnd);
            Gizmos.DrawLine(leftEnd, previousPoint);
            Gizmos.DrawLine(rightEnd, startPosition + rightDir * attackRange);

            Gizmos.color = Color.red;
            float specialAttackRange = playerStatus.attackRange * 1.5f;
            Gizmos.DrawWireSphere(transform.position, specialAttackRange);
        }
    }


}