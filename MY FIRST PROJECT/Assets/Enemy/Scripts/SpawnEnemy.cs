using UnityEngine;
using System.Collections.Generic;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPointsInScena;
    public Transform[] spawnPointsWave;
    public int poolSize = 20;

    private Queue<GameObject> enemyPool = new Queue<GameObject>();
    private int activeEnemies = 0;

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }

        for (int i = 0; i < spawnPointsInScena.Length; i++)
        {
            SpawnAt(spawnPointsInScena[i].position);
        }
    }

    public void SpawnAt(Vector3 position)
    {
        if (enemyPool.Count == 0) return;
        GameObject enemy = enemyPool.Dequeue();
        enemy.transform.position = position;
        enemy.SetActive(true);
        activeEnemies++; // Incrementa ao ativar
    }

    public void OnEnemyDeath(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
        activeEnemies--; // Decrementa ao desativar
    }

    public int GetActiveEnemies()
    {
        return activeEnemies;
    }
}