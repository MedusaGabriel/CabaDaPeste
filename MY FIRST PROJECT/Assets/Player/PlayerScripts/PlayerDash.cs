using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    public string dashParam = "Dash";
    public string dashTrigger = "IsDash";
    private Rigidbody _rigidbody;
    private Animator animator;
    private float lastDashTime = -Mathf.Infinity;
    private bool isDashing = false;
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
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryDash();
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
                Debug.Log("Stamina insuficiente para realizar o Dash.");
            }
        }
    }

    System.Collections.IEnumerator DashRoutine()
    {
        isDashing = true;
        lastDashTime = Time.time;

        if (_playerController != null)
            _playerController.enabled = false;

        float dashValue = 1f;
        animator.SetFloat(dashParam, dashValue);
        animator.SetTrigger(dashTrigger);

        dashDirection = transform.forward;
        float dashDistance = playerStatus.dashDistance;
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

    public void EnablePlayerController()
    {
        if (_playerController != null)
            _playerController.enabled = true;
    }

    public void ApplyDashImpulse()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.AddForce(dashDirection * dashSpeed, ForceMode.VelocityChange);
    }
}