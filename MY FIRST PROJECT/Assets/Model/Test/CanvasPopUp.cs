using UnityEngine;
using TMPro;
using System.Collections;

public class CanvasPopUp : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public Transform playerTransform;
    public float fadeDuration = 1f;
    private int? lastDamage = null;
    private Animator popupAnimator;
    private Coroutine fadeCoroutine;

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("[CanvasPopUp] Nenhum objeto com a tag 'Player' foi encontrado na cena! Pop-up desativado.");
                gameObject.SetActive(false);
                return;
            }
        }

        popupText.gameObject.SetActive(true);
        popupAnimator = popupText.GetComponent<Animator>();
    }

    void Update()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + Vector3.up * currentYOffset;
            if (Camera.main != null)
            {
                transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                                 Camera.main.transform.rotation * Vector3.up);
            }
        }
    }

    private float currentYOffset = 2f;

    public void ShowDamage(int damage)
    {
        if (lastDamage.HasValue)
            popupText.text = $"({lastDamage.Value}  {damage})";
        else
            popupText.text = damage.ToString();

        lastDamage = damage; 

        popupText.gameObject.SetActive(true);

        float newFadeDuration = fadeDuration;

        if (popupAnimator != null)
        {
            popupAnimator.speed = 1f;
            popupAnimator.Play("Pop-Up", -1, 0f);
        }

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOutAndDeactivate(newFadeDuration));
    }

    IEnumerator FadeOutAndDeactivate(float duration)
    {
        popupText.alpha = 1f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            popupText.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            yield return null;
        }
        popupText.alpha = 0f;
        if (popupAnimator != null)
            popupAnimator.speed = 1f;

        popupText.gameObject.SetActive(false);
        gameObject.SetActive(false);

        lastDamage = null;
    }
}