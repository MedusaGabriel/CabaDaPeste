using UnityEngine;

public class IneracaoUI : MonoBehaviour
{
    [Header("Menus")]
    public GameObject pauseMenuCanvas;
    public GameObject optionsMenuCanvas;

    public void ShowOptions()
    {
        optionsMenuCanvas.SetActive(true);
    }

    public void BackToPause()
    {
        optionsMenuCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(true);
    }
}