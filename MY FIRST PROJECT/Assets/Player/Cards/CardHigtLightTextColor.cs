using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CardHigtLightTextColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text[] cardTexts;
    public Button cardButton;

    private Color normalColor;
    private Coroutine fadeCoroutine;

    void Start()
    {
        if (cardTexts.Length > 0)
            normalColor = cardTexts[0].color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartFade(cardButton.colors.highlightedColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartFade(normalColor);
    }

    private void StartFade(Color targetColor)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        float fadeDuration = cardButton.colors.fadeDuration;
        fadeCoroutine = StartCoroutine(FadeTextColor(targetColor, fadeDuration));
    }

    private IEnumerator FadeTextColor(Color targetColor, float duration)
    {
        float time = 0f;
        Color[] startColors = new Color[cardTexts.Length];
        for (int i = 0; i < cardTexts.Length; i++)
            startColors[i] = cardTexts[i].color;

        while (time < duration)
        {
            float t = time / duration;
            for (int i = 0; i < cardTexts.Length; i++)
                cardTexts[i].color = Color.Lerp(startColors[i], targetColor, t);

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        for (int i = 0; i < cardTexts.Length; i++)
            cardTexts[i].color = targetColor;
    }
}