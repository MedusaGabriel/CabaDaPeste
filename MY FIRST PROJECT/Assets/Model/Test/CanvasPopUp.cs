using UnityEngine;
using TMPro;
using System.Collections;

public class CanvasPopUp : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public Transform playerTransform;
    public float fadeDuration = 1.5f;
    private Animator popupAnimator; 

    void Start()
    {
        popupText.gameObject.SetActive(false);
        popupAnimator = popupText.GetComponent<Animator>(); 
    }

    void Update()
    {
        if (playerTransform != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(playerTransform.position);
            popupText.rectTransform.position = screenPos;
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            int randomNumber = Random.Range(0, 100);
            popupText.text = randomNumber.ToString();
            popupText.gameObject.SetActive(true);
            if (popupAnimator != null)
            {
                popupAnimator.Play("Pop-Up", -1, 0f); 
            }
            StopAllCoroutines(); 
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        popupText.alpha = 1f;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            popupText.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }
        popupText.alpha = 0f;
        popupText.gameObject.SetActive(false);
    }
}