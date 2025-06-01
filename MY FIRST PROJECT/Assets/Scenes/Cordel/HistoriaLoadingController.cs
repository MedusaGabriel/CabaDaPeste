using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement; // Adicione isto
using UnityEngine.Video; // Adicione isto para controlar o vídeo

public class HistoriaLoadingController : MonoBehaviour
{
    public Image fadeImage;           // Arraste o painel preto aqui no Inspector
    public float fadeDuration = 1.0f; // Duração do fade-out em segundos
    public VideoPlayer videoPlayer;   // Arraste o VideoPlayer aqui no Inspector

    private float timer = 0f;
    private bool canProceed = false;
    private bool sceneLoaded = false;

    void Start()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            StartCoroutine(FadeOut());
        }

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (!canProceed && timer >= 7)
        {
            canProceed = true;
        }

        if (canProceed && !sceneLoaded && Input.anyKeyDown)
        {
            LoadFase1();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (!sceneLoaded)
        {
            LoadFase1();
        }
    }

    void LoadFase1()
    {
        sceneLoaded = true;
        SceneManager.LoadScene("Level 1");
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
        c.a = 0f;
        fadeImage.color = c;
        fadeImage.gameObject.SetActive(false);
    }
}