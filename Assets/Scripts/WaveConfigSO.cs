using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave Config", fileName = "New Wave Config")]
public class WaveConfigSO : ScriptableObject
{
    // List of enemy prefabs for the wave
    [SerializeField] List<GameObject> enemyPrefabs;
    
    // Path for enemy waypoints
    [SerializeField] Transform pathPrefab;
    
    // Configuration for enemy movement speed and spawn timings
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float timeBetweenEnemySpawns = 1f;
    [SerializeField] float spawnTimeVariance = 0f;
    [SerializeField] float minimumSpawnTime = 0.2f;

    // Returns the count of enemies in the wave
    public int GetEnemyCount()
    {
        return enemyPrefabs.Count;
    }

    // Returns the enemy prefab at a specific index
    public GameObject GetEnemyPrefab(int index)
    {
        return enemyPrefabs[index];
    }

    // Returns the starting waypoint of the path
    public Transform GetStartingWaypoint()
    {
        return pathPrefab.GetChild(0);
    }

    // Creates a list of waypoints from the path prefab's children
    public List<Transform> GetWaypoints()
    {
        List<Transform> waypoints = new List<Transform>();
        foreach (Transform child in pathPrefab)
        {
            waypoints.Add(child);
        }
        return waypoints;
    }

    // Returns the movement speed for enemies
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    // Generates a random spawn time, clamped to avoid excessive delays
    public float GetRandomSpawnTime()
    {
        float spawnTime = Random.Range(timeBetweenEnemySpawns - spawnTimeVariance, 
                                       timeBetweenEnemySpawns + spawnTimeVariance);
        return Mathf.Clamp(spawnTime, minimumSpawnTime, float.MaxValue);
    }
}
