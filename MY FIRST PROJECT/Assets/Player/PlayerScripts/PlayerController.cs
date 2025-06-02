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
    private Vector3 lastPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
    }

    private void Update()
    {
        ComboAttack combo = GetComponent<ComboAttack>();
        if (combo != null && combo.atacando)
        {
            moveDirection = Vector3.zero;
            currentSpeed = 0f;
            if (anim != null)
                anim.SetFloat("Speed", 0f);
            return;
        }

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
        MovePlayer();
        RotatePlayer(moveDirection);
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


}