using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject that defines the configuration for an enemy wave,
/// including movement paths, spawn timings, and enemy types.
/// </summary>
[CreateAssetMenu(menuName = "Wave Config", fileName = "New Wave Config")]
public class WaveConfigSO : ScriptableObject
{
    [Header("Wave Settings")]
    [SerializeField] List<GameObject> enemyPrefabs;      // List of enemy prefabs for this wave
    [SerializeField] Transform pathPrefab;              // Pathway for enemy movement (waypoints)
    [SerializeField] float moveSpeed = 5f;              // Speed at which enemies move along the path

    [Header("Spawn Timing")]
    [SerializeField] float timeBetweenEnemySpawns = 1f; // Base time between spawns
    [SerializeField] float spawnTimeVariance = 0.5f;    // Variance for randomizing spawn intervals
    [SerializeField] float minimumSpawnTime = 0.2f;     // Minimum allowable spawn interval

    /// <summary>
    /// Returns the total number of enemies in the wave.
    /// </summary>
    /// <returns>Number of enemies.</returns>
    public int GetEnemyCount()
    {
        return enemyPrefabs.Count;
    }

    /// <summary>
    /// Returns the enemy prefab at the specified index.
    /// </summary>
    /// <param name="index">Index of the enemy in the list.</param>
    /// <returns>The enemy prefab.</returns>
    public GameObject GetEnemyPrefab(int index)
    {
        return enemyPrefabs[index];
    }

    /// <summary>
    /// Returns the starting waypoint of the path.
    /// </summary>
    /// <returns>The first waypoint Transform.</returns>
    public Transform GetStartingWaypoint()
    {
        return pathPrefab.GetChild(0); // Assumes first child is the starting point
    }

    /// <summary>
    /// Generates a list of waypoints from the path prefab's children.
    /// </summary>
    /// <returns>List of waypoints (Transforms).</returns>
    public List<Transform> GetWaypoints()
    {
        List<Transform> waypoints = new List<Transform>();
        foreach (Transform child in pathPrefab)
        {
            waypoints.Add(child);
        }
        return waypoints;
    }

    /// <summary>
    /// Returns the movement speed for enemies in the wave.
    /// </summary>
    /// <returns>Movement speed.</returns>
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    /// <summary>
    /// Generates a random spawn time for enemies within the configured range.
    /// Clamps the time to avoid excessively short or long intervals.
    /// </summary>
    /// <returns>Randomized spawn interval.</returns>
    public float GetRandomSpawnTime()
    {
        float spawnTime = Random.Range(
            timeBetweenEnemySpawns - spawnTimeVariance,
            timeBetweenEnemySpawns + spawnTimeVariance
        );

        return Mathf.Clamp(spawnTime, minimumSpawnTime, float.MaxValue);
    }
}