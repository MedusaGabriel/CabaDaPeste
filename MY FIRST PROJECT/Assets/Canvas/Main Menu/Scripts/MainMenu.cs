using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject mainMenuCanvas;
    public GameObject creditosCanvas;
    public GameObject opcoesCanvas;

    // Chama quando clica em "Jogar"
    public void AbrirJogo()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void AbrirOpcoes()
    {
        mainMenuCanvas.SetActive(false);
        opcoesCanvas.SetActive(true);
    }
    public void AbrirCreditos()
    {
        mainMenuCanvas.SetActive(false);
        creditosCanvas.SetActive(true);
    }
    public void VoltarAoMenu()
    {
        creditosCanvas.SetActive(false);
        opcoesCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }
    // Chama quando clica em "Sair"
     public void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }
}