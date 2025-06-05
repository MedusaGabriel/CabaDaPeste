using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToActivate; // GameObject que será ativado após o tempo
    
    [SerializeField]
    private float activationDelay = 90f; // 1 minuto e meio em segundos
    
    private bool timerStarted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Inicia o timer quando o objeto for ativado
        StartActivationTimer();
    }

    void OnEnable()
    {
        // Caso o objeto seja ativado depois (não apenas no Start)
        StartActivationTimer();
    }

    private void StartActivationTimer()
    {
        if (!timerStarted)
        {
            timerStarted = true;
            StartCoroutine(ActivationTimer());
        }
    }

    private IEnumerator ActivationTimer()
    {
        Debug.Log("Timer iniciado. Ativando objeto em " + activationDelay + " segundos.");
        
        yield return new WaitForSeconds(activationDelay);
        
        // Ativar o objeto após o tempo de espera
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            Debug.Log("Objeto ativado após " + activationDelay + " segundos!");
        }
        else
        {
            Debug.LogWarning("Nenhum objeto para ativar foi atribuído!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}