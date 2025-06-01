using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{

    public GameObject mainMenuCanvas;
    public GameObject creditosCanvas;
    public GameObject opcoesCanvas;

    public Image fadeImage;
    public float fadeDuration = 1f;

    [Header("Áudio")]
    public AudioClip clickSound;
    public AudioMixerGroup clickMixerGroup;

    void Start()
    {
        // Não precisa mais de AudioSource aqui
    }

    private void TocarSomDeClique()
    {
        if (clickSound != null && AudioManager.Instance != null)
            AudioManager.Instance.PlaySound(clickSound, clickMixerGroup);
    }

    public void AbrirJogo()
    {
        TocarSomDeClique();
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeAndLoadScene());
    }
    private IEnumerator FadeAndLoadScene()
    {
        float t = 0f;
        Color c = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
        c.a = 1;
        fadeImage.color = c;
        SceneManager.LoadSceneAsync("CordelHistory");
    }

    public void AbrirOpcoes()
    {
        TocarSomDeClique();
        mainMenuCanvas.SetActive(false);
        opcoesCanvas.SetActive(true);
    }
    public void AbrirCreditos()
    {
        TocarSomDeClique();
        mainMenuCanvas.SetActive(false);
        creditosCanvas.SetActive(true);
    }
    public void VoltarAoMenu()
    {
        TocarSomDeClique();
        creditosCanvas.SetActive(false);
        opcoesCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }
    // Chama quando clica em "Sair"
    public void SairDoJogo()
    {
        TocarSomDeClique();
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }
}