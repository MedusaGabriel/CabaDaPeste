using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]

    public string dashParam = "Dash";
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
    private PlayerTarget _playerTarget;
    private PlayerStatus playerStatus;


    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        _rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        _playerAttackMelee = GetComponent<PlayerAttackMelee>();
        _playerTarget = GetComponent<PlayerTarget>();

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

        if (!_playerTarget.IsTargeting)
        {
            FaceSliderToCamera();
        }

        UpdateCooldownUI();
    }

    void LateUpdate()
    {
        if (sliderWorldObject != null)
        {
            sliderWorldObject.transform.position = transform.position + sliderOffset;
        }
    }

    void TryDash()
    {
        if (!isDashing && Time.time > lastDashTime + playerStatus.dashCooldown)
        {
            StartCoroutine(DashRoutine());
        }
    }
    IEnumerator DashRoutine()
    {
        isDashing = true;
        lastDashTime = Time.time;
        cooldownSlider.gameObject.SetActive(true);
        cooldownSlider.value = 0f;

        float dashValue = (_playerTarget != null && _playerTarget.IsTargeting) ? 2f : 1f;
        animator.SetFloat(dashParam, dashValue);
        animator.SetTrigger(dashTrigger);

        Vector3 dashDirection = (dashValue == 2f) ? -transform.forward : transform.forward;

        float startTime = Time.time;
        while (Time.time < startTime + playerStatus.dashTime)
        {
            _rigidbody.linearVelocity = dashDirection * playerStatus.dashSpeed * dashValue;
            yield return null;
        }

        _rigidbody.linearVelocity = Vector3.zero;
        isDashing = false;
    }

    void UpdateCooldownUI()
    {
        if (cooldownSlider == null) return;

        float progress = Mathf.Clamp01((Time.time - lastDashTime) / playerStatus.dashCooldown);
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
