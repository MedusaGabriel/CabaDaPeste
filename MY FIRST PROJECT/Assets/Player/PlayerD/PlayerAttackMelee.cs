using UnityEngine;

public class PlayerAttackMelee : MonoBehaviour
{
    public BoxCollider attackCollider;
    public float attackDamage = 20f;
    public Animator playerAnimator;
    public bool CanMove { get; private set; } = true;

    // Novo campo para impedir ataques duplos e travar rotação
    public bool IsAttacking { get; private set; } = false;

    private void Update()
    {
        // Muda de GetKey para GetKeyDown + checagem de IsAttacking
        if (Input.GetKeyDown(KeyCode.E) && !IsAttacking)
        {
            Attack();
        }
    }

    private void Attack()
    {
        IsAttacking = true;
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
        // Libera ataque e rotação ao fim da animação
        IsAttacking = false;
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