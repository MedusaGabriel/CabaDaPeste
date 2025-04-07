using UnityEngine;

public class PlayerAttackMelee : MonoBehaviour
{
    [Header("Referência para a Arma")]
    [Tooltip("Arraste aqui o objeto da arma que contém o BoxCollider.")]
    public GameObject weaponObject;

    public float attackDamage = 20f;
    public Animator playerAnimator;
    public bool CanMove { get; private set; } = true;
    public bool IsAttacking { get; private set; } = false;

    private BoxCollider _weaponCollider;

    private void Start()
    {
        if (weaponObject != null)
        {
            _weaponCollider = weaponObject.GetComponent<BoxCollider>();
            if (_weaponCollider != null)
            {
                _weaponCollider.enabled = false; 
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !IsAttacking)
        {
            Attack();
        }
    }

    private void Attack()
    {
        IsAttacking = true;
        playerAnimator.SetTrigger("PlayerMelee");
    }

    public void EnableCollider()
    {
        if (_weaponCollider != null)
        {
            _weaponCollider.enabled = true;
        }
        CanMove = false;
        playerAnimator.SetBool("IsAttacking", true);
    }

    public void DisableCollider()
    {
        if (_weaponCollider != null)
        {
            _weaponCollider.enabled = false;
        }
        CanMove = true;
        playerAnimator.SetBool("IsAttacking", false);
        IsAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }
}