using UnityEngine;
using TMPro;

public class EnemyWave : MonoBehaviour
{
    public SpawnEnemy enemyInScenaSpawner;
    public SpawnEnemy enemyWaveSpawner;
    public GameObject passagem;
    public GameObject contagemHUD;
    public TextMeshProUGUI contagemText;

    private bool waveActivated = false;
    private bool passagemLiberada = false;

    void Start()
    {
        if (enemyWaveSpawner != null)
            enemyWaveSpawner.gameObject.SetActive(false);

        if (contagemHUD != null)
            contagemHUD.SetActive(true);

        UpdateContagemHUD();
    }

    void Update()
    {
        if (!waveActivated && enemyInScenaSpawner != null)
        {
            UpdateContagemHUD();
            if (enemyInScenaSpawner.GetActiveEnemies() == 0)
            {
                if (enemyWaveSpawner != null)
                    enemyWaveSpawner.gameObject.SetActive(true);
                waveActivated = true;
                UpdateContagemHUD();
            }
        }
        else if (waveActivated && !passagemLiberada && enemyWaveSpawner != null)
        {
            UpdateContagemHUD();
            if (enemyWaveSpawner.GetActiveEnemies() == 0)
            {
                if (passagem != null)
                    passagem.SetActive(false);
                passagemLiberada = true;

                if (enemyInScenaSpawner != null)
                    enemyInScenaSpawner.gameObject.SetActive(false);
                if (enemyWaveSpawner != null)
                    enemyWaveSpawner.gameObject.SetActive(false);

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
                if (!waveActivated && enemyInScenaSpawner != null)
                    count = enemyInScenaSpawner.GetActiveEnemies();
                else if (waveActivated && enemyWaveSpawner != null)
                    count = enemyWaveSpawner.GetActiveEnemies();
                else
                    count = 0;
            }
            contagemText.text = $"{count}";
        }
    }
}