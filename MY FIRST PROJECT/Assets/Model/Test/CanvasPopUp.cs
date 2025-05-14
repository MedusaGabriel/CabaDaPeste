using UnityEngine;
using TMPro;
using System.Collections;

public class CanvasPopUp : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public Transform playerTransform;
    public float fadeDuration = 1.5f;
    public float verticalOffset = 2f;
    private Animator popupAnimator;

    private Coroutine fadeCoroutine;
    private float minFadeDuration = 0.2f;


    void Start()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
                Debug.Log($"[CanvasPopUp] Player encontrado! Posição atual: {playerTransform.position}");
            }
            else
            {
                Debug.LogWarning("[CanvasPopUp] Nenhum objeto com a tag 'Player' foi encontrado na cena!");
            }
        }

        popupText.gameObject.SetActive(true);
        popupAnimator = popupText.GetComponent<Animator>();
    }

    void Update()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + Vector3.up * verticalOffset;

            if (Camera.main != null)
            {
                transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                                 Camera.main.transform.rotation * Vector3.up);
            }
        }
    }
    public void ShowDamage(int damage)
    {
        popupText.text = damage.ToString();
        popupText.gameObject.SetActive(true);

        float currentAlpha = popupText.alpha;
        float newFadeDuration = Mathf.Max(minFadeDuration, fadeDuration * currentAlpha);

        if (popupAnimator != null)
        {
            popupAnimator.speed = fadeDuration / newFadeDuration;
            popupAnimator.Play("Pop-Up", -1, 0f);
        }

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOut(newFadeDuration));
    }

    IEnumerator FadeOut(float duration)
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

        Destroy(gameObject);
    }
}