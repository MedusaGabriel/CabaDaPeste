using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    public int maxStamina = 5;
    public float recoveryTime = 0.5f;
    private int currentStamina;
    private float timer;

    public int CurrentStamina => currentStamina;

    void Start()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {
        if (currentStamina < maxStamina)
        {
            timer += Time.deltaTime;
            if (timer >= recoveryTime)
            {
                currentStamina++;
                timer = 0f;
            }
        }
    }

    public bool TryConsumeStamina(int amount = 1)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            Debug.Log($"Stamina consumida: {amount}. Stamina restante: {currentStamina}");
            return true;
        }
        Debug.Log("Stamina insuficiente. Recuperação de stamina pausada.");
        return false;
    }
}