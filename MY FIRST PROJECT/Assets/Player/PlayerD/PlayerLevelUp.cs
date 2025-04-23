using UnityEngine;

public class PlayerLevelUp : MonoBehaviour
{
    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 49;

    private float baseAttackDamage;
    private int baseMaxHealth;
    private int baseMaxStamina;
    private float baseMoveSpeed;
    private float baseSprintSpeed;
    public GameObject upgradePanel;

    [Header("Configuração de XP para Level Up")]
    [Tooltip("XP necessário para o level 2 = XP base + % (ex: 49 + 20% = 58,8 para level 2) ")]
    [Range(0, 100)]
    public float percentLevel2 = 20f;

    [Tooltip("XP necessário para os próximos níveis = valor anterior( 58,8 + % (ex: 58,8 + 10% para proximo level)")]
    [Range(0, 100)]
    public float percentNextLevels = 10f;

    /*
     * Como funciona:
     * - Level 1: XP base (49)
     * - Level 2: 49 + (49 * percentLevel2/100)
     * - Level 3+: XP anterior + (XP anterior * percentNextLevels/100)
     * 
     * Exemplo: percentLevel2 = 20, percentNextLevels = 10
     * Level 2: 49 + 9.8 = 58.8 (arredonda para 59)
     * Level 3: 59 + 5.9 = 64.9 (arredonda para 65)
     * Level 4: 65 + 6.5 = 71.5 (arredonda para 72)
     */

    private PlayerStatus playerStatus;

    public void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        baseMaxHealth = playerStatus.maxHealth;
        baseMaxStamina = playerStatus.maxStamina;
        baseAttackDamage = playerStatus.attackDamage;
        baseMoveSpeed = playerStatus.moveSpeed;
        baseSprintSpeed = playerStatus.sprintSpeed;
        ApplyLevelStats();
        Debug.Log(
            $"Level inicial: {level}\n" +
            $"XP: {currentXP}/{xpToNextLevel} (Faltam {xpToNextLevel - currentXP} XP para o próximo level)"
        );
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        while (currentXP >= xpToNextLevel)
        {
            currentXP = 0;
            LevelUp();
        }
    }

    public void LevelUp()
    {
        int oldHealth = playerStatus.maxHealth;
        int oldStamina = playerStatus.maxStamina;
        float oldAttack = playerStatus.attackDamage;
        float oldMove = playerStatus.moveSpeed;
        float oldSprint = playerStatus.sprintSpeed;

        level++;
        xpToNextLevel = CalculateXPForNextLevel(level, xpToNextLevel);

        ApplyLevelStats();

        // Cura 100% da vida ao upar de level
        playerStatus.currentHealth = playerStatus.maxHealth;

        // Atualiza a UI de vida
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.currentHealth = playerStatus.maxHealth;
        playerHealth.UpdateHealthUI();

        Debug.Log(
            $"Level Up! Novo level: {level}\n" +
            $"Vida: {oldHealth} → {playerStatus.maxHealth}\n" +
            $"Stamina: {oldStamina} → {playerStatus.maxStamina}\n" +
            $"Dano: {oldAttack} → {playerStatus.attackDamage}\n" +
            $"Velocidade: {oldMove} → {playerStatus.moveSpeed}\n" +
            $"Sprint: {oldSprint} → {playerStatus.sprintSpeed}\n" +
            $"XP para o próximo level: {xpToNextLevel - currentXP}\n" +
            $"Vida curada para {playerStatus.currentHealth}"
        );
        // if (level % 3 == 0)
        // {
            Time.timeScale = 0f;
            upgradePanel.SetActive(true);
        // }
    }

    private int CalculateXPForNextLevel(int currentLevel, int previousXP)
    {
        if (currentLevel == 2)
            return Mathf.RoundToInt(49 + (49 * percentLevel2 / 100f));
        else if (currentLevel > 2)
            return Mathf.RoundToInt(previousXP + (previousXP * percentNextLevels / 100f));
        else
            return 49;
    }

    private void ApplyLevelStats()
    {
        playerStatus.maxHealth = baseMaxHealth + (level - 1) * 10;
        playerStatus.maxStamina = baseMaxStamina + (level - 1);
        playerStatus.attackDamage = baseAttackDamage + (level - 1) * 2f;
        playerStatus.moveSpeed = baseMoveSpeed + (level - 1) * 0.1f;
        playerStatus.sprintSpeed = baseSprintSpeed + (level - 1) * 0.1f;
    }

    public void ChooseUpgrade(string upgradeType)
    {
        switch (upgradeType)
        {
            case "Attack":
                playerStatus.attackDamage += 20f;
                Debug.Log("Upgrade escolhido: Attack (+20 de dano) aplicado!");
                break;
            case "SpeedAttack":
                playerStatus.attackSpeed += 0.5f; // ajuste o valor conforme desejar
                Debug.Log("Upgrade escolhido: SpeedAttack (+0.5 de velocidade de ataque) aplicado!");
                break;
            case "KnockBack":
                playerStatus.knockbackForce += 20f;
                Debug.Log("Upgrade escolhido: KnockBack (+20 de knockback) aplicado!");
                break;
        }
        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}