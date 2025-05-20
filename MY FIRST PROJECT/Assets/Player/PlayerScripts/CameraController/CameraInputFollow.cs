using UnityEngine;
using Cinemachine;
using PlayerInputS;

public class CameraInputFollow : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    public float playerRotateSpeed = 10f;
    public float cameraRotateSpeed = 8f;

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
            float cameraY = freeLookCamera.State.RawOrientation.eulerAngles.y;
            float playerY = _playerTransform.eulerAngles.y;

            if (Mathf.Abs(Mathf.DeltaAngle(playerY, cameraY)) > 1f)
            {
                float newY = Mathf.LerpAngle(playerY, cameraY, playerRotateSpeed * Time.deltaTime);
                _playerTransform.rotation = Quaternion.Euler(0, newY, 0);
                float cameraCurrentY = freeLookCamera.m_XAxis.Value;
                float cameraTargetY = newY;
                float smoothCameraY = Mathf.LerpAngle(cameraCurrentY, cameraTargetY, playerRotateSpeed * Time.deltaTime);
                freeLookCamera.m_XAxis.Value = smoothCameraY;
            }
        }
    }
}