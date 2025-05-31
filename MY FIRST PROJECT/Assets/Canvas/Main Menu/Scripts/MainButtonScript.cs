using UnityEngine;
using UnityEngine.EventSystems;

public class MainButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    public float scaleFactor = 1.1f;
    public float animationSpeed = 10f;

    private bool isHovered = false;

    [Header("Som de Hover")]
    public AudioClip hoverSound;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        Vector3 targetScale = isHovered ? originalScale * scaleFactor : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        TocarSomHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    private void TocarSomHover()
    {
        if (hoverSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(hoverSound);
        }
    }
}