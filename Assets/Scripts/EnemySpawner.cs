using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // List of wave configurations, each containing data for a specific wave of enemies
    [SerializeField] List<WaveConfigSO> waveConfigs;
    
    // Time delay between waves
    [SerializeField] float timeBetweenWaves = 0f;
    [SerializeField] bool isLooping;
    
    // Tracks the current wave being processed
    WaveConfigSO currentWave;
    
    void Start()
    {
        // Starts the coroutine to spawn enemies in waves
        StartCoroutine(SpawnEnemyWaves());
    }

    // Returns the current wave configuration
    public WaveConfigSO GetCurrentWave()
    {
        return currentWave;
    }

    // Coroutine to handle spawning enemies based on wave configurations
    IEnumerator SpawnEnemyWaves()
    {
        do
        {
        // Iterate through each wave in the wave configuration list
        foreach (WaveConfigSO wave in waveConfigs)
            {
                currentWave = wave; // Set the current wave

                // Spawn each enemy in the wave
                for (int i = 0; i < currentWave.GetEnemyCount(); i++)
                {
                    Instantiate(
                        currentWave.GetEnemyPrefab(0), // Gets the enemy prefab from the wave config
                        currentWave.GetStartingWaypoint().position, // Sets the start position
                        Quaternion.Euler(0,0,180), // No rotation
                        transform // Sets this object as the parent
                    );
                    
                    // Waits for a random time before spawning the next enemy
                    yield return new WaitForSeconds(currentWave.GetRandomSpawnTime());
                }

                // Waits before starting the next wave
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
        while(isLooping);
    }
}
