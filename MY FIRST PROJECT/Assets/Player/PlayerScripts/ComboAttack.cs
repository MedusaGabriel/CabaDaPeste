using UnityEngine;
using System.Collections.Generic;

public class ComboAttack : MonoBehaviour
{
    public Animator ani;
    public int combo;
    public bool atacando;

    [Header("Configurações de Ataque")]
    public float attackRange = 2.0f;
    public float attackDamage = 20f;
    public float knockbackForce = 5f;
    private HashSet<GameObject> _enemiesHit = new HashSet<GameObject>();

    [Header("Efeito Visual")]
    public GameObject effectSword;
    private PlayerStamina playerStamina;

    void Start()
    {
        ani = GetComponent<Animator>();
        playerStamina = GetComponent<PlayerStamina>();
    }

    public void Start_Combo()
    {
        atacando = false;
        if (combo < 3)
        {
            combo++;
        }
        _enemiesHit.Clear();

        if (effectSword != null)
        {
            effectSword.SetActive(true);
        }

        ApplyDamageToEnemies();
    }

    public void Finish_Ani()
    {
        atacando = false;
        combo = 0;
        if (effectSword != null)
        {
            effectSword.SetActive(false);
        }
    }

    public void Combos_()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !atacando)
        {
            if (playerStamina != null && !playerStamina.TryConsumeStamina())
            {
                return;
            }
            atacando = true;
            ani.SetTrigger("" + combo);
        }
    }

    private void ApplyDamageToEnemies()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy") && !_enemiesHit.Contains(enemy.gameObject))
            {
                Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
                float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

                if (angleToEnemy <= 70f)
                {
                    EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(attackDamage);
                    }

                    EnemyController enemyController = enemy.GetComponent<EnemyController>();
                    if (enemyController != null)
                    {
                        enemyController.ApplyKnockback(directionToEnemy, knockbackForce);
                    }

                    _enemiesHit.Add(enemy.gameObject);
                }
            }
        }
    }

    void Update()
    {
        Combos_();
    }

    // Adicionar visualização da área de ataque (opcional)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        float attackAngle = 140f;
        Vector3 forward = transform.forward;

        Vector3 leftDir = Quaternion.Euler(0, -attackAngle / 2f, 0) * forward;
        Vector3 rightDir = Quaternion.Euler(0, attackAngle / 2f, 0) * forward;

        Gizmos.DrawRay(transform.position, leftDir * attackRange);
        Gizmos.DrawRay(transform.position, rightDir * attackRange);
        int segments = 10;
        float angleStep = attackAngle / segments;
        Vector3 previousPoint = transform.position + leftDir * attackRange;

        for (int i = 1; i <= segments; i++)
        {
            float currentAngle = -attackAngle / 2f + i * angleStep;
            Vector3 currentDir = Quaternion.Euler(0, currentAngle, 0) * forward;
            Vector3 currentPoint = transform.position + currentDir * attackRange;
            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
    }
}