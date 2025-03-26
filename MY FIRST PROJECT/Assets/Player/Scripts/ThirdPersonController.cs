using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        public float MoveSpeed = 2.0f;
        public float SprintSpeed = 5.335f;
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;
        public float SpeedChangeRate = 10.0f;

        [Header("Dash")]
        public float DashDistance = 5f;
        public float DashDuration = 0.2f;
        public float DashCooldown = 1f;
        public float DashSpeedMultiplier = 2f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;
        public AudioClip DashAudioClip;

        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _dashCooldownDelta;
        private bool _isDashing;
        private float _dashTime;
        private Vector3 _dashDirection;

        private int _animIDSpeed;
        private int _animIDMotionSpeed;
        private int _animIDDashForward;
        private int _animIDDashBackward;
        private int _animIDDashLeft;
        private int _animIDDashRight;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;
        private AudioSource _audioSource;

        private const float _threshold = 0.01f;
        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
                return false;
#endif
            }
        }

        private void Awake()
        {
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
            
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        private void Start()
        {
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#endif

            AssignAnimationIDs();
            _dashCooldownDelta = DashCooldown;
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);
            Dash();
            Move();
            Cooldowns();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDDashForward = Animator.StringToHash("DashForward");
            _animIDDashBackward = Animator.StringToHash("DashBackward");
            _animIDDashLeft = Animator.StringToHash("DashLeft");
            _animIDDashRight = Animator.StringToHash("DashRight");
        }

        private void Move()
        {
            if (_isDashing) return;

            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime));

            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void Dash()
        {
            if (_input.jump && _dashCooldownDelta <= 0f && !_isDashing)
            {
                Vector3 dashDirection;
                
                if (_input.move == Vector2.zero)
                {
                    dashDirection = transform.forward;
                }
                else
                {
                    dashDirection = new Vector3(_input.move.x, 0, _input.move.y).normalized;
                    dashDirection = transform.TransformDirection(dashDirection);
                }

                _dashDirection = dashDirection;
                _isDashing = true;
                _dashTime = 0f;
                _dashCooldownDelta = DashCooldown;

                if (_hasAnimator)
                {
                    float angle = Vector3.SignedAngle(transform.forward, dashDirection, Vector3.up);

                    if (angle > -45f && angle <= 45f)
                    {
                        _animator.SetTrigger(_animIDDashForward);
                    }
                    else if (angle > 45f && angle <= 135f)
                    {
                        _animator.SetTrigger(_animIDDashRight);
                    }
                    else if (angle > -135f && angle <= -45f)
                    {
                        _animator.SetTrigger(_animIDDashLeft);
                    }
                    else
                    {
                        _animator.SetTrigger(_animIDDashBackward);
                    }
                }

                if (DashAudioClip != null)
                {
                    _audioSource.PlayOneShot(DashAudioClip);
                }

                _input.jump = false;
            }

            if (_isDashing)
            {
                _dashTime += Time.deltaTime;

                if (_dashTime < DashDuration)
                {
                    float dashSpeed = DashDistance / DashDuration * DashSpeedMultiplier;
                    _controller.Move(_dashDirection * dashSpeed * Time.deltaTime);
                }
                else
                {
                    _isDashing = false;
                }
            }
        }

        private void Cooldowns()
        {
            if (_dashCooldownDelta > 0f)
            {
                _dashCooldownDelta -= Time.deltaTime;
            }
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
}