using UnityEngine;
using TMPro;

public class EnemyHuds : MonoBehaviour
{
    public SpawnEnemy spawner;
    public GameObject passagem;  // Objeto bloqueando a passagem
    public GameObject contagemHUD;
    public TextMeshProUGUI contagemText;
    private bool passagemJaLiberada = false;

    void Start()
    {
        if (contagemHUD != null)
            contagemHUD.SetActive(true);
            
        if (passagem != null)
            passagem.SetActive(true);
    }

    void Update()
    {
        UpdateContagemHUD();
        
        if (!passagemJaLiberada && spawner != null && spawner.todasOndasConcluidas)
        {
            LiberarPassagem();
        }
    }

    void LiberarPassagem()
    {
        if (passagem != null)
        {
            passagem.SetActive(false);
            Debug.Log("PASSAGEM LIBERADA!");
        }
        
        passagemJaLiberada = true;
    }

    void UpdateContagemHUD()
    {
        if (contagemHUD != null && contagemText != null && spawner != null)
        {
            if (spawner.todasOndasConcluidas)
            {
                contagemText.text = "0";
            }
            else
            {
                int count = spawner.GetActiveSpawnPoints();
                contagemText.text = $"{count}";
            }
        }
    }
}