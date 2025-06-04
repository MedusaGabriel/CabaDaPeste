using UnityEngine;

public class BuffPlayer : MonoBehaviour
{
    public GameObject buffHUD; // Arraste sua HUD de buff aqui no Inspector
    private int enemiesKilled = 0;
    private int nextBuffKillCount = 3;
    private PlayerStatus playerStatus;
    private ComboAttack comboAttack;

    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        comboAttack = GetComponent<ComboAttack>();
        if (buffHUD != null)
            buffHUD.SetActive(false);
    }

    // Chame este método toda vez que um inimigo morrer
    public void OnEnemyKilled()
    {
        enemiesKilled++;
        if (enemiesKilled >= nextBuffKillCount)
        {
            ActivateBuffHUD();
            nextBuffKillCount *= 2; // Dobra o objetivo para o próximo buff
        }
    }

    void ActivateBuffHUD()
    {
        Time.timeScale = 0f;
        if (buffHUD != null)
            buffHUD.SetActive(true);

        // Habilita o mouse
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Botão: +20 de ataque
    public void BuffAttack()
    {
        if (comboAttack != null)
            comboAttack.attackDamage += 20f;
        HealPlayer();
        playerStatus.UpdateStatusHUD(); // Atualiza HUD
        CloseBuffHUD();
    }

    public void BuffStamina()
    {
        if (playerStatus != null)
            playerStatus.maxStamina += 1;
        HealPlayer();
        playerStatus.UpdateStatusHUD(); // Atualiza HUD
        CloseBuffHUD();
    }

    public void BuffMaxHealth()
    {
        var playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.maxHealth = Mathf.RoundToInt(playerHealth.maxHealth * 1.2f);
            playerHealth.currentHealth = Mathf.Min(playerHealth.currentHealth, playerHealth.maxHealth);
            playerHealth.UpdateHealthUI();
        }
        HealPlayer();
        playerStatus.UpdateStatusHUD(); // Atualiza HUD
        CloseBuffHUD();
    }

    void HealPlayer()
    {
        var playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            int healAmount = Mathf.RoundToInt(playerHealth.maxHealth * 0.4f); // 40% da vida máxima
            playerHealth.currentHealth = Mathf.Min(playerHealth.currentHealth + healAmount, playerHealth.maxHealth);
            playerHealth.UpdateHealthUI();
        }
    }
    void CloseBuffHUD()
    {
        if (buffHUD != null)
            buffHUD.SetActive(false);
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}