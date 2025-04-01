using UnityEngine;
using PlayerInputS; // Namespace correto
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
    private PlayerInputSystem _input; // Corrigido para usar a classe PlayerInputSystem

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        _hasAnimator = TryGetComponent(out _animator);
        AssignAnimationIDs();

        _input = GetComponent<PlayerInputSystem>();
    }

    private void Update()
    {
        Move();
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void Move()
    {
        Vector2 input = _input.move;
        float moveX = input.x;
        float moveZ = input.y;

        bool isSprinting = _input.sprint;
        float targetSpeed = isSprinting ? SprintSpeed : MoveSpeed;

        // Se não há input, velocidade = 0
        if (moveX == 0 && moveZ == 0)
        {
            targetSpeed = 0.0f;
        }

        // Calcula direção sem depender de rotação
        Vector3 rawDirection = new Vector3(-moveX, 0, -moveZ).normalized;

        // Rotação suave (somente visual)
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

        // Interpolação de velocidade
        _speed = Mathf.Lerp(_speed, targetSpeed, Time.deltaTime * SpeedChangeRate);

        // Se a velocidade está próxima de zero e não há input, força _speed = 0
        if (targetSpeed == 0f && _speed < 0.01f)
        {
            _speed = 0f;
        }

        // Movimento imediato no corpo físico
        Vector3 movement = rawDirection * (_speed * Time.deltaTime);
        _rigidbody.MovePosition(transform.position + movement);

        // Atualiza animações
        if (_hasAnimator)
        {
            float motionMagnitude = (moveX == 0 && moveZ == 0) ? 0f : rawDirection.magnitude;
            _animator.SetFloat(_animIDSpeed, _speed);
            _animator.SetFloat(_animIDMotionSpeed, motionMagnitude);
        }
    }
}