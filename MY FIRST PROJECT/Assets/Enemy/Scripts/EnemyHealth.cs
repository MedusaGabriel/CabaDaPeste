using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    private float currentHealth;
    public Animator enemyAnimator;
    public GameObject playerGameObject;
    private EnemyStatus enemyStatus;

    void Start()
    {
        enemyStatus = GetComponent<EnemyStatus>();
        if (enemyStatus == null)
        {
            Debug.LogError("EnemyStatus não encontrado!");
            return;
        }

        currentHealth = enemyStatus.maxHealth;

        if (playerGameObject == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                playerGameObject = playerObj;
        }

    }

    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, enemyStatus.maxHealth);

        if (enemyAnimator != null)
        {
            if (currentHealth > 0)
            {
                enemyAnimator.SetTrigger("GetHit");
            }
            else
            {
                enemyAnimator.ResetTrigger("GetHit");
                enemyAnimator.SetBool("IsDead", true);
                var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
                if (agent != null) agent.enabled = false;
            }
        }

        EnemyController controller = GetComponent<EnemyController>();
        if (controller != null)
        {
            controller.HandleHitReaction();
        }

        if (currentHealth <= 0)
        {
            StartCoroutine(WaitAndDestroy());
        }
    }

    public void Die()
    {
        StartCoroutine(WaitAndDestroy());
    }

    private System.Collections.IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(2f);

        FindFirstObjectByType<AttackSpecial>()?.AddCharge();
        FindFirstObjectByType<BuffPlayer>()?.OnEnemyKilled();
        GetComponent<EnemyController>().OnDeath();
    }

}