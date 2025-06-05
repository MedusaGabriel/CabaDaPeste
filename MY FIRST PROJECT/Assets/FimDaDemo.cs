using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FimDaDemo : MonoBehaviour
{
    [SerializeField]
    private string finalGameSceneName = "FinalGame"; // Nome da cena que será carregada
    
    [SerializeField]
    private float delayBeforeLoading = 2f; // Tempo de espera antes de carregar a cena
    
    [SerializeField]
    private GameObject bossObject; // Referência ao objeto do Boss
    
    private bool isLoadingScene = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Se o bossObject não foi definido no inspector, tenta encontrar pelo nome
        if (bossObject == null)
        {
            bossObject = GameObject.FindWithTag("Boss");
            
            if (bossObject == null)
            {
                Debug.LogError("Boss não encontrado! Certifique-se de atribuí-lo no Inspector ou marcá-lo com a tag 'Boss'.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Verificamos se o Boss está presente e se ele foi desativado ou destruído
        if (bossObject != null)
        {
            // Verificamos se o Boss está inativo (o que indica que foi derrotado)
            if (!bossObject.activeInHierarchy && !isLoadingScene)
            {
                isLoadingScene = true;
                Invoke("LoadFinalGame", delayBeforeLoading);
                Debug.Log("Boss derrotado! Carregando cena final em " + delayBeforeLoading + " segundos.");
            }
        }
    }
    
    private void LoadFinalGame()
    {
        SceneManager.LoadScene(finalGameSceneName);
    }
}