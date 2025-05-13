using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    [Header("Vida e Energia")]
    public int maxHealth = 100;
    public int currentHealth;
    public int maxStamina = 5;
    public float staminaRecoveryTime = 0.5f;

    [Header("Movimentação")]
    public float moveSpeed = 3.4f;
    public float sprintSpeed = 6.2f;

    [Header("Dash")]
    
    public float dashDistance = 8f;
    public float dashTime = 0.25f;
    public float dashCooldown = 2f;

    [Header("Ataque")]
    public float attackDamage = 20f;
    public float attackSpeed = 1f;
    public float attackRange = 1f;
    public float knockbackForce = 5f;
}
