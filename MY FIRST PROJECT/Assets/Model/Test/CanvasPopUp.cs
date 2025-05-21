using UnityEngine;
using TMPro;
using System.Collections;

public class CanvasPopUp : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public Transform playerTransform;
    [Header("Órbita do Pop-Up")]
    [Range(0.1f, 2f)]
    public float minOrbitRadius = 0.3f;
    [Range(0.1f, 2f)]
    public float maxOrbitRadius = 0.7f;
    [Range(0.5f, 5f)]
    public float verticalOffset = 2f;
    [Range(10f, 360f)]
    public float orbitSpeed = 90f;
    [Header("Duração do Fade")]

    [Header("Comportamento")]
    public bool useOrbit = true;
    public float fadeDuration = 1.5f;
    public float animationVerticalOffset = -0.5f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float baseOrbitAngle;
    private float randomOrbitRadius;

    private Animator popupAnimator;
    private Coroutine fadeCoroutine;
    private float minFadeDuration = 0.2f;
    private bool isFadingOut = false;

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
                Debug.LogWarning("[CanvasPopUp] Nenhum objeto com a tag 'Player' foi encontrado na cena! Pop-up destruído.");
                Destroy(gameObject);
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
            if (useOrbit)
            {
                if (!isFadingOut)
                {
                    baseOrbitAngle += orbitSpeed * Time.deltaTime;
                }
                float rad = baseOrbitAngle * Mathf.Deg2Rad;
                Vector3 horizontalOffset = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * randomOrbitRadius;
                startPosition = playerTransform.position + Vector3.up * (verticalOffset + animationVerticalOffset) + horizontalOffset;
                endPosition = playerTransform.position + Vector3.up * verticalOffset + horizontalOffset;
            }
            else
            {
                startPosition = playerTransform.position + Vector3.up * (verticalOffset + animationVerticalOffset);
                endPosition = playerTransform.position + Vector3.up * verticalOffset;
            }
            transform.position = startPosition;

            if (Camera.main != null)
            {
                transform.LookAt(Camera.main.transform);
                transform.Rotate(0, 180f, 0);
            }
        }
    }

    public void ShowDamage(int damage)
    {
        popupText.text = damage.ToString();
        popupText.gameObject.SetActive(true);

        baseOrbitAngle = Random.Range(0f, 360f);
        randomOrbitRadius = Random.Range(minOrbitRadius, maxOrbitRadius);

        float currentAlpha = popupText.alpha;
        float newFadeDuration = Mathf.Max(minFadeDuration, fadeDuration * currentAlpha);

        if (popupAnimator != null)
        {
            popupAnimator.speed = fadeDuration / newFadeDuration;
            popupAnimator.Play("Pop-Up", -1, 0f);
        }

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOutAndMove(newFadeDuration));
    }

    IEnumerator FadeOutAndMove(float duration)
    {
        isFadingOut = true;
        popupText.alpha = 1f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            popupText.alpha = Mathf.Lerp(1f, 0f, t);
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }
        popupText.alpha = 0f;
        if (popupAnimator != null)
            popupAnimator.speed = 1f;

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        if (playerTransform == null) return;
        Gizmos.color = Color.green;
        DrawOrbitGizmo(playerTransform.position, verticalOffset, minOrbitRadius);
        Gizmos.color = Color.red;
        DrawOrbitGizmo(playerTransform.position, verticalOffset, maxOrbitRadius);
    }

    void DrawOrbitGizmo(Vector3 center, float yOffset, float radius)
    {
        int segments = 64;
        Vector3 prevPoint = center + Vector3.up * yOffset + new Vector3(radius, 0, 0);
        for (int i = 1; i <= segments; i++)
        {
            float angle = (float)i / segments * Mathf.PI * 2;
            Vector3 nextPoint = center + Vector3.up * yOffset + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
    public void ShowCustomMessage(string message)
    {
        popupText.text = message;
        popupText.gameObject.SetActive(true);

        baseOrbitAngle = Random.Range(0f, 360f);
        randomOrbitRadius = Random.Range(minOrbitRadius, maxOrbitRadius);

        float currentAlpha = popupText.alpha;
        float newFadeDuration = Mathf.Max(minFadeDuration, fadeDuration * currentAlpha);

        if (popupAnimator != null)
        {
            popupAnimator.speed = fadeDuration / newFadeDuration;
            popupAnimator.Play("Pop-Up", -1, 0f);
        }

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOutAndMove(newFadeDuration));
    }
}