using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    private int currentStamina;
    private float timer;
    public int CurrentStamina => currentStamina;

    public Slider staminaSlider;
    public Vector3 sliderOffset = new Vector3(0, 2f, 0);
    private GameObject sliderWorldObject;
    private PlayerStatus playerStatus;
    public CanvasPopUp noStaminaPrefab;
    private CanvasPopUp currentPopup;


    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        currentStamina = playerStatus.maxStamina;
        if (staminaSlider != null)
        {
            CreateWorldSpaceSlider();
        }
        else
        {
            Debug.LogError("Atribua um Slider UI no Inspector para Stamina!");
        }
    }

    void LateUpdate()
    {
        if (sliderWorldObject != null)
        {
            sliderWorldObject.transform.position = transform.position + sliderOffset;
            FaceSliderToCamera();
        }
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
        ShowNoStaminaMessage();
        return false;

    }

    void CreateWorldSpaceSlider()
    {
        sliderWorldObject = new GameObject("StaminaSliderWorld");
        sliderWorldObject.transform.SetParent(transform);
        sliderWorldObject.transform.localPosition = sliderOffset;

        Canvas canvas = sliderWorldObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(2f, 0.5f);

        Slider worldSlider = Instantiate(staminaSlider, sliderWorldObject.transform);
        worldSlider.transform.localPosition = Vector3.zero;
        worldSlider.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        staminaSlider = worldSlider;
        staminaSlider.value = 1f;
    }

    void UpdateStaminaUI()
    {
        if (staminaSlider == null) return;

        float fill = (float)currentStamina / playerStatus.maxStamina;
        staminaSlider.value = fill;
    }

    void FaceSliderToCamera()
    {
        if (sliderWorldObject != null && Camera.main != null)
        {
            sliderWorldObject.transform.forward = -Camera.main.transform.forward;
        }
    }
    private void ShowNoStaminaMessage()
    {
        if (noStaminaPrefab != null)
        {
            if (currentPopup != null)
            {
                Destroy(currentPopup.gameObject);
            }

            CanvasPopUp newPopup = Instantiate(noStaminaPrefab, noStaminaPrefab.transform.parent);
            newPopup.transform.position = transform.position + Vector3.up * 2f;
            newPopup.useOrbit = false; 
            string mensagem = "<b><size=50><color=#FFD700>Sem Stamina!</color></size></b>";
            newPopup.ShowCustomMessage(mensagem);

            currentPopup = newPopup;
        }
    }
}