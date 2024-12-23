using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles enemy movement along a predefined path using waypoints.
/// Automatically destroys the enemy once the path is completed.
/// </summary>
public class Pathfinder : MonoBehaviour
{
    [Header("Path Settings")]
    private WaveConfigSO waveConfig;          // Stores the configuration of the current wave
    private List<Transform> waypoints;        // Holds the waypoints for the enemy path
    private int waypointIndex = 0;            // Current waypoint the enemy is moving towards

    private EnemySpawner enemySpawner;        // Reference to the enemy spawner script

    void Awake()
    {
        // Finds the EnemySpawner component in the scene
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    void Start()
    {
        if (enemySpawner != null)
        {
            Debug.LogWarning("Pathfinder no longer uses EnemySpawner. Check script dependencies.");
        }

        // Assign waypoints directly if needed (e.g., via the Inspector or a ScriptableObject)
        if (waveConfig != null)
        {
            waypoints = waveConfig.GetWaypoints();
            if (waypoints.Count > 0)
            {
                transform.position = waypoints[waypointIndex].position;
            }
        }
        else
        {
            Debug.LogError("WaveConfigSO is missing! Pathfinder cannot operate without waypoints.");
        }
    }


    void Update()
    {
        FollowPath(); // Continuously move along the path
    }

    /// <summary>
    /// Moves the enemy along the designated path of waypoints.
    /// Destroys the enemy when the path is completed.
    /// </summary>
    void FollowPath()
    {
        if (waypointIndex < waypoints.Count) // Check if there are waypoints left
        {
            Vector3 targetPosition = waypoints[waypointIndex].position; // Target position
            float delta = waveConfig.GetMoveSpeed() * Time.deltaTime;   // Speed adjustment per frame
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, delta); // Move to target

            // Check if the enemy has reached the current waypoint
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                waypointIndex++; // Move to the next waypoint
            }
        }
        else
        {
            Destroy(gameObject); // Destroy the enemy when the path ends
        }
    }
}