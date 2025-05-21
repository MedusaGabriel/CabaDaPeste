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
    private PlayerController _playerController;
    private Vector3 dashDirection;
    private float dashSpeed;
    private PlayerStamina playerStamina;



    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        playerStamina = GetComponent<PlayerStamina>();
        _rigidbody = GetComponent<Rigidbody>();
        _playerController = GetComponent<PlayerController>();
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
            if (playerStamina != null && playerStamina.TryConsumeStamina())
            {
                StartCoroutine(DashRoutine());
            }
            else
            {
                Debug.Log("Sem stamina para dash!");
            }
        }
    }
    IEnumerator DashRoutine()
    {
        isDashing = true;
        lastDashTime = Time.time;
        cooldownSlider.gameObject.SetActive(true);
        cooldownSlider.value = 0f;

        if (_playerController != null)
            _playerController.enabled = false;

        float dashValue = (_playerTarget != null && _playerTarget.IsTargeting) ? 2f : 1f;
        animator.SetFloat(dashParam, dashValue);
        animator.SetTrigger(dashTrigger);

        dashDirection = (dashValue == 2f) ? -transform.forward : transform.forward;
        float dashDistance = playerStatus.dashDistance * dashValue;
        float dashTime = playerStatus.dashTime;

        Vector3 start = transform.position;
        Vector3 end = start + dashDirection * dashDistance;

        float elapsed = 0f;
        bool originalGravity = _rigidbody.useGravity;
        _rigidbody.useGravity = false;

        while (elapsed < dashTime)
        {
            float t = elapsed / dashTime;
            _rigidbody.MovePosition(Vector3.Lerp(start, end, t));
            elapsed += Time.deltaTime;
            yield return null;
        }
        _rigidbody.MovePosition(end);

        _rigidbody.useGravity = originalGravity;

        isDashing = false;
    }

    public void ApplyDashImpulse()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.AddForce(dashDirection * dashSpeed, ForceMode.VelocityChange);
    }
    public void EnablePlayerController()
    {
        if (_playerController != null)
            _playerController.enabled = true;
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
