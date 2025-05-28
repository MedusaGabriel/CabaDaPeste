using UnityEngine;
using Cinemachine;

namespace PlayerInputS
{
    public class CameraInputFollow : MonoBehaviour
    {
        [Header("Referências")]
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        public Vector3 GetCameraRelativeDirection(Vector2 input)
        {
            if (virtualCamera == null)
            {
                Debug.LogWarning("VirtualCamera não atribuída!");
                return Vector3.zero;
            }

            Transform camTransform = virtualCamera.transform;

            Vector3 camForward = camTransform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = camTransform.right;
            camRight.y = 0;
            camRight.Normalize();

            Vector3 moveDir = (camForward * input.y + camRight * input.x).normalized;
            return moveDir;
        }
    }
}