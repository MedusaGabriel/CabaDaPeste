using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
        Time.timeScale = 1f; // Resetar o tempo pra voltar ao normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recarregar a cena atual
    }
}