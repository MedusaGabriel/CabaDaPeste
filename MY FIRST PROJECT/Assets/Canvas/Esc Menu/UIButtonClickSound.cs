using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class UIButtonClickSound : MonoBehaviour, IPointerClickHandler
{
    [Header("Som de Clique")]
    public AudioClip clickSound;
    public AudioMixerGroup clickMixerGroup;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null && AudioManager.Instance != null)
            AudioManager.Instance.PlaySound(clickSound, clickMixerGroup);
    }
}
