using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public BoxCollider attackCollider;
    public float attackDamage = 20f;  // Valor de dano configurável
    public Animator playerAnimator;   // Referência ao Animator do jogador
    public bool CanMove { get; private set; } = true;

    private void Start()
    {
        attackCollider.enabled = false;

        if (playerAnimator == null)
        {
            playerAnimator = GetComponent<Animator>();
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }

    }

    private void Attack()
    {
        playerAnimator.SetTrigger("PlayerMelee");

        Debug.Log("Ataque iniciado!");
    }

    public void EnableCollider()
    {
        attackCollider.enabled = true;
        CanMove = false;
        playerAnimator.SetBool("IsAttacking", true);
        Debug.Log("Collider Ativado!");
    }

    public void DisableCollider()
    {
        attackCollider.enabled = false;
        CanMove = true;
        playerAnimator.SetBool("IsAttacking", false);
        Debug.Log("Collider Desativado!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
                Debug.Log("Inimigo atingido! Dano causado: " + attackDamage);
            }
        }
    }
}