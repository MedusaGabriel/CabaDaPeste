using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    private bool levelComplete = false;

    void Update()
    {
        if (!levelComplete && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            levelComplete = true;
            // Avança para a próxima cena baseada na ordem definida no Build Settings
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}