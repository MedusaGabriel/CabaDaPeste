using UnityEngine;
using UnityEngine.UI;

public class AttackSpecial : MonoBehaviour
{
    [Header("Ultimate Settings")]
    public int maxCharges = 5;
    [Tooltip("Cargas iniciais para teste")]
    public int startCharges = 0;
    public int currentCharges = 0;
    public KeyCode specialKey = KeyCode.F;

    [Header("Ataque Especial")]
    public float specialRange = 3f;
    public float specialDamage = 50f;
    public float specialKnockback = 10f;
    public LayerMask enemyLayer;

    [Header("HUD")]
    private bool canUseSpecial => currentCharges >= maxCharges;
    private PlayerHealth playerHealth;
    private Animator animator;

    [Header("Efeito Visual")]
    public GameObject effectSword;

    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        currentCharges = Mathf.Clamp(startCharges, 0, maxCharges);
        UpdateHUD();
    }

    void Update()
    {
        if (canUseSpecial && Input.GetKeyDown(specialKey))
        {
            UseSpecialAttack();
        }
    }

    public void AddCharge()
    {
        if (currentCharges < maxCharges)
        {
            currentCharges++;
            UpdateHUD();
        }
    }

    private void UseSpecialAttack()
    {
        Debug.Log("Ultimate ativada!");

        if (effectSword != null)
            effectSword.SetActive(true);
        if (animator != null)
            animator.SetTrigger("SpecialMove");

        // Aplica dano e knockback em inimigos próximos
        Collider[] hits = Physics.OverlapSphere(transform.position, specialRange, enemyLayer);
        foreach (var hit in hits)
        {
            // Aplica dano se o inimigo tiver EnemyHealth
            var enemyHealth = hit.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
                enemyHealth.TakeDamage(specialDamage);

            // Aplica knockback se tiver Rigidbody
            var rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 dir = (hit.transform.position - transform.position).normalized;
                rb.AddForce(dir * specialKnockback, ForceMode.Impulse);
            }
        }

        currentCharges = 0;
        UpdateHUD();
    }

    public void FinishSpecialEffect()
    {
        if (effectSword != null)
            effectSword.SetActive(false);
    }

    private void UpdateHUD()
    {
        if (playerHealth != null && playerHealth.hudImageFrente != null && playerHealth.hudImageFrenteSprites != null && playerHealth.hudImageFrenteSprites.Length > 0)
        {
            int spriteIndex = Mathf.Clamp(currentCharges, 0, playerHealth.hudImageFrenteSprites.Length - 1);
            playerHealth.hudImageFrente.sprite = playerHealth.hudImageFrenteSprites[spriteIndex];
        }
    }

#if UNITY_EDITOR
    // Gizmo para visualizar o alcance do ataque especial
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, specialRange);
    }
#endif
}