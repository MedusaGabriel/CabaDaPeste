using UnityEngine;
using UnityEngine.SceneManagement;
public class BackToMenu : MonoBehaviour
{
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Garante que o tempo esteja normal
        SceneManager.LoadScene(0); // Vai para a cena do menu (índice 0)
    }
}