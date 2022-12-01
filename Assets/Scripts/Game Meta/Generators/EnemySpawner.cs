using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Spawner spawnerPrefab;
    public List<WeightedElement<EnemyBase>> enemies;
    public float spawnRange;
    public float spawnCheckRange;

    [Space]
    public int minEnemies;
    public float spawnPreDelay;
    public float spawnCountScalar;
    public float spawnCountOffset;

    [Space]
    public Team enemyTeam;
    public Signal enemyDiedEvent;

    float gameDuration;
    float nextSpawnTime;

    bool killedDummyTanks = false;
    bool spawningNewWave;

    private void OnEnable()
    {
        enemyDiedEvent.OnRaise += TrySpawnWave;
    }

    private void OnDisable()
    {
        enemyDiedEvent.OnRaise -= TrySpawnWave;
    }

    private void TrySpawnWave()
    {
        StartCoroutine(TrySpawnWaveRoutine());
    }

    private IEnumerator TrySpawnWaveRoutine()
    {
        if (spawningNewWave) yield break;

        print(enemyTeam.players.Count);
        if (enemyTeam.players.Count >= minEnemies) yield break;

        spawningNewWave = true;

        yield return new WaitForSeconds(spawnPreDelay);

        int enemiesToSpawn = (int)(Time.time * spawnCountScalar + spawnCountOffset);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector2 spawnLocation = Util.GetSpawnLocation(spawnRange, spawnCheckRange);
            var spawner = Instantiate(spawnerPrefab, spawnLocation, Quaternion.identity);
            spawner.spawnObject = enemies.Evaluate().gameObject;
        }

        spawningNewWave = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
        Gizmos.color = Color.white;
    }
}
