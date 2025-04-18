using UnityEngine;
using System.Collections.Generic;

public class PlayerAttackMelee : MonoBehaviour
{
    [Header("Referência para a Arma")]
    [Tooltip("Arraste aqui o objeto da arma que contém o BoxCollider.")]
    public GameObject weaponObject;
    public float attackDamage = 20f;
    private PlayerStamina playerStamina;

    private int comboIndex = 0;
    private float comboResetTime = 1.5f;
    private float comboTimer = 0f;
    public float knockbackForce = 5f;
    public Animator playerAnimator;

    public ParticleSystem weaponEffect;
    public bool CanMove { get; private set; } = true;
    public bool IsAttacking { get; private set; } = false;

    private BoxCollider _weaponCollider;
    private HashSet<GameObject> _enemiesHit = new HashSet<GameObject>();
    private Vector3 _originalColliderSize;

    public float attackRangeFactor = 1f;

    private void Start()
    {
        playerStamina = GetComponent<PlayerStamina>();

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
        {
            weaponEffect.Stop();
        }

        Collider playerCollider = GetComponent<Collider>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Collider enemyCollider = enemy.GetComponent<Collider>();
            if (enemyCollider != null)
            {
                Physics.IgnoreCollision(playerCollider, enemyCollider);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !IsAttacking && playerStamina.TryConsumeStamina())
        {
            PerformComboAttack();
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
        IsAttacking = true;
        CanMove = false;
        comboTimer = 0f;

        if (comboIndex == 0)
        {
            playerAnimator.SetTrigger("PlayerMelee");
        }
        else if (comboIndex % 2 == 1)
        {
            playerAnimator.SetTrigger("AtkCombo1");
        }
        else if (comboIndex % 2 == 0)
        {
            playerAnimator.SetTrigger("AtkCombo2");
        }

        comboIndex++;
    }

    private void ResetCombo()
    {
        comboIndex = 0;
        comboTimer = 0f;
        IsAttacking = false;
        CanMove = true;

        playerAnimator.ResetTrigger("AtkCombo1");
        playerAnimator.ResetTrigger("AtkCombo2");
        playerAnimator.ResetTrigger("PlayerMelee");
    }

    private void ResetIsAttacking()
    {
        IsAttacking = false;
        CanMove = false;
    }
    public void EnableMovement()
    {
        CanMove = true;
    }

    private void EndAttackAnimation()
    {
        IsAttacking = false;
        CanMove = true;
    }

    public void EnableCollider()
    {
        if (_weaponCollider != null)
        {
            Vector3 newSize = _originalColliderSize * attackRangeFactor;
            _weaponCollider.size = newSize;
            _weaponCollider.enabled = true;
        }
        _enemiesHit.Clear();
        CanMove = false;
        playerAnimator.SetBool("IsAttacking", true);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !_enemiesHit.Contains(other.gameObject))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
                Debug.Log("Dano causado ao inimigo " + other.name + ": " + attackDamage);
            }

            Rigidbody enemyRigidbody = other.GetComponent<Rigidbody>();
            EnemyController enemyController = other.GetComponent<EnemyController>();
            Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;

            if (enemyController != null)
            {
                enemyController.ApplyKnockback(knockbackDirection, knockbackForce);
            }
            else if (enemyRigidbody != null)
            {
                enemyRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            }

            _enemiesHit.Add(other.gameObject);
        }
    }

    public void DisableCollider()
    {
        if (_weaponCollider != null)
        {
            _weaponCollider.enabled = false;
        }
        _enemiesHit.Clear();
        CanMove = true;
        playerAnimator.SetBool("IsAttacking", false);
        IsAttacking = false;

        if (weaponEffect != null)
        {
            weaponEffect.Stop();
        }
    }
}