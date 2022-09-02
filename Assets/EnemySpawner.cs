using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Spawner spawnerPrefab;
    [SerializeField] List<EnemyBase> enemies;
    [SerializeField] float spawnRange;
    [SerializeField] float spawnCheckRange;
    [SerializeField] int maxEnemies;

    [Space]
    [SerializeField] float spawnA;
    [SerializeField] float spawnB;
    [SerializeField] float spawnC;

    float gameDuration;
    float nextSpawnTime;

    bool killedDummyTanks = false;

    public float SpawnDelay => Mathf.Exp(spawnB - gameDuration / spawnA) + spawnC;

    private void Update()
    {
        if (!killedDummyTanks)
        {
            if (EnemyBase.Enemies.Count == 0)
            {
                killedDummyTanks = true;
            }
            else return;
        }

        if (Time.time > nextSpawnTime)
        {
            EnemyBase enemy = GetRandomWeightedEnemy();
            Vector2 spawnLocation = GetSpawnLocation();

            if (enemy != null)
            {
                if (EnemyBase.Enemies.Count < maxEnemies)
                {
                    Spawner spawnerInstance = Instantiate(spawnerPrefab, spawnLocation, Quaternion.identity);
                    spawnerInstance.spawnObject = enemy.gameObject;
                }

                nextSpawnTime = Time.time + SpawnDelay;
            }
        }

        gameDuration += Time.deltaTime;
    }

    private EnemyBase GetRandomWeightedEnemy()
    {
        float totalWeight = 0.0f;
        List<EnemyBase> validEnemies = new List<EnemyBase>();
        foreach (var enemy in enemies)
        {
            if (gameDuration > enemy.MinSpawnTime)
            {
                validEnemies.Add(enemy);
                totalWeight += enemy.SpawnWeight;
            }
        }

        if (validEnemies.Count == 0) return null;

        float targetWeight = Random.Range(0.0f, totalWeight);
        float countingWeight = 0.0f;
        foreach (var enemy in validEnemies)
        {
            countingWeight += enemy.SpawnWeight;

            if (targetWeight < countingWeight)
            {
                return enemy;
            }
        }
        return validEnemies[validEnemies.Count - 1];
    }

    private Vector2 GetSpawnLocation()
    {
        Vector2 spawnLocation;
        do
        {
            spawnLocation = Random.insideUnitCircle * spawnRange;
        }
        while (Physics2D.OverlapBox(spawnLocation, Vector2.one * spawnCheckRange, 0.0f));
        return spawnLocation;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
        Gizmos.color = Color.white;
    }
}
