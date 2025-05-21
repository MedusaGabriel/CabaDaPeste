using UnityEngine;
using PlayerInputS;

public class PlayerController : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private CameraInputFollow cameraInputFollow;
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private PlayerAudioManager audioManager;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool wasWalking = false;
    private bool wasRunning = false;
    private Vector3 lastPosition;
    private PlayerAttackMelee playerAttack;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
        playerAttack = GetComponent<PlayerAttackMelee>();
    }

    private void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDirection = cameraInputFollow.GetCameraRelativeDirection(input);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = isRunning ? playerStatus.sprintSpeed : playerStatus.moveSpeed;

        if (anim != null)
        {
            float animSpeed = 0f;
            if (input.magnitude == 0)
                animSpeed = 0f;
            else if (isRunning)
                animSpeed = playerStatus.sprintSpeed;
            else
                animSpeed = playerStatus.moveSpeed;

            anim.SetFloat("Speed", animSpeed);
        }

        currentSpeed = targetSpeed;
    }

    private float currentSpeed = 0f;

    private void FixedUpdate()
    {
        if (playerAttack != null && !playerAttack.CanMove)
        {
            rb.linearVelocity = Vector3.zero;
            if (wasWalking && audioManager != null)
            {
                audioManager.StopWalkLoop();
                wasWalking = false;
            }
            if (wasRunning && audioManager != null)
            {
                audioManager.StopRunLoop();
                wasRunning = false;
            }
            return;
        }
        MovePlayer();
        RotatePlayer(moveDirection);
        HandleFootstepAudio();
    }

    private void MovePlayer()
    {
        Vector3 velocity = moveDirection * currentSpeed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }

    private void RotatePlayer(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }

    private void HandleFootstepAudio()
    {
        if (playerAttack != null && !playerAttack.CanMove)
        {
            if (wasWalking)
            {
                audioManager.StopWalkLoop();
                wasWalking = false;
            }
            if (wasRunning)
            {
                audioManager.StopRunLoop();
                wasRunning = false;
            }
            return;
        }

        if (audioManager == null) return;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isInputPressed = input.x != 0 || input.y != 0;

        // Verifica se a posição mudou significativamente
        bool isActuallyMoving = Vector3.Distance(transform.position, lastPosition) > 0.01f && isInputPressed;

        if (isActuallyMoving && !isRunning)
        {
            if (!wasWalking)
            {
                audioManager.PlayWalkLoop();
                wasWalking = true;
                wasRunning = false;
            }
        }
        else
        {
            if (wasWalking)
            {
                audioManager.StopWalkLoop();
                wasWalking = false;
            }
        }

        if (isActuallyMoving && isRunning)
        {
            if (!wasRunning)
            {
                audioManager.PlayRunLoop();
                wasRunning = true;
                wasWalking = false;
            }
        }
        else
        {
            if (wasRunning)
            {
                audioManager.StopRunLoop();
                wasRunning = false;
            }
        }

        lastPosition = transform.position;
    }
}