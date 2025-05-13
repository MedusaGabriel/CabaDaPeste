using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Status Gerais")]
    public int currentHealth;
    public GameObject gameOverCanvas;
    public Button restartButton;
    public bool isInvulnerable = false;
    private PlayerStatus playerStatus;
    public Animator playerAnimator;
    private bool isDead = false;
    public CanvasPopUp canvasPopUp;


    [Header("Health UI")]
    public Slider healthSlider;
    public Vector3 sliderOffset = new Vector3(0, 2f, 0);

    private GameObject sliderWorldObject;

    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        currentHealth = playerStatus.maxHealth;
        gameOverCanvas.SetActive(false);

        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(false);
            CreateWorldSpaceSlider();
        }
        else
        {
            Debug.LogError("Atribua um Slider UI no Inspector!");
        }

        // restartButton.onClick.AddListener(RestartGame);
    }

    void Update()
    {
        UpdateHealthUI();
        FaceSliderToCamera();

        // TESTE: Toma dano aleatório ao apertar F5
        if (Input.GetKeyDown(KeyCode.F5))
        {
            int dano = Random.Range(1, 21);
            TakeDamage(dano);
            Debug.Log($"Player tomou {dano} de dano (teste F5)");
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable || isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, playerStatus.maxHealth);

        if (canvasPopUp != null)
            canvasPopUp.ShowDamage(damage);

        if (currentHealth <= 0)
        {

            Die();
        }
    }

    public void UpdateHealthUI()
    {
        if (healthSlider == null) return;

        float healthPercent = (float)currentHealth / playerStatus.maxHealth;
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
        sliderWorldObject = new GameObject("HealthSliderWorld");
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
        if (isDead) return;
        isDead = true;

        Debug.Log("Player morreu!");
        gameOverCanvas.SetActive(true);
        playerAnimator.SetTrigger("Death");
        playerAnimator.SetBool("IsDead", true);
        StartCoroutine(PauseGameAfterDelay(2f));
    }

    private System.Collections.IEnumerator PauseGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Debug.Log("Botão Restart foi pressionado!");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToggleInvincibility()
    {
        if (isInvulnerable)
        {
            DisableInvincibility();
        }
        else
        {
            EnableInvincibility();
        }
    }

    public void EnableInvincibility()
    {
        Debug.Log("Invincibilidade Ativada!");
        isInvulnerable = true;
    }

    public void DisableInvincibility()
    {
        Debug.Log("Invincibilidade Desativada!");
        isInvulnerable = false;
    }
}
