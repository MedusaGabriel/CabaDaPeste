using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    private int currentStamina;
    private float timer;
    public int CurrentStamina => currentStamina;
    public Slider staminaSlider;
    private PlayerStatus playerStatus;
    private CanvasPopUp currentPopup;


    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        currentStamina = playerStatus.maxStamina;
        if (staminaSlider != null)
        {
            staminaSlider.gameObject.SetActive(true); // Ative o slider na UI
            staminaSlider.value = 1f;
        }
        else
        {
            Debug.LogError("Atribua um Slider UI no Inspector para Stamina!");
        }
        // Remova chamada para CreateWorldSpaceSlider();
    }

void Update()
{
    UpdateStaminaUI();
    if (currentStamina < playerStatus.maxStamina)
    {
        timer += Time.deltaTime;
        if (timer >= playerStatus.staminaRecoveryTime)
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
            return true;
        }
        // ShowNoStaminaMessage();
        return false;

    }

    void UpdateStaminaUI()
    {
        if (staminaSlider == null) return;

        float fill = (float)currentStamina / playerStatus.maxStamina;
        staminaSlider.value = fill;
    }

    // public void ShowCustomMessage(string message)
    // {
    //     popupText.text = message;
    //     popupText.gameObject.SetActive(true);

    //     currentYOffset = Random.Range(minYOffset, maxYOffset);

    //     if (popupAnimator != null)
    //     {
    //         popupAnimator.Play("Pop-Up", -1, 0f);
    //     }

    //     if (fadeCoroutine != null)
    //         StopCoroutine(fadeCoroutine);
    //     fadeCoroutine = StartCoroutine(FadeOutAndDeactivate(fadeDuration));
    // }
}