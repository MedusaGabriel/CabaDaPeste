using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 50f;   // Vida máxima do inimigo
    private float currentHealth;    // Vida atual

    [Header("Health UI")]
    public Slider healthSlider;
    public Vector3 sliderOffset = new Vector3(0, 2f, 0);
    public Animator enemyAnimator;
    private GameObject sliderWorldObject;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(false);
            CreateWorldSpaceSlider();
        }
        else
        {
            Debug.LogError("Atribua um Slider UI no Inspector!");
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
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("GetHit");
        }

        EnemyController controller = GetComponent<EnemyController>();
        if (controller != null)
        {
            controller.HandleHitReaction();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthSlider == null) return;

        float healthPercent = currentHealth / maxHealth;
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

    void Die()
    {
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("Die");
        }

        Debug.Log("Inimigo morreu!");
        Destroy(gameObject);
    }
    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
