using UnityEngine;

public class IsoCameraController : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 5f;

    private float rotationX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");

        if (mouseX != 0) 
        {
            rotationX += mouseX * rotationSpeed;
            transform.position = player.position + Quaternion.Euler(0, rotationX, 0) * new Vector3(0, 5, -5);
            transform.LookAt(player.position);
        }
    }
}
