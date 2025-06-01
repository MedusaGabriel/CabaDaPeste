using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class MainButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    public float scaleFactor = 1.1f;
    public float animationSpeed = 10f;

    private bool isHovered = false;

    [Header("Audio")]
    public AudioClip hoverSound;
    public AudioMixerGroup clickMixerGroup;

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
        if (hoverSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(hoverSound, clickMixerGroup);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}