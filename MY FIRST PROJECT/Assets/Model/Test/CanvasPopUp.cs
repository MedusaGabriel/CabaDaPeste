using UnityEngine;
using TMPro;
using System.Collections;

public class CanvasPopUp : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public Transform playerTransform;
    [Header("Comportamento")]
    public float fadeDuration = 1.5f;
    public float minYOffset = 2f;
    public float maxYOffset = 5f;

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
        // Mantém o popup acima do player
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
        popupText.text = damage.ToString();
        popupText.gameObject.SetActive(true);

        // Offset Y aleatório
        currentYOffset = Random.Range(minYOffset, maxYOffset);

        float currentAlpha = popupText.alpha;
        float newFadeDuration = Mathf.Max(minFadeDuration, fadeDuration * currentAlpha);

        if (popupAnimator != null)
        {
            popupAnimator.speed = fadeDuration / newFadeDuration;
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
    }
}