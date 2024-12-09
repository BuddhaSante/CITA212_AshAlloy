using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dynamically spawns enemies based on the player's position and the bounds
/// defined by the EnemyBounds object.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawning Settings")]
    [Tooltip("The prefab used for spawning enemies.")]
    [SerializeField] GameObject enemyPrefab; // Enemy prefab to spawn

    [Tooltip("Time interval between spawns, in seconds.")]
    [SerializeField] float spawnInterval = 2f; // Time between spawns

    [Tooltip("Vertical offset for enemy spawns above the player.")]
    [SerializeField] float verticalSpawnOffset = 10f; // Distance above the player for spawning

    [Tooltip("Reference to the player's Transform.")]
    [SerializeField] Transform player; // Player reference

    [Tooltip("Reference to the EnemyBounds object.")]
    [SerializeField] GameObject enemyBounds; // Object defining the stage boundaries

    private Bounds bounds; // The bounds of the EnemyBounds collider

    void Start()
    {
        // Retrieve the bounds of the EnemyBounds object
        PolygonCollider2D collider = enemyBounds.GetComponent<PolygonCollider2D>();
        if (collider != null)
        {
            bounds = collider.bounds;
        }
        else
        {
            Debug.LogError("EnemyBounds object must have a PolygonCollider2D!");
            return;
        }

        // Begin continuously spawning enemies
        StartCoroutine(SpawnEnemies());
    }

    /// <summary>
    /// Continuously spawns enemies within the bounds and above the player.
    /// </summary>
    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Ensure the spawn position stays within the EnemyBounds
            float spawnX = Random.Range(bounds.min.x, bounds.max.x); // Random X within bounds
            float spawnY = Mathf.Min(
                player.position.y + verticalSpawnOffset, // Offset above the player
                bounds.max.y                            // Do not exceed the upper bounds
            );

            Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

            // Spawn the enemy
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Assign the player as the target for the spawned enemy
            EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.SetTarget(player); // Set the target dynamically
            }

            // Wait for the specified interval before spawning the next enemy
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}