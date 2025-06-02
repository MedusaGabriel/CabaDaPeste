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

    private PlayerStatus playerStatus;
    private bool isDead = false;
    public Animator playerAnimator;
    public CanvasPopUp canvasPopUpPrefab;
    public DamagePopUpPool popupPool;
    [Header("HUD - Game Over UI")]
    public GameObject gameOverCanvas;

    [Header("HUD - Health UI")]
    public GameObject lifeStaminaHUD;
    public Slider healthSlider;
    public Image hudImageFrente;
    public Sprite[] hudImageFrenteSprites;

    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        currentHealth = playerStatus.maxHealth;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(true);
            healthSlider.value = (float)currentHealth / playerStatus.maxHealth;
        }
        else
        {
            Debug.LogError("Atribua um Slider UI no Inspector!");
        }
        if (lifeStaminaHUD != null)
        {
            lifeStaminaHUD.SetActive(true);
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

        // Habilita o mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(PauseGameAfterDelay(2f));
    }

    private System.Collections.IEnumerator PauseGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0f;
    }
}