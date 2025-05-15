using UnityEngine;
using PlayerInputS;
using Cinemachine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    /// testando fds
    [Header("Player Movement")]
    public float RotationSmoothTime = 0.1f;
    public float SpeedChangeRate = 15.0f;

    private float _speed;
    // private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private Vector3 _moveDirection;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private int _animIDSpeed;
    private int _animIDMotionSpeed;
    private bool _hasAnimator;
    private PlayerInputSystem _input;
    private PlayerAttackMelee _playerAttack;
    private PlayerStatus playerStatus;
    private PlayerAudioManager _audioManager;
    private bool wasWalking = false;
    private bool wasRunning = false;
    public CinemachineFreeLook freeLookCamera;
    private Vector3 _lastPosition;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _hasAnimator = TryGetComponent(out _animator);
        AssignAnimationIDs();

        _input = GetComponent<PlayerInputSystem>();
        _lastPosition = transform.position;

        _audioManager = GetComponent<PlayerAudioManager>();
        _playerAttack = GetComponent<PlayerAttackMelee>();
        playerStatus = GetComponent<PlayerStatus>();
        Physics.IgnoreLayerCollision(
        LayerMask.NameToLayer("Player"),
        LayerMask.NameToLayer("Enemy"),
        false
    );

    }

    private void Update()
    {
        Rotate();
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void FixedUpdate()
    {
        Move();
        HandleFootstepAudio();
    }
    private void HandleFootstepAudio()
    {
        bool isSprinting = _input.sprint;
        Vector2 input = _input.move;
        bool isInputPressed = input.x != 0 || input.y != 0;

        // Verifica se a posição mudou significativamente
        bool isActuallyMoving = Vector3.Distance(transform.position, _lastPosition) > 0.01f && isInputPressed;

        if (_audioManager == null) return;

        if (isActuallyMoving && !isSprinting)
        {
            if (!wasWalking)
            {
                _audioManager.PlayWalkLoop();
                wasWalking = true;
                wasRunning = false;
            }
        }
        else
        {
            if (wasWalking)
            {
                _audioManager.StopWalkLoop();
                wasWalking = false;
            }
        }

        if (isActuallyMoving && isSprinting)
        {
            if (!wasRunning)
            {
                _audioManager.PlayRunLoop();
                wasRunning = true;
                wasWalking = false;
            }
        }
        else
        {
            if (wasRunning)
            {
                _audioManager.StopRunLoop();
                wasRunning = false;
            }
        }

        _lastPosition = transform.position;
    }
    private void Move()
    {
        if (_playerAttack != null && !_playerAttack.CanMove)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            return;
        }

        Vector2 input = _input.move;
        float moveX = input.x;
        float moveZ = input.y;

        bool isSprinting = _input.sprint;
        float targetSpeed = isSprinting ? playerStatus.sprintSpeed : playerStatus.moveSpeed;
        if (moveX == 0 && moveZ == 0)
        {
            targetSpeed = 0.0f;
            _input.SprintInput(false);
        }

        _speed = Mathf.Lerp(_speed, targetSpeed, Time.fixedDeltaTime * SpeedChangeRate);

        if (targetSpeed == 0f && _speed < 0.01f)
        {
            _speed = 0f;
        }

        // Direção baseada na câmera
        Vector3 camForward = freeLookCamera.transform.forward;
        Vector3 camRight = freeLookCamera.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * moveZ + camRight * moveX;
        moveDir.Normalize();

        Vector3 movement = moveDir * _speed;

        _rigidbody.MovePosition(_rigidbody.position + movement * Time.fixedDeltaTime);

        if (_hasAnimator)
        {
            float motionMagnitude = (moveX == 0 && moveZ == 0) ? 0f : moveDir.magnitude;
            _animator.SetFloat(_animIDSpeed, _speed);
            _animator.SetFloat(_animIDMotionSpeed, motionMagnitude);
        }
    }


    private void Rotate()
    {
        if (_playerAttack != null && !_playerAttack.CanMove)
        {
            return;
        }
        Vector2 input = _input.move;
        float moveX = input.x;
        float moveZ = input.y;

        if (moveX != 0 || moveZ != 0)
        {
            Vector3 camForward = freeLookCamera.transform.forward;
            Vector3 camRight = freeLookCamera.transform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = camForward * moveZ + camRight * moveX;
            moveDir.Normalize();

            if (moveDir.sqrMagnitude > 0.0f)
            {
                float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
                float smoothAngle = Mathf.SmoothDampAngle(
                    transform.eulerAngles.y,
                    targetAngle,
                    ref _rotationVelocity,
                    RotationSmoothTime
                );
                transform.rotation = Quaternion.Euler(0.0f, smoothAngle, 0.0f);
            }
        }
    }

}