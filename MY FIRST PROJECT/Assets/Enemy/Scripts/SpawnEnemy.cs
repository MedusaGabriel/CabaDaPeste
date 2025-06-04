using UnityEngine;
using System.Collections.Generic;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;

    [Header("Sequência de Ondas")]
    public Transform[] spawnPointsParte1;
    public Transform[] spawnPointsParte2;
    public int poolSize = 20;

    [Header("Estados")]
    public bool parte2Ativa = false;
    public bool todasOndasConcluidas = false;

    private Queue<GameObject> enemyPool = new Queue<GameObject>();
    private int activeEnemies = 0;

    void Start()
    {
        // Inicializa a pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }

        // Desativa todos os spawnPoints da parte 2 no início
        foreach (var sp in spawnPointsParte2)
        {
            sp.gameObject.SetActive(false);
        }

        // Ativa os inimigos da parte 1
        foreach (var sp in spawnPointsParte1)
        {
            SpawnAt(sp.position, sp);
        }
    }

    void Update()
    {
        if (!parte2Ativa && GetActiveSpawnPointsPart(1) == 0)
        {
            Debug.Log("Todos os inimigos da Parte 1 eliminados! Iniciando Parte 2!");
            parte2Ativa = true;

            foreach (var sp in spawnPointsParte2)
            {
                sp.gameObject.SetActive(true);
                SpawnAt(sp.position, sp);
            }
        }

        if (parte2Ativa && !todasOndasConcluidas && GetActiveSpawnPointsPart(2) == 0)
        {
            Debug.Log("TODAS AS ONDAS CONCLUÍDAS!");
            todasOndasConcluidas = true;
        }
    }
    public void OnEnemyDeath(GameObject enemy, Transform spawnPoint = null)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
        activeEnemies--;

        if (spawnPoint != null)
        {
            Debug.Log($"Desativando spawnPoint: {spawnPoint.name}");
            spawnPoint.gameObject.SetActive(false);
        } 
    }

    public void SpawnAt(Vector3 position, Transform spawnPoint = null)
    {
        if (enemyPool.Count == 0) return;

        GameObject enemy = enemyPool.Dequeue();
        enemy.transform.position = position;

        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.SetSpawner(this, spawnPoint);
        }

        enemy.SetActive(true);
        activeEnemies++;

        if (spawnPoint != null)
            spawnPoint.gameObject.SetActive(true);
    }
    public int GetActiveSpawnPointsPart(int part)
    {
        int count = 0;
        Transform[] points = (part == 1) ? spawnPointsParte1 : spawnPointsParte2;

        foreach (var sp in points)
            if (sp != null && sp.gameObject.activeSelf) count++;

        return count;
    }
    public int GetActiveSpawnPoints()
    {
        return GetActiveSpawnPointsPart(1) + GetActiveSpawnPointsPart(2);
    }
}