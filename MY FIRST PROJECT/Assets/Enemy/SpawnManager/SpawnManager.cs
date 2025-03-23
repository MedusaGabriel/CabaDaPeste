using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public int maxEnemies = 10;
    public float spawnInterval = 2f; 

    private List<GameObject> enemyPool; 
    private List<SpawnPoint> spawnPoints; 

    private void Start()
    {
        enemyPool = new List<GameObject>();
        for (int i = 0; i < maxEnemies; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }

        spawnPoints = new List<SpawnPoint>(FindObjectsOfType<SpawnPoint>());

        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    private void SpawnEnemy()
    {
        GameObject enemy = GetPooledEnemy();
        if (enemy != null)
        {
            SpawnPoint spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            enemy.transform.position = spawnPoint.transform.position;
            enemy.SetActive(true);
        }
    }

    private GameObject GetPooledEnemy()
    {
        foreach (GameObject enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                return enemy;
            }
        }
        return null; 
    }
}