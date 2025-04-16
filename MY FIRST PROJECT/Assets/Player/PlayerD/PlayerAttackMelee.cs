using UnityEngine;
// testes de mensagem para brian


public class PlayerAttackMelee : MonoBehaviour
{
    [Header("Referência para a Arma")]
    [Tooltip("Arraste aqui o objeto da arma que contém o BoxCollider.")]
    public GameObject weaponObject;
    public float attackRange = 2f;
    public float attackDamage = 20f;
    public Animator playerAnimator;
    public ParticleSystem weaponEffect; // Adicione esta linha
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

        if (weaponEffect != null)
        {
            weaponEffect.Stop(); // Certifique-se de que o efeito está desativado no início
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

        if (weaponEffect != null)
        {
            weaponEffect.Play(); // Ativa o efeito visual
        }

        // Verifica todos os inimigos no alcance
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                }
            }
        }
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

        if (weaponEffect != null)
        {
            weaponEffect.Stop(); // Desativa o efeito visual
        }
    }
}