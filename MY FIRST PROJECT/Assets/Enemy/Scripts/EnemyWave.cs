using UnityEngine;
using TMPro; // Troque UnityEngine.UI por TMPro

public class EnemyWave : MonoBehaviour
{
    public GameObject enemyInScena;
    public GameObject enemyWave;
    public GameObject passagem;
    public GameObject contagemHUD; // Arraste a HUD de contagem aqui
    public TextMeshProUGUI contagemText; // Arraste o componente TextMeshProUGUI aqui

    private bool waveActivated = false;
    private bool passagemLiberada = false;

    void Start()
    {
        if (enemyWave != null)
            enemyWave.SetActive(false);

        if (contagemHUD != null)
            contagemHUD.SetActive(true);

        UpdateContagemHUD();
    }

    void Update()
    {
        if (!waveActivated && enemyInScena != null)
        {
            UpdateContagemHUD();
            if (enemyInScena.transform.childCount == 0)
            {
                if (enemyWave != null)
                    enemyWave.SetActive(true);
                waveActivated = true;
                UpdateContagemHUD();
            }
        }
        else if (waveActivated && !passagemLiberada && enemyWave != null)
        {
            UpdateContagemHUD();
            if (enemyWave.transform.childCount == 0)
            {
                if (passagem != null)
                    passagem.SetActive(false);
                passagemLiberada = true;

                if (enemyInScena != null)
                    enemyInScena.SetActive(false);
                if (enemyWave != null)
                    enemyWave.SetActive(false);

                UpdateContagemHUD(0);
            }
        }
    }

    void UpdateContagemHUD(int overrideCount = -1)
    {
        if (contagemHUD != null && contagemText != null)
        {
            int count = overrideCount;
            if (count < 0)
            {
                if (!waveActivated && enemyInScena != null)
                    count = enemyInScena.transform.childCount;
                else if (waveActivated && enemyWave != null)
                    count = enemyWave.transform.childCount;
                else
                    count = 0;
            }
            contagemText.text = $"{count}";
        }
    }
}