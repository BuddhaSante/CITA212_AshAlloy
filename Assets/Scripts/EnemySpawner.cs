using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns enemies in waves based on predefined configurations.
/// Supports looping and randomized spawn positions for variety.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Configuration")]
    [SerializeField] List<WaveConfigSO> waveConfigs; // List of wave configurations
    [SerializeField] float timeBetweenWaves = 2f;    // Time delay between waves
    [SerializeField] bool isLooping = false;         // Whether the waves repeat after completion

    private WaveConfigSO currentWave; // Tracks the current wave being spawned

    void Start()
    {
        // Starts the enemy wave spawning coroutine
        StartCoroutine(SpawnEnemyWaves());
    }

    /// <summary>
    /// Retrieves the current wave configuration.
    /// </summary>
    /// <returns>The active WaveConfigSO.</returns>
    public WaveConfigSO GetCurrentWave()
    {
        return currentWave;
    }

    /// <summary>
    /// Coroutine that handles spawning enemies for all waves in sequence.
    /// Supports looping if enabled.
    /// </summary>
    IEnumerator SpawnEnemyWaves()
    {
        do
        {
            // Iterate through each wave in the configuration list
            foreach (WaveConfigSO wave in waveConfigs)
            {
                currentWave = wave; // Set the active wave

                // Spawn each enemy in the wave
                for (int i = 0; i < currentWave.GetEnemyCount(); i++)
                {
                    SpawnEnemy(currentWave, i); // Spawn enemy with specific configuration

                    // Wait before spawning the next enemy
                    yield return new WaitForSeconds(currentWave.GetRandomSpawnTime());
                }

                // Wait before starting the next wave
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
        while (isLooping); // Repeat if looping is enabled
    }

    /// <summary>
    /// Spawns a single enemy based on the current wave configuration.
    /// Adds random offsets to the position for variety.
    /// </summary>
    /// <param name="wave">The wave configuration.</param>
    /// <param name="index">The index of the enemy in the wave.</param>
    private void SpawnEnemy(WaveConfigSO wave, int index)
    {
        Vector3 spawnPosition = wave.GetStartingWaypoint().position;

        // Add a slight random offset for spawn variation
        spawnPosition += new Vector3(
            Random.Range(-0.5f, 0.5f), // Random offset in X
            Random.Range(-0.5f, 0.5f), // Random offset in Y
            0
        );

        // Instantiate the enemy prefab
        GameObject enemy = Instantiate(
            wave.GetEnemyPrefab(index), // Enemy prefab
            spawnPosition,              // Spawn position
            Quaternion.Euler(0, 0, 180), // Rotation (facing downward for top-down shooters)
            transform                   // Set this spawner as the parent
        );

        // Enable automatic firing for the spawned enemy
        Shooter enemyShooter = enemy.GetComponent<Shooter>();
        if (enemyShooter != null)
        {
            enemyShooter.isFiring = true; // Start firing immediately
        }
    }
}