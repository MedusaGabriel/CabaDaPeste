using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    private float currentHealth;

    [Header("Health UI")]
    public Slider healthSlider;
    public Vector3 sliderOffset = new Vector3(0, 2f, 0);
    public Animator enemyAnimator;
    private GameObject sliderWorldObject;
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

        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(false);
            CreateWorldSpaceSlider();
        }

    }

    void Update()
    {
        UpdateHealthUI();
        FaceSliderToCamera();
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

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<ExpDrop>().DropXP(playerGameObject);
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        Destroy(gameObject);
    }

    void UpdateHealthUI()
    {
        if (healthSlider == null) return;

        float healthPercent = currentHealth / enemyStatus.maxHealth;
        healthSlider.value = healthPercent;

        healthSlider.gameObject.SetActive(currentHealth > 0);
    }

    void FaceSliderToCamera()
    {
        if (sliderWorldObject != null && Camera.main != null)
        {
            sliderWorldObject.transform.forward = -Camera.main.transform.forward;
        }
    }

    void CreateWorldSpaceSlider()
    {
        sliderWorldObject = new GameObject("EnemyHealthSlider");
        sliderWorldObject.transform.SetParent(transform);
        sliderWorldObject.transform.localPosition = sliderOffset;

        Canvas canvas = sliderWorldObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(2f, 0.5f);

        Slider worldSlider = Instantiate(healthSlider, sliderWorldObject.transform);
        worldSlider.transform.localPosition = Vector3.zero;
        worldSlider.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        healthSlider = worldSlider;
        healthSlider.value = 1;
    }
}