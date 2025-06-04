using UnityEngine;
using UnityEngine.SceneManagement;
public class BackToMenu : MonoBehaviour
{
    public GameObject hudRoot;

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void ResumeAndHideHUD()
    {
        Time.timeScale = 1f;
        if (hudRoot != null)
            hudRoot.SetActive(false);
    }
}