using UnityEngine;
using StarterAssets;
using System.Collections;
using UnityEngine.UI;

public class DashController : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashSpeed = 10f;
    public float dashTime = 0.5f;
    public float dashCooldown = 2f;
    public string dashAnimationTrigger = "Dash";
    public string dashBackwardTrigger = "DashBackward"; 

    [Header("Cooldown UI")]
    public Slider cooldownSlider;
    public Vector3 sliderOffset = new Vector3(0, 2f, 0);

    private ThirdPersonController moveScript;
    private CharacterController characterController;
    private Animator animator;
    private float lastDashTime = -Mathf.Infinity;
    private bool isDashing = false;
    private GameObject sliderWorldObject;

    private bool isLocked;

    void Start()
    {
        moveScript = GetComponent<ThirdPersonController>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (cooldownSlider != null)
        {
            cooldownSlider.gameObject.SetActive(false);
            CreateWorldSpaceSlider();
        }
        else
        {
            Debug.LogError("Atribua um Slider UI no Inspector!");
        }
    }

    void Update()
    {
        isLocked = animator.GetBool("IsLocked");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryDash();
        }
        
        UpdateCooldownUI();
        FaceSliderToCamera();
    }

    void TryDash()
    {
        if (!isDashing && Time.time > lastDashTime + dashCooldown)
        {
            StartCoroutine(DashRoutine());
        }
    }

    IEnumerator DashRoutine()
    {
        isDashing = true;
        lastDashTime = Time.time;
        cooldownSlider.gameObject.SetActive(true);
        cooldownSlider.value = 0;


        Vector3 dashDirection;
        
        if (isLocked) 
        {
            dashDirection = -transform.forward;
            animator.SetTrigger(dashBackwardTrigger);
        } 
        else 
        {
            dashDirection = transform.forward;
            animator.SetTrigger(dashAnimationTrigger);
        }

        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            characterController.Move(dashDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }
        
        isDashing = false;
    }

    void UpdateCooldownUI()
    {
        if (cooldownSlider == null) return;

        float progress = Mathf.Clamp01((Time.time - lastDashTime) / dashCooldown);
        cooldownSlider.value = progress;

        cooldownSlider.gameObject.SetActive(progress < 1f);
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
        sliderWorldObject = new GameObject("CooldownSliderWorld");
        sliderWorldObject.transform.SetParent(transform);
        sliderWorldObject.transform.localPosition = sliderOffset;

        Canvas canvas = sliderWorldObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(2f, 0.5f);

        Slider worldSlider = Instantiate(cooldownSlider, sliderWorldObject.transform);
        worldSlider.transform.localPosition = Vector3.zero;
        worldSlider.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        
        cooldownSlider = worldSlider;
        cooldownSlider.value = 0;
    }
}
