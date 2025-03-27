using UnityEngine;
using StarterAssets;
using System.Collections;

public class DashController : MonoBehaviour
{
    private ThirdPersonController moveScript;
    private Animator animator;
    private CharacterController characterController;

    public float dashSpeed;
    public float dashTime;

    private bool isDashing = false;

    void Start()
    {
        moveScript = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Verifica se o jogador pressionou a tecla de dash e ainda não está dashing
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            Vector3 dashDirection = transform.forward; 

            // Se a tecla "S" estiver pressionada, faz o dash para trás
            if (Input.GetKey(KeyCode.S)) 
            {
                dashDirection = -transform.forward;
            }

            // Começa a Coroutine do Dash
            StartCoroutine(Dash(dashDirection));
        }
    }

    IEnumerator Dash(Vector3 direction)
    {
        isDashing = true;

        if (direction == transform.forward)
        {
            animator.SetTrigger("DashForward");
        }

        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            characterController.Move(direction * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
    }

}
