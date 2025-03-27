using UnityEngine;
using System.Collections;
using StarterAssets;

public class DashController : MonoBehaviour
{
    private ThirdPersonController moveScript;

    public float dashSpeed;
    public float dashTime;

    void Start()
    {
        moveScript = GetComponent<ThirdPersonController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            moveScript.Controller.Move(moveScript.MoveDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
