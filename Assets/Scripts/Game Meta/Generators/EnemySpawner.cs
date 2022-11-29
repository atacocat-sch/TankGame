using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Spawner spawnerPrefab;
    public List<EnemyBase> enemies;
    public float spawnRange;
    public float spawnCheckRange;
    public int maxEnemies;

    [Space]
    public float spawnA;
    public float spawnB;
    public float spawnC;

    [Space]
    public List<GameObject> preSpawnEnemies;

    float gameDuration;
    float nextSpawnTime;

    bool killedDummyTanks = false;

    public float SpawnDelay => Mathf.Exp(spawnB - gameDuration / spawnA) + spawnC;

    private void Update()
    {
        if (!killedDummyTanks)
        {
            foreach (var enemy in preSpawnEnemies)
            {
                if (enemy.activeSelf)
                {
                    return;
                }
            }

            killedDummyTanks = true;
        }

        if (Time.time > nextSpawnTime)
        {
            EnemyBase enemy = GetRandomWeightedEnemy();
            Vector2 spawnLocation = Util.GetSpawnLocation(spawnRange, spawnCheckRange);

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
        Gizmos.color = Color.white;
    }
}
