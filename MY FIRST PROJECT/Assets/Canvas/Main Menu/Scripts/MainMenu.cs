using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject creditosCanvas;
    public GameObject opcoesCanvas;

    [Header("Áudio")]
    public AudioClip clickSound;

    void Start()
    {
        // Não precisa mais de AudioSource aqui
    }

    private void TocarSomDeClique()
    {
        if (clickSound != null && AudioManager.Instance != null)
            AudioManager.Instance.PlaySound(clickSound);
    }

    public void AbrirJogo()
    {
        TocarSomDeClique();
        SceneManager.LoadSceneAsync(1);
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

    public void SairDoJogo()
    {
        TocarSomDeClique();
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }
}