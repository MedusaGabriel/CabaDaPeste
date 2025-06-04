using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    [Header("Níveis")]
    public GameObject enemyLevel1;
    public GameObject enemyLevel2;
    
    [Header("Passagens")]
    public GameObject passagemLevel1Para2; // Passagem que foi aberta pelo EnemyHuds
    
    [Header("Configurações")]
    public string playerTag = "Player";
    
    private bool transicaoRealizada = false;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se é o Player e se a transição ainda não foi realizada
        if (!transicaoRealizada && other.CompareTag(playerTag))
        {
            RealizarTransicao();
        }
    }
    
    private void RealizarTransicao()
    {
        Debug.Log("Player atravessou! Realizando transição para o Level 2");
        
        // Fecha a passagem para o Level 1
        if (passagemLevel1Para2 != null)
        {
            passagemLevel1Para2.SetActive(true);
            Debug.Log("Passagem para Level 1 fechada!");
        }
        
        // Desativa o Level 1
        if (enemyLevel1 != null)
        {
            enemyLevel1.SetActive(false);
            Debug.Log("Level 1 desativado!");
        }
        
        // Ativa o Level 2
        if (enemyLevel2 != null)
        {
            enemyLevel2.SetActive(true);
            Debug.Log("Level 2 ativado!");
        }
        
        // Marca a transição como realizada para não repetir
        transicaoRealizada = true;
        
        // Opcional: desativa este objeto após a transição
        gameObject.SetActive(false);
    }
}