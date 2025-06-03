using UnityEngine;
using UnityEngine.SceneManagement;
public class BackToMenu : MonoBehaviour
{
    public void GoToMainMenu()
    {
            Debug.Log("Botão Menu Fase pressionado!");
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}