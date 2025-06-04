using UnityEngine;
using TMPro;

public class PlayerStatus : MonoBehaviour
{
    [Header("Energia")]
    public int maxStamina = 5;
    public float staminaRecoveryTime = 0.5f;

    [Header("Movimentação")]
    public float moveSpeed = 3.4f;
    public float sprintSpeed = 6.2f;

    [Header("Dash")]
    public float dashDistance = 8f;
    public float dashTime = 0.25f;
    public float dashCooldown = 2f;

    [Header("HUD de Status")]
    public GameObject statusHUD; // Arraste o objeto da HUD aqui
    public TextMeshProUGUI vidaText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI ataqueText;

    void Start()
    {
        if (statusHUD != null)
            statusHUD.SetActive(true);

        UpdateStatusHUD();
    }

    public void UpdateStatusHUD()
    {
        var playerHealth = GetComponent<PlayerHealth>();
        var comboAttack = GetComponent<ComboAttack>();

        if (vidaText != null && playerHealth != null)
            vidaText.text = $"{playerHealth.maxHealth}";

        if (staminaText != null)
            staminaText.text = $"{maxStamina}";

        if (ataqueText != null && comboAttack != null)
            ataqueText.text = $"{comboAttack.attackDamage}";
    }
}