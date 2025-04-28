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

    private BoxCollider _weaponCollider;
    private HashSet<GameObject> _enemiesHit = new HashSet<GameObject>();
    private Vector3 _originalColliderSize;

    private void Start()
    {
        playerStamina = GetComponent<PlayerStamina>();
        playerStatus = GetComponent<PlayerStatus>();

        if (weaponObject != null)
        {
            _weaponCollider = weaponObject.GetComponent<BoxCollider>();
            if (_weaponCollider != null)
            {
                _originalColliderSize = _weaponCollider.size;
                _weaponCollider.enabled = false;
            }
        }

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
        if (_weaponCollider != null)
        {
            Vector3 newSize = _originalColliderSize * playerStatus.attackRange;
            _weaponCollider.size = newSize;
            _weaponCollider.enabled = true;
        }
        _enemiesHit.Clear();
        playerAnimator.SetBool("IsAttacking", true);
    }

    public void DisableCollider()
    {
        if (_weaponCollider != null)
            _weaponCollider.enabled = false;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !_enemiesHit.Contains(other.gameObject))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
                enemyHealth.TakeDamage(playerStatus.attackDamage);

            Rigidbody enemyRigidbody = other.GetComponent<Rigidbody>();
            EnemyController enemyController = other.GetComponent<EnemyController>();
            Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;

            if (enemyController != null)
                enemyController.ApplyKnockback(knockbackDirection, playerStatus.knockbackForce);
            else if (enemyRigidbody != null)
                enemyRigidbody.AddForce(knockbackDirection * playerStatus.knockbackForce, ForceMode.Impulse);

            _enemiesHit.Add(other.gameObject);
        }
    }
}