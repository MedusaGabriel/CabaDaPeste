using UnityEngine;
using PlayerInputS;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    public float MoveSpeed = 5.0f;
    public float SprintSpeed = 8.0f;
    public float RotationSmoothTime = 0.1f;
    public float SpeedChangeRate = 15.0f;

    private float _speed;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private Vector3 _moveDirection;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private int _animIDSpeed;
    private int _animIDMotionSpeed;
    private bool _hasAnimator;
    private PlayerInputSystem _input;
    private PlayerAttackMelee _playerAttack;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _hasAnimator = TryGetComponent(out _animator);
        AssignAnimationIDs();

        _input = GetComponent<PlayerInputSystem>();

        _playerAttack = GetComponent<PlayerAttackMelee>();

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

    private void Rotate()
    {

        if (_playerAttack != null && _playerAttack.IsAttacking)
        {
            return;
        }
        Vector2 input = _input.move;
        float moveX = input.x;
        float moveZ = input.y;

        Vector3 rawDirection = new Vector3(-moveX, 0, -moveZ).normalized;
        if (rawDirection != Vector3.zero)
        {
            _targetRotation = Mathf.Atan2(rawDirection.x, rawDirection.z) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                _targetRotation,
                ref _rotationVelocity,
                RotationSmoothTime
            );
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
    }

    private void FixedUpdate()
{
    Move();
}

private void Move()
{
    if (_playerAttack != null && _playerAttack.IsAttacking)
    {
        return;
    }

    Vector2 input = _input.move;
    float moveX = input.x;
    float moveZ = input.y;

    bool isSprinting = _input.sprint;
    float targetSpeed = isSprinting ? SprintSpeed : MoveSpeed;

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

    Vector3 rawDirection = new Vector3(-moveX, 0, -moveZ).normalized;
    Vector3 movement = rawDirection * _speed;

    _rigidbody.MovePosition(_rigidbody.position + movement * Time.fixedDeltaTime);

    if (_hasAnimator)
    {
        float motionMagnitude = (moveX == 0 && moveZ == 0) ? 0f : rawDirection.magnitude;
        _animator.SetFloat(_animIDSpeed, _speed);
        _animator.SetFloat(_animIDMotionSpeed, motionMagnitude);
    }
}
}