using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Status Gerais")]
    private int _currentHealth;
    public int currentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Clamp(value, 0, playerStatus.maxHealth);
            if (_currentHealth <= 0 && !isDead)
            {
                Die();
            }
        }
    }

    public GameObject gameOverCanvas;
    public Button restartButton;
    private PlayerStatus playerStatus;
    public Animator playerAnimator;
    private bool isDead = false;
    public CanvasPopUp canvasPopUpPrefab;
    public DamagePopUpPool popupPool;

    [Header("Health UI")]
    public Slider healthSlider;

    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        currentHealth = playerStatus.maxHealth;
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / playerStatus.maxHealth;
        }
        else
        {
            Debug.LogError("Atribua um Slider UI no Inspector!");
        }
    }

    void Update()
    {
        // Não é mais necessário chamar UpdateHealthUI() a cada frame
        if (Input.GetKeyDown(KeyCode.F5))
        {
            int dano = Random.Range(1, 21);
            TakeDamage(dano);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        UpdateHealthUI();

        if (popupPool != null)
        {
            var popup = popupPool.GetPopUp();
            popup.gameObject.SetActive(true);
            popup.playerTransform = transform;
            popup.ShowDamage(damage);
        }
    }

    public void UpdateHealthUI()
    {
        float fill = (float)currentHealth / playerStatus.maxHealth;
        healthSlider.value = fill;
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
}