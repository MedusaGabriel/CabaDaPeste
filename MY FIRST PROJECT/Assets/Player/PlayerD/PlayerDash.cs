using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashSpeed = 10f;
    public float dashTime = 0.5f;
    public float dashCooldown = 2f;

    // Parâmetro único para o Blend Tree
    //  1 => dash para frente; -1 => dash para trás
    public string dashParam = "Dash";
    // Trigger que chama o dash no Animator
    public string dashTrigger = "IsDash";

    [Header("Cooldown UI")]
    public Slider cooldownSlider;
    public Vector3 sliderOffset = new Vector3(0, 2f, 0);

    private Rigidbody _rigidbody;
    private Animator animator;
    private float lastDashTime = -Mathf.Infinity;
    private bool isDashing = false;
    private GameObject sliderWorldObject;
    private PlayerAttackMelee _playerAttackMelee;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        _playerAttackMelee = GetComponent<PlayerAttackMelee>();

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_playerAttackMelee != null && _playerAttackMelee.IsAttacking)
            {
                return;
            }
            else
            {
                TryDash();
            }
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

        float dashValue = 1f;
        animator.SetFloat(dashParam, dashValue);
        animator.SetTrigger(dashTrigger);

        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            _rigidbody.linearVelocity = transform.forward * (dashSpeed * dashValue);
            yield return null;
        }

        _rigidbody.linearVelocity = Vector3.zero;
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