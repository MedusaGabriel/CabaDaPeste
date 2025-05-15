using UnityEngine;
using Cinemachine;
using PlayerInputS;

public class CameraInputFollow : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    public float cameraFollowSpeed = 2f;

    private PlayerInputSystem _input;
    private Transform _playerTransform;

    void Start()
    {
        _input = GetComponent<PlayerInputSystem>();
        _playerTransform = transform;
    }

    void Update()
    {
        if (freeLookCamera == null || _input == null) return;

        Vector2 moveInput = _input.move;

        if (moveInput != Vector2.zero)
        {
            float targetAngle = _playerTransform.eulerAngles.y;
            float currentAngle = freeLookCamera.m_XAxis.Value;
            freeLookCamera.m_XAxis.Value = Mathf.LerpAngle(currentAngle, targetAngle, cameraFollowSpeed * Time.deltaTime);
        }
    }
}